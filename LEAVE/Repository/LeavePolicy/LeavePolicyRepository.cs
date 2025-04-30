using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Models.Models.Entity;
using LEAVE.Dto;
using LEAVE.Helpers;
using LEAVE.Helpers.AccessMetadataService;
using Microsoft.EntityFrameworkCore;

namespace LEAVE.Repository.LeavePolicy
{
    public class LeavePolicyRepository : ILeavePolicyRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly ExternalApiService _externalApiService;
        private IAccessMetadataService _accessMetadataService;
        public LeavePolicyRepository(EmployeeDBContext context, ExternalApiService externalApiService, IAccessMetadataService accessMetadataService)
        {
            _context = context;
            _externalApiService = externalApiService;
            _accessMetadataService = accessMetadataService;
        }

        private static IEnumerable<string> SplitStrings_XML(string list, char delimiter = ',') =>
          string.IsNullOrWhiteSpace(list)
              ? Enumerable.Empty<string>()
              : list.Split(delimiter)
                    .Select(item => item.Trim())
                    .Where(item => !string.IsNullOrEmpty(item));

        public async Task<List<object>> FillLeavepolicyAsync(int SecondEntityId, int EmpId)
        {
            var accessMetadata = await _accessMetadataService.GetAccessMetadataAsync("LeavePolicy", SecondEntityId, EmpId);

            if (accessMetadata.HasAccessRights)
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
            else
            {
                return await FillLeaveMasterAsyncNoAccessMode(EmpId, SecondEntityId, accessMetadata.LinkLevel, accessMetadata.TransactionId);
            }

        }
        private async Task<List<object>> FillLeaveMasterAsyncNoAccessMode(int empId, int roleId, int? lnklev, int transid)
        {

            var newHigh = await _accessMetadataService.GetNewHighListAsync(empId, roleId, transid, lnklev);

            var finalLeavePolicies = await _context.LeavePolicyMasters
                   .Where(a => newHigh.Contains(a.LeavePolicyMasterId))
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
        private async Task<List<object>> FinalLeavePoliciesNoAccessMode(int empId, int roleId, int? lnklev, int transid)
        {

            var newHigh = await _accessMetadataService.GetNewHighListAsync(empId, roleId, transid, lnklev);

            var leavePolicies = await _context.HrmLeaveMasters
                    .Where(l => l.Active == 1 && newHigh.Contains(l.LeaveMasterId))
                    .GroupBy(l => new { l.LeaveMasterId, l.LeaveCode, l.Description })
                    .Select(g => new
                    {
                        Leave_type_id = g.Key.LeaveMasterId,
                        Leave_desc = g.Key.LeaveCode,
                        Descriptions = g.Key.Description
                    })
                    .ToListAsync<object>();

            leavePolicies.AddRange(leavePolicies);
            return leavePolicies;


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
            var accessMetadata = await _accessMetadataService.GetAccessMetadataAsync("Leave", SecondEntityId, EmpId);

            List<object> finalLeavePolicies = new List<object>();

            if (accessMetadata.HasAccessRights)
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
                return await FinalLeavePoliciesNoAccessMode(EmpId, SecondEntityId, accessMetadata.LinkLevel, accessMetadata.TransactionId);
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
