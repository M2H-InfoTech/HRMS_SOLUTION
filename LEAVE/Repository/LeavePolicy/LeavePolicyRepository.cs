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


        public async Task<List<object>> EditFillInstatntLimitLeaveAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {
            var result = await (
                from l in _context.LeavePolicyInstanceLimits
                join m in _context.LeavePolicyMasters
                    on l.LeavePolicyMasterId equals m.LeavePolicyMasterId
                join n in _context.LeavePolicyLeaveIncludes
                    on l.LeavePolicyInstanceLimitId equals n.LeavePolicyInstanceLimitId into leaveIncludes
                from n in leaveIncludes.DefaultIfEmpty() // Left join to include rows even when there's no match
                where l.LeavePolicyMasterId == LeavePolicyMasterID &&
                      (l.LeavePolicyInstanceLimitId == LeavePolicyInstanceLimitID || LeavePolicyInstanceLimitID == 0)
                select new
                {
                    n.Leavestatus,
                    leaveFromdays = n.Fromdays,
                    leaveLeavetype = n.Leavetype,
                    n.LeaveDays,
                    n.OffdaysIncExc
                }
            ).ToListAsync();

            return result.Cast<object>().ToList();
        }

        public async Task<List<object>> fillweekendinclude(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {
            var result = await (from l in _context.LeavePolicyInstanceLimits
                                join m in _context.LeavePolicyMasters
                                    on l.LeavePolicyMasterId equals m.LeavePolicyMasterId
                                join n in _context.LeavePolicyWeekendIncludes
                                    on l.LeavePolicyInstanceLimitId equals n.LeavePolicyInstanceLimitId into weekendGroup
                                from n in weekendGroup.DefaultIfEmpty()
                                where l.LeavePolicyMasterId == LeavePolicyMasterID &&
                                      (LeavePolicyInstanceLimitID == 0 || l.LeavePolicyInstanceLimitId == LeavePolicyInstanceLimitID)
                                select new
                                {
                                    Weekendstatus = n.Weekendstatus,
                                    weekFromdays = n.Fromdays,
                                    weekTodays = n.Todays,
                                    weekLeavetype = n.Leavetype,
                                    LeaveDays = n.LeaveDays,
                                    OffdaysIncExc = n.OffdaysIncExc
                                }).ToListAsync();

            return result.Cast<object>().ToList();
        }


        public async Task<List<object>> FillHolidayincludeAsync(int leavePolicyMasterID, int leavePolicyInstanceLimitID)
        {
            var result = await (from l in _context.LeavePolicyInstanceLimits
                                join m in _context.LeavePolicyMasters
                                    on l.LeavePolicyMasterId equals m.LeavePolicyMasterId
                                join h in _context.LeavePolicyHolidayIncludes
                                    on l.LeavePolicyInstanceLimitId equals h.LeavePolicyInstanceLimitId into holidayGroup
                                from h in holidayGroup.DefaultIfEmpty()
                                where l.LeavePolicyMasterId == leavePolicyMasterID &&
                                      (leavePolicyInstanceLimitID == 0 || l.LeavePolicyInstanceLimitId == leavePolicyInstanceLimitID)
                                select new
                                {
                                    h.Holidaystatus,
                                    holFromdays = h.Fromdays,
                                    holTodays = h.Todays,
                                    holLeavetype = h.Leavetype,
                                    h.LeaveDays,
                                    h.OffdaysIncExc
                                }).ToListAsync();

            return result.Cast<object>().ToList();
        }
        public async Task<string> InsertInstanceLeaveLimitAsync(LeavePolicyInstanceLimitDto dto, string compLeaveIDs, int empId)
        {
            if (dto.LeavePolicyInstanceLimitID == 0)
            {
                // Duplicate check
                var isDuplicate = await _context.LeavePolicyInstanceLimits.AnyAsync(l =>
                    l.LeavePolicyMasterId == dto.LeavePolicyMasterID &&
                    l.LeaveId == dto.LeaveID);

                if (isDuplicate)
                    return "Already";

                // Insert logic
                var entity = new LeavePolicyInstanceLimit
                {
                    LeavePolicyMasterId = dto.LeavePolicyMasterID,
                    InstId = dto.Inst_Id,
                    LeaveId = dto.LeaveID,
                    MaximamLimit = dto.MaximamLimit,
                    MinimumLimit = dto.MinimumLimit,
                    IsHolidayIncluded = dto.IsHolidayIncluded,
                    IsWeekendIncluded = dto.IsWeekendIncluded,
                    NoOfDayIncludeHoliday = dto.NoOfDayIncludeHoliday,
                    NoOfDayIncludeWeekEnd = dto.NoOfDayIncludeWeekEnd,
                    EntryBy = dto.EntryBy,
                    EntryDate = DateTime.UtcNow,
                    Daysbtwnleaves = dto.Daysbtwnleaves,
                    Salaryadvancedays = dto.Salaryadvancedays,
                    Roledeligationdays = dto.Roledeligationdays,
                    Attachmentdays = dto.Attachmentdays,
                    ProbationMl = dto.ProbationML,
                    NewjoinMl = dto.NewjoinML,
                    OtherMl = dto.OtherML,
                    Halfday = dto.Halfday,
                    PredatedApplication = dto.PredatedApplication,
                    Daysbtwndifferentleave = dto.Daysbtwndifferentleave,
                    Daysleaveclubbing = dto.Daysleaveclubbing,
                    Predateddayslimit = dto.Predateddayslimit,
                    Returndate = dto.Returndate,
                    Autotravelapprove = dto.Autotravelapprove,
                    Leaveinclude = dto.Leaveinclude,
                    Contactdetails = dto.Contactdetails,
                    Leavereason = dto.Leavereason,
                    Approvremark = dto.Approvremark,
                    Nobalance = dto.Nobalance,
                    Applyafterallleave = dto.Applyafterallleave,
                    Applyafterleaveids = dto.Applyafterleaveids,
                    Showinapplicationonly = dto.Showinapplicationonly,
                    Rejectremark = dto.Rejectremark,
                    Predatedapplicationproxy = dto.Predatedapplicationproxy,
                    Predateddayslimitproxy = dto.Predateddayslimitproxy,
                    PredatedapplicationAttendance = dto.PredatedapplicationAttendance,
                    PredatedapplicationAttendanceDays = dto.PredatedapplicationAttendanceDays,
                    FutureleaveApplication = dto.FutureleaveApplication,
                    FutureleaveApplicationDays = dto.FutureleaveApplicationDays
                };

                _context.LeavePolicyInstanceLimits.Add(entity);
                await _context.SaveChangesAsync();

                dto.LeavePolicyInstanceLimitID = entity.LeavePolicyInstanceLimitId;
            }
            else
            {
                // Update logic
                var existing = await _context.LeavePolicyInstanceLimits
                    .FirstOrDefaultAsync(x => x.LeavePolicyInstanceLimitId == dto.LeavePolicyInstanceLimitID);

                if (existing == null) return "NotFound";

                // Update fields
                existing.LeaveId = dto.LeaveID;
                existing.MaximamLimit = dto.MaximamLimit;
                existing.MinimumLimit = dto.MinimumLimit;
                existing.EntryBy = dto.EntryBy;
                existing.EntryDate = DateTime.UtcNow;
                existing.IsHolidayIncluded = dto.IsHolidayIncluded;
                existing.IsWeekendIncluded = dto.IsWeekendIncluded;
                existing.NoOfDayIncludeHoliday = dto.NoOfDayIncludeHoliday;
                existing.NoOfDayIncludeWeekEnd = dto.NoOfDayIncludeWeekEnd;
                existing.Daysbtwnleaves = dto.Daysbtwnleaves;
                existing.Salaryadvancedays = dto.Salaryadvancedays;
                existing.Roledeligationdays = dto.Roledeligationdays;
                existing.Attachmentdays = dto.Attachmentdays;
                existing.ProbationMl = dto.ProbationML;
                existing.NewjoinMl = dto.NewjoinML;
                existing.OtherMl = dto.OtherML;
                existing.Halfday = dto.Halfday;
                existing.PredatedApplication = dto.PredatedApplication;
                existing.Daysbtwndifferentleave = dto.Daysbtwndifferentleave;
                existing.Daysleaveclubbing = dto.Daysleaveclubbing;
                existing.Predateddayslimit = dto.Predateddayslimit;
                existing.Returndate = dto.Returndate;
                existing.Autotravelapprove = dto.Autotravelapprove;
                existing.Leaveinclude = dto.Leaveinclude;
                existing.Contactdetails = dto.Contactdetails;
                existing.Leavereason = dto.Leavereason;
                existing.Approvremark = dto.Approvremark;
                existing.Nobalance = dto.Nobalance;
                existing.Applyafterallleave = dto.Applyafterallleave;
                existing.Applyafterleaveids = dto.Applyafterleaveids;
                existing.Showinapplicationonly = dto.Showinapplicationonly;
                existing.Rejectremark = dto.Rejectremark;
                existing.Predatedapplicationproxy = dto.Predatedapplicationproxy;
                existing.Predateddayslimitproxy = dto.Predateddayslimitproxy;
                existing.PredatedapplicationAttendance = dto.PredatedapplicationAttendance;
                existing.PredatedapplicationAttendanceDays = dto.PredatedapplicationAttendanceDays;
                existing.FutureleaveApplication = dto.FutureleaveApplication;
                existing.FutureleaveApplicationDays = dto.FutureleaveApplicationDays;

                await _context.SaveChangesAsync();

                // Clean up old references
                var oldIncludes = _context.LeavePolicyLeaveNotIncludes
                    .Where(x => x.LeavePolicyInstanceLimitId == dto.LeavePolicyInstanceLimitID);
                _context.LeavePolicyLeaveNotIncludes.RemoveRange(oldIncludes);

                _context.HrmLeaveProofs.RemoveRange(
                    _context.HrmLeaveProofs.Where(x => x.InstantlimitId == dto.LeavePolicyInstanceLimitID));

                _context.LeavePolicyHolidayIncludes.RemoveRange(
                    _context.LeavePolicyHolidayIncludes.Where(x => x.LeavePolicyInstanceLimitId == dto.LeavePolicyInstanceLimitID));

                _context.LeavePolicyWeekendIncludes.RemoveRange(
                    _context.LeavePolicyWeekendIncludes.Where(x => x.LeavePolicyInstanceLimitId == dto.LeavePolicyInstanceLimitID));

                _context.LeavePolicyLeaveIncludes.RemoveRange(
                    _context.LeavePolicyLeaveIncludes.Where(x => x.LeavePolicyInstanceLimitId == dto.LeavePolicyInstanceLimitID));

                await _context.SaveChangesAsync();

                // Insert audit record
                //_context.LeavePolicyHistories.Add(new LeavePolicyHistory
                //{
                //    Leavepolicymasterid = dto.LeavePolicyMasterID,
                //    EmployeeId = empId,
                //    UpdatedBy = empId,
                //    UpdatedDate = DateTime.UtcNow
                //});

                //await _context.SaveChangesAsync();
            }

            // Reinsert LeavePolicyLeaveNotInclude
            if (!string.IsNullOrWhiteSpace(compLeaveIDs))
            {
                var leaveIdList = compLeaveIDs.Split(',')
                    .Select(id => int.TryParse(id, out var val) ? val : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => new LeavePolicyLeaveNotInclude
                    {
                        LeavePolicyInstanceLimitId = dto.LeavePolicyInstanceLimitID,
                        LeavePolicyMasterId = dto.LeavePolicyMasterID,
                        LeaveId = id.Value,
                        CreatedBy = dto.EntryBy,
                        CreatedDate = DateTime.UtcNow
                    });

                _context.LeavePolicyLeaveNotIncludes.AddRange(leaveIdList);
                await _context.SaveChangesAsync();
            }

            return dto.LeavePolicyInstanceLimitID.ToString();
        }

        public async Task<string?> DeleteInstanceLimit(int LeavePolicyInstanceLimitID)
        {
            var leaveNotIncludes = _context.LeavePolicyLeaveNotIncludes
                .Where(x => x.LeavePolicyInstanceLimitId == LeavePolicyInstanceLimitID);
            _context.LeavePolicyLeaveNotIncludes.RemoveRange(leaveNotIncludes);

            var leaveIncludes = _context.LeavePolicyLeaveIncludes
                .Where(x => x.LeavePolicyInstanceLimitId == LeavePolicyInstanceLimitID);
            _context.LeavePolicyLeaveIncludes.RemoveRange(leaveIncludes);

            var holidayIncludes = _context.LeavePolicyHolidayIncludes
                .Where(x => x.LeavePolicyInstanceLimitId == LeavePolicyInstanceLimitID);
            _context.LeavePolicyHolidayIncludes.RemoveRange(holidayIncludes);

            var weekendIncludes = _context.LeavePolicyWeekendIncludes
                .Where(x => x.LeavePolicyInstanceLimitId == LeavePolicyInstanceLimitID);
            _context.LeavePolicyWeekendIncludes.RemoveRange(weekendIncludes);

            var leaveProofs = _context.HrmLeaveProofs
                .Where(x => x.InstantlimitId == LeavePolicyInstanceLimitID);
            _context.HrmLeaveProofs.RemoveRange(leaveProofs);

            var instanceLimit = await _context.LeavePolicyInstanceLimits
                .FirstOrDefaultAsync(x => x.LeavePolicyInstanceLimitId == LeavePolicyInstanceLimitID);
            if (instanceLimit != null)
            {
                _context.LeavePolicyInstanceLimits.Remove(instanceLimit);
            }
            await _context.SaveChangesAsync();
            string errorMessage = "true";

            return errorMessage;
        }
        public async Task<object> LeaveCompilation(int empId, string subMode)
        {
            var settingsId = await _externalApiService.EmployeeParameterSettings(empId, "EmployeeReporting", "Leavecalculation", "COM");
            if (settingsId == 1 && subMode == "leavebalancefulldetails" || subMode == "Fulldetails" || subMode == "upload")
            {
                return await Task.FromResult(new
                {
                    Status = "Error",
                    Message = "Leave compilation settings not found."
                });
            }
            return await Task.FromResult(new
            {
                Status = "Success",
                Message = "Leave compilation completed successfully."
            });
        }

    }

}
