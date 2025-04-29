using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models.Models.Entity;
using LEAVE.Dto;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace LEAVE.Repository.LeaveMaster
{
    public class LeaveMasterRepository : ILeaveMasterRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;
        private readonly EmployeeSettings _employeeSettings;
        public LeaveMasterRepository(EmployeeDBContext dbContext, HttpClient httpClient)
        {
            _context = dbContext;
            _httpClient = httpClient;
        }

        private static IEnumerable<string> SplitStrings_XML(string list, char delimiter = ',') =>
            string.IsNullOrWhiteSpace(list)
                ? Enumerable.Empty<string>()
                : list.Split(delimiter)
                      .Select(item => item.Trim())
                      .Where(item => !string.IsNullOrEmpty(item));
        private static string FormatDate(DateTime? date, string format)
        {
            return date.HasValue ? date.Value.ToString(format) : string.Empty; // Or any other default value
        }
        private async Task<List<LeaveDetailModelDto>> FillLeaveMasterAsyncNoAccessMode(int empId, int roleId, int? lnklev, int transid)
        {

            var newHigh = await GetNewHighListAsync(empId, roleId, transid, lnklev);

            // Final Fetch
            return await _context.HrmLeaveMasters
                .Where(l => newHigh.Contains(l.LeaveMasterId))
                .GroupJoin(_context.AdmUserMasters,
                    l => l.CreatedBy,
                    u => u.UserId,
                    (l, users) => new { Leave = l, User = users.FirstOrDefault() })
                .Select(x => new LeaveDetailModelDto
                {
                    UserName = x.User.UserName,
                    LeaveMasterId = x.Leave.LeaveMasterId,
                    LeaveCode = x.Leave.LeaveCode,
                    Description = x.Leave.Description,
                    PayType = x.Leave.PayType,
                    LeaveUnit = x.Leave.LeaveUnit,
                    Active = x.Leave.Active,
                    CreatedDate = x.Leave.CreatedDate
                })
                .ToListAsync();
        }
        public async Task<List<long?>> GetNewHighListAsync(int empId, int roleId, long transid, int? lnklev)
        {
            // Step 1: Get the EmpEntity for the given empId
            var empEntity = await _context.HrEmpMasters
                .Where(h => h.EmpId == empId)
                .Select(h => h.EmpEntity)
                .FirstOrDefaultAsync();

            // Step 2: Split EmpEntity into ctnew
            var ctnew = SplitStrings_XML(empEntity)
                .Select((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 })
                .ToList();

            // Step 3: Get access rights for the role
            var accessRights = await _context.EntityAccessRights02s
                .Where(s => s.RoleId == roleId)
                .ToListAsync();

            // Step 4: Build applicableFinal list from access rights
            var applicableFinal = accessRights
                .Where(s => !string.IsNullOrEmpty(s.LinkId))
                .SelectMany(s => SplitStrings_XML(s.LinkId),
                            (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel })
                .ToList();

            // Step 5: Add ctnew items if LinkLevel >= lnklev
            if (lnklev > 0)
            {
                applicableFinal.AddRange(ctnew.Where(c => c.LinkLevel >= lnklev));
            }

            // Step 6: Convert to HashSet of long values (to ensure uniqueness)
            var applicableFinalSetLong = applicableFinal
                .Select(a => (long?)Convert.ToInt64(a.Item))
                .ToHashSet();

            // Step 7: Get EntityApplicable00Final
            var entityApplicable00Final = await _context.EntityApplicable00s
                .Where(e => e.TransactionId == transid)
                .Select(e => new { e.LinkId, e.LinkLevel, e.MasterId })
                .ToListAsync();

            // Step 8: Move applicableFinal to client-side evaluation by calling AsEnumerable
            var applicableFinalItems = applicableFinal.Select(a => a.Item).ToList(); // We use this list for comparison

            // Step 9: Get applicableFinal02Emp (client-side evaluation)
            var applicableFinal02Emp = await (
                from emp in _context.EmployeeDetails
                join ea in _context.EntityApplicable01s on emp.EmpId equals ea.EmpId
                join hlv in _context.HighLevelViewTables on emp.LastEntity equals hlv.LastEntityId
                where ea.TransactionId == transid
                let levelOneId = hlv.LevelOneId.ToString()  // Convert to string for client-side comparison
                where applicableFinalItems.Contains(levelOneId)  // Perform the comparison in memory
                select ea.MasterId
            ).Distinct().ToListAsync();

            // Step 10: Combine results
            var newhigh = applicableFinalSetLong
                .Union(applicableFinal02Emp.Select(emp => (long?)emp))  // Assuming MasterId is the result you want
                .ToList();

            return newhigh;
        }

        public async Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId)
        {
            // Execute the external API calls concurrently
            var transactionIdTask = _httpClient.GetAsync("http://localhost:5194/gateway/Employee/GetTransactionIdByTransactionType?transactionType=Leave");
            var linkLevelTask = _httpClient.GetAsync($"http://localhost:5194/gateway/Employee/GetLinkLevelByRoleId?roleId={secondEntityId}");
            var entityAccessRightsTask = _httpClient.GetAsync($"http://localhost:5194/gateway/Employee/GetEntityAccessRights?roleId={secondEntityId}&linkSelect={linkLevelTask.Result}");

            // Wait for all tasks to complete
            await Task.WhenAll(transactionIdTask, linkLevelTask, entityAccessRightsTask);

            // Parse the results
            var transIdString = await transactionIdTask.Result.Content.ReadAsStringAsync();
            if (!int.TryParse(transIdString, out int transId))
            { 
                throw new InvalidOperationException("Failed to parse transaction ID.");
            }

            var linkLevelString = await linkLevelTask.Result.Content.ReadAsStringAsync();
            if (!int.TryParse(linkLevelString, out int linkLevel))
            {
                throw new InvalidOperationException("Failed to parse link level.");
            }

            var entityAccess = await entityAccessRightsTask.Result.Content.ReadAsStringAsync();
            bool hasAccessRights = !string.IsNullOrEmpty(entityAccess) && entityAccess.Any();

            if (hasAccessRights)
            {
                // Fetch leave details if entity access rights are valid
                return await _context.HrmLeaveMasters
                    .GroupJoin(_context.AdmUserMasters,
                        l => l.CreatedBy,
                        u => u.UserId,
                        (l, users) => new { Leave = l, User = users.FirstOrDefault() })
                    .Select(x => new LeaveDetailModelDto
                    {
                        UserName = x.User.UserName,
                        LeaveMasterId = x.Leave.LeaveMasterId,
                        LeaveCode = x.Leave.LeaveCode,
                        Description = x.Leave.Description,
                        PayType = x.Leave.PayType,
                        LeaveUnit = x.Leave.LeaveUnit,
                        Active = x.Leave.Active,
                        CreatedDate = x.Leave.CreatedDate
                    })
                    .ToListAsync();
            }
            else
            {
                // If no entity access, fetch the result from dd method
                return await FillLeaveMasterAsyncNoAccessMode(empId, secondEntityId, linkLevel, transId);
            }
        }

        public async Task<List<object>> FillvaluetypesAsync(string type)
        {
            if (type == "Entitlement")
            {
                var data = await _context.HrmValueTypes
                    .Where(v => v.Type == type && v.Code != "N")
                    .Select(v => new
                    {
                        v.Value,
                        v.Description
                    })
                    .ToListAsync();

                return data.Cast<object>().ToList();
            }
            else if (type == "ReasonMaster")
            {
                var data = await _context.GeneralCategories
                    .Select(g => new
                    {
                        g.Id,
                        g.Description,
                        g.Code
                    })
                    .ToListAsync();

                return data.Cast<object>().ToList();
            }
            else
            {
                var data = await _context.HrmValueTypes
                    .Where(v => v.Type == type)
                    .Select(v => new
                    {
                        v.Value,
                        v.Description
                    })
                    .ToListAsync();
                return data.Cast<object>().ToList();
            }
        }

        public async Task<int?> CreateMasterAsync(CreateMasterDto dto)
        {

            string camelCaseDescription = string.IsNullOrWhiteSpace(dto.Description)
     ? dto.Description
     : dto.Description.ToUpper();


            // If updating
            if (dto.MasterId != 0)
            {
                var existing = await _context.HrmLeaveMasters
                    .FirstOrDefaultAsync(x => x.LeaveMasterId == dto.MasterId);

                if (existing != null)
                {
                    existing.LeaveCode = dto.LeaveCode;
                    existing.Description = camelCaseDescription;
                    existing.PayType = dto.PayType;
                    existing.LeaveUnit = dto.LeaveUnit;
                    existing.Active = dto.Active;
                    existing.Colour = dto.Colour;

                    await _context.SaveChangesAsync();
                    return existing.LeaveMasterId;
                }
            }
            else
            {

                var exists = await _context.HrmLeaveMasters
                    .AnyAsync(x => x.LeaveCode == dto.LeaveCode);

                if (!exists)
                {
                    var newEntity = new HrmLeaveMaster
                    {
                        LeaveCode = dto.LeaveCode,
                        Description = camelCaseDescription,
                        PayType = dto.PayType,
                        LeaveUnit = dto.LeaveUnit,
                        Active = dto.Active,
                        CreatedBy = dto.CreatedBy,
                        Colour = dto.Colour,
                        CreatedDate = DateTime.UtcNow
                    };

                    _context.HrmLeaveMasters.Add(newEntity);
                    await _context.SaveChangesAsync();

                    return newEntity.LeaveMasterId;
                }
            }

            return null;
        }
        public async Task<List<object>> FillbasicsettingsAsync(int Masterid, int SecondEntityId, int EmpId)
        {
            if (Masterid == 0)
            {
                //var transId = await GetTransactionIdByTransactionType("Leave_BS");
                //var lnklev = await GetLinkLevelByRoleId(SecondEntityId);

                //bool hasAccess = await _context.EntityAccessRights02s
                //    .AnyAsync(s => s.RoleId == SecondEntityId && s.LinkLevel == 15);
                var transactionIdTask = _httpClient.GetAsync("http://localhost:5194/gateway/Employee/GetTransactionIdByTransactionType?transactionType=Leave_BS");
                var linkLevelTask = _httpClient.GetAsync($"http://localhost:5194/gateway/Employee/GetLinkLevelByRoleId?roleId={SecondEntityId}");
                var entityAccessRightsTask = _httpClient.GetAsync($"http://localhost:5194/gateway/Employee/GetEntityAccessRights?roleId={SecondEntityId}&linkSelect={linkLevelTask.Result}");

                // Wait for all tasks to complete
                await Task.WhenAll(transactionIdTask, linkLevelTask, entityAccessRightsTask);

                // Parse the results
                var transIdString = await transactionIdTask.Result.Content.ReadAsStringAsync();
                if (!int.TryParse(transIdString, out int transId))
                {
                    throw new InvalidOperationException("Failed to parse transaction ID.");
                }

                var linkLevelString = await linkLevelTask.Result.Content.ReadAsStringAsync();
                if (!int.TryParse(linkLevelString, out int lnklev))
                {
                    throw new InvalidOperationException("Failed to parse link level.");
                }

                var entityAccess = await entityAccessRightsTask.Result.Content.ReadAsStringAsync();
                bool hasAccessRights = !string.IsNullOrEmpty(entityAccess) && entityAccess.Any();

                if (hasAccessRights)
                {

                    
                    var result = await (from b in _context.HrmLeaveBasicSettings
                                        join a in _context.AdmUserMasters
                                            on b.CreatedBy equals a.UserId into gj
                                        from a in gj.DefaultIfEmpty()
                                        select new
                                        {
                                            UserName = a != null ? a.UserName : null,
                                            SettingsId = b.SettingsId,
                                            SettingsName = b.SettingsName,
                                            SettingsDescription = b.SettingsDescription,
                                            CreatedDate = b.CreatedDate.HasValue ? FormatDate(b.CreatedDate, _employeeSettings.DateFormat): null
                                        })
                                        .Distinct()
                                        .ToListAsync<object>();

                    return result;
                }



           



                var empEntity = await _context.HrEmpMasters
                    .Where(h => h.EmpId == EmpId)
                    .Select(h => h.EmpEntity)
                    .FirstOrDefaultAsync();

                var ctnew = SplitStrings_XML(empEntity, ',')
                    .Select((item, index) => new LinkItemDto { Item = item, LinkLevel = index + 2 })
                    .Where(c => !string.IsNullOrEmpty(c.Item))
                    .ToList();

                var applicableFinal = await _context.EntityAccessRights02s
                    .Where(s => s.RoleId == SecondEntityId)
                    .SelectMany(s => SplitStrings_XML(s.LinkId, default),
                    (s, item) => new LinkItemDto { Item = item, LinkLevel = s.LinkLevel })
                    .ToListAsync();

                if (lnklev > 0)
                {
                    applicableFinal.AddRange(
                        ctnew.Where(c => c.LinkLevel >= lnklev)
                             .Select(c => new LinkItemDto { Item = c.Item, LinkLevel = c.LinkLevel })
                    );
                }

                var applicableFinalSetLong = applicableFinal
                    .Select(a => (long?)Convert.ToInt64(a.Item))
                    .ToHashSet();

                var entityApplicable00Final = await _context.EntityApplicable00s
                    .Where(e => e.TransactionId == transId)
                    .Select(e => new { e.LinkId, e.LinkLevel, e.MasterId })
                    .ToListAsync();

                var applicableFinal02 = applicableFinal.ToList();

                var applicableFinal02Emp = await (
                    from emp in _context.EmployeeDetails
                    join ea in _context.EntityApplicable01s on emp.EmpId equals ea.EmpId
                    join hlv in _context.HighLevelViewTables on emp.LastEntity equals hlv.LastEntityId
                    join af2 in applicableFinal02 on hlv.LevelOneId.ToString() equals af2.Item into af2LevelOne
                    from af2L1 in af2LevelOne.DefaultIfEmpty()
                    where ea.TransactionId == transId
                    select ea.MasterId
                ).Distinct().ToListAsync();

                var newhigh = entityApplicable00Final
                    .Where(e => applicableFinalSetLong.Contains(e.LinkId) || e.LinkLevel == 15)
                    .Select(e => e.MasterId)
                    .Union(applicableFinal02Emp)
                    .Distinct()
                    .ToList();

                var finalResult = await (from b in _context.HrmLeaveBasicSettings
                                         join a in _context.AdmUserMasters
                                             on b.CreatedBy equals a.UserId into gj
                                         from a in gj.DefaultIfEmpty()
                                         where newhigh.Contains(b.SettingsId)
                                         select new
                                         {
                                             UserName = a != null ? a.UserName : null,
                                             SettingsId = b.SettingsId,
                                             SettingsName = b.SettingsName,
                                             SettingsDescription = b.SettingsDescription,
                                     CreatedDate = b.CreatedDate.HasValue ? FormatDate(b.CreatedDate, _employeeSettings.DateFormat): null
                                         })
                                         .Distinct()
                                         .ToListAsync<object>();

                return finalResult;
            }
            else
            {
                var finalResult = await (from lmsl in _context.HrmLeaveMasterandsettingsLinks
                                         join lm in _context.HrmLeaveMasters
                                             on lmsl.LeaveMasterId equals lm.LeaveMasterId
                                         join lbs in _context.HrmLeaveBasicSettings
                                             on lmsl.SettingsId equals lbs.SettingsId
                                         join um in _context.AdmUserMasters
                                             on lbs.CreatedBy equals um.UserId into userJoin
                                         from um in userJoin.DefaultIfEmpty()
                                         where lm.LeaveMasterId == Masterid
                                         select new
                                         {
                                             UserName = um != null ? um.UserName : null,
                                             SettingsId = lbs.SettingsId,
                                             SettingsName = lbs.SettingsName,
                                             SettingsDescription = lbs.SettingsDescription,
                                             CreatedDate = lbs.CreatedDate.HasValue ? lbs.CreatedDate.Value.ToString("dd/MM/yyyy") : null
                                         })
                                         .Distinct()
                                         .ToListAsync<object>();

                return finalResult;
            }
        }
    }


}
