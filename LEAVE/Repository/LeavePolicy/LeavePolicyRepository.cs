using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Models.Models.Entity;
using LEAVE.Dto;
using Microsoft.EntityFrameworkCore;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace LEAVE.Repository.LeavePolicy
{
    public class LeavePolicyRepository : ILeavePolicyRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;
        public LeavePolicyRepository(EmployeeDBContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        private static IEnumerable<string> SplitStrings_XML(string list, char delimiter = ',') =>
          string.IsNullOrWhiteSpace(list)
              ? Enumerable.Empty<string>()
              : list.Split(delimiter)
                    .Select(item => item.Trim())
                    .Where(item => !string.IsNullOrEmpty(item));

        public async Task<List<object>> FillLeavepolicyAsync(int SecondEntityId, int EmpId)
        {

            var transactionIdTask = _httpClient.GetAsync("http://localhost:5194/gateway/Employee/GetTransactionIdByTransactionType?transactionType=LeavePolicy");
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
                var leavePolicies = await _context.LeavePolicyMasters
                    .GroupBy(a => new { a.LeavePolicyMasterId, a.InstId, a.PolicyName, a.Blockmultiunapprovedleave })
                    .Select(g => new
                    {
                        g.Key.LeavePolicyMasterId,
                        g.Key.InstId,
                        g.Key.PolicyName,
                        g.Key.Blockmultiunapprovedleave
                    })
                    .ToListAsync<object>();

                return leavePolicies;
            }

            // No direct access, check entity levels
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

            // Now fetch LeavePolicyMaster where LeavePolicyMasterId is in newhigh
            var finalLeavePolicies = await _context.LeavePolicyMasters
                .Where(a => newhigh.Contains(a.LeavePolicyMasterId))
                .GroupBy(a => new { a.LeavePolicyMasterId, a.InstId, a.PolicyName, a.Blockmultiunapprovedleave })
                .Select(g => new
                {
                    g.Key.LeavePolicyMasterId,
                    g.Key.InstId,
                    g.Key.PolicyName,
                    g.Key.Blockmultiunapprovedleave
                })
                .ToListAsync<object>();
            return finalLeavePolicies;
        }


        public async Task<int?> CreatepolicyAsync(CreatePolicyDto createPolicyDto)
        {

            var existingPolicy = await _context.LeavePolicyMasters
                .FirstOrDefaultAsync(x => x.LeavePolicyMasterId == createPolicyDto.LeavePolicyMasterID);

            if (existingPolicy == null)
            {

                var newPolicy = new LeavePolicyMaster
                {
                    InstId = createPolicyDto.Inst_Id,
                    PolicyName = createPolicyDto.Name,
                    Blockmultiunapprovedleave = createPolicyDto.BlockMultiUnapprovedLeaves,
                    EntryBy = createPolicyDto.EntryBy,
                    EmpId = createPolicyDto.EmpId
                };

                await _context.LeavePolicyMasters.AddAsync(newPolicy);
                await _context.SaveChangesAsync();
                return newPolicy.LeavePolicyMasterId;
            }
            else
            {

                existingPolicy.PolicyName = createPolicyDto.Name;
                existingPolicy.Blockmultiunapprovedleave = createPolicyDto.BlockMultiUnapprovedLeaves;

                _context.LeavePolicyMasters.Update(existingPolicy);
                await _context.SaveChangesAsync();
                return existingPolicy.LeavePolicyMasterId;
            }
        }


        public async Task<List<object>> FillleaveAsync(int SecondEntityId, int EmpId)
        {


            var transactionIdTask = _httpClient.GetAsync("http://localhost:5194/gateway/Employee/GetTransactionIdByTransactionType?transactionType=Leave");
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
            List<object> finalLeavePolicies = new List<object>();

            if (hasAccessRights)
            {
                // Access granted, fetch Leave data
                var leaveData = await _context.HrmLeaveMasters
                    .Where(l => l.Active == 1)
                    .GroupBy(l => new { l.LeaveMasterId, l.LeaveCode, l.Description })
                    .Select(g => new
                    {
                        Leave_type_id = g.Key.LeaveMasterId,
                        Leave_desc = g.Key.LeaveCode,
                        Descriptions = g.Key.Description
                    })
                    .ToListAsync();

                finalLeavePolicies.AddRange(leaveData);
            }
            else
            {
                // No direct access, check entity levels
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

                // Now fetch LeavePolicyMaster where LeavePolicyMasterId is in newhigh
                var leavePolicies = await _context.HrmLeaveMasters
                    .Where(l => l.Active == 1 && newhigh.Contains(l.LeaveMasterId))
                    .GroupBy(l => new { l.LeaveMasterId, l.LeaveCode, l.Description })
                    .Select(g => new
                    {
                        Leave_type_id = g.Key.LeaveMasterId,
                        Leave_desc = g.Key.LeaveCode,
                        Descriptions = g.Key.Description
                    })
                    .ToListAsync();

                finalLeavePolicies.AddRange(leavePolicies);
            }

            return finalLeavePolicies;
        }

        public async Task<List<object>> FillInstatntLimitAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {
            var query = from l in _context.LeavePolicyInstanceLimits
                        join m in _context.LeavePolicyMasters
                                        on l.LeavePolicyMasterId equals m.LeavePolicyMasterId
                        join rel in _context.HrEmployeeUserRelations
            on l.EntryBy equals rel.UserId into relJoin
                        from relLeft in relJoin.DefaultIfEmpty()
                        join emp in _context.HrEmpMasters
                            on relLeft.EmpId equals emp.EmpId into empJoin
                        from empLeft in empJoin.DefaultIfEmpty()
                        join leave in _context.HrmLeaveMasters
                            on l.LeaveId equals leave.LeaveMasterId
                        where l.LeavePolicyMasterId == LeavePolicyMasterID &&
                              (l.LeavePolicyInstanceLimitId == LeavePolicyInstanceLimitID || LeavePolicyInstanceLimitID == 0)
                        select new
                        {
                            m.PolicyName,
                            EmployeeName = empLeft.FirstName,
                            l.LeaveId,
                            LeaveName = leave.Description,
                            l.LeavePolicyInstanceLimitId,
                            l.LeavePolicyMasterId,
                            l.MaximamLimit,
                            l.MinimumLimit,
                            l.IsHolidayIncluded,
                            l.IsWeekendIncluded,
                            l.NoOfDayIncludeHoliday,
                            l.NoOfDayIncludeWeekEnd,
                            l.Daysbtwnleaves,
                            l.Salaryadvancedays,
                            l.Roledeligationdays,
                            l.Attachmentdays,
                            l.ProbationMl,
                            l.NewjoinMl,
                            l.OtherMl,
                            l.Halfday,
                            l.PredatedApplication,
                            l.Daysbtwndifferentleave,
                            l.Daysleaveclubbing,
                            l.Predateddayslimit,
                            l.Returndate,
                            l.Autotravelapprove,
                            l.Leaveinclude,
                            l.Contactdetails,
                            l.Leavereason,
                            l.Approvremark,
                            l.Nobalance,
                            l.Applyafterallleave,
                            l.Applyafterleaveids,
                            l.Showinapplicationonly,
                            l.Rejectremark,
                            l.Predatedapplicationproxy,
                            l.Predateddayslimitproxy,
                            l.PredatedapplicationAttendance,
                            l.PredatedapplicationAttendanceDays,
                            l.FutureleaveApplication,
                            l.FutureleaveApplicationDays,

                            // List of Compensatory Leave IDs
                            CompLeaveID = (from notInclude in _context.LeavePolicyLeaveNotIncludes
                                           where notInclude.LeavePolicyInstanceLimitId == l.LeavePolicyInstanceLimitId
                                           select notInclude.LeaveId.ToString()).ToList(),

                            // List of Leave Codes
                            Leave = (from notInclude in _context.LeavePolicyLeaveNotIncludes
                                     join leaveMaster in _context.HrmLeaveMasters
                                     on notInclude.LeaveId equals leaveMaster.LeaveMasterId
                                     where notInclude.LeavePolicyInstanceLimitId == l.LeavePolicyInstanceLimitId
                                     select leaveMaster.LeaveCode).ToList()
                        };

            var data = await query.ToListAsync();

            var result = data.Select(d => new
            {
                d.PolicyName,
                d.EmployeeName,
                d.LeaveId,
                d.LeaveName,
                d.LeavePolicyInstanceLimitId,
                d.LeavePolicyMasterId,
                d.MaximamLimit,
                d.MinimumLimit,

                // Fixed: Always 1 or 0
                d.IsHolidayIncluded,
                d.IsWeekendIncluded,

                d.NoOfDayIncludeHoliday,
                d.NoOfDayIncludeWeekEnd,
                d.Daysbtwnleaves,
                d.Salaryadvancedays,
                d.Roledeligationdays,
                d.Attachmentdays,
                d.ProbationMl,
                d.NewjoinMl,
                d.OtherMl,
                d.Halfday,
                d.PredatedApplication,
                d.Daysbtwndifferentleave,
                d.Daysleaveclubbing,
                d.Predateddayslimit,
                d.Returndate,
                d.Autotravelapprove,
                d.Leaveinclude,
                d.Contactdetails,
                d.Leavereason,
                d.Approvremark,
                d.Nobalance,
                d.Applyafterallleave,
                d.Applyafterleaveids,
                d.Showinapplicationonly,
                d.Rejectremark,
                d.Predatedapplicationproxy,
                d.Predateddayslimitproxy,
                d.PredatedapplicationAttendance,
                d.PredatedapplicationAttendanceDays,
                d.FutureleaveApplication,
                d.FutureleaveApplicationDays,

                // Convert lists to strings
                CompLeaveID = string.Join(",", d.CompLeaveID),
                Leave = string.Join("+", d.Leave)
            })
            .Cast<object>()
            .ToList();

            return result;
        }
    }
}
