using EMPLOYEE_INFORMATION.Data;
using LEAVE.Dto;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace LEAVE.Repository.LeaveMaster
{
    public class LeaveMasterRepository : ILeaveMasterRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;
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
    }


}
