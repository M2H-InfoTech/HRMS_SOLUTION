using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;
using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Models.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ATTENDANCE.Repository.AttendancePolicy
{
    public class AttendancePolicyRepository(EmployeeDBContext _context):IAttendancePolicyRepository
    {
        public async Task<List<OvertimeValueDto>> FillOverTime()
        {
            var result = await _context.HrmValueTypes
                .Where(a => a.Type == "OverTime")
                .Select(a => new OvertimeValueDto
                {
                    Value = a.Value,
                    Description = a.Description,
                    Maximum = "0",
                    Minimum = "0",
                    WeekDays = "",
                    Code = a.Code
                })
                .ToListAsync(); // <-- removed extra semicolon above

            return result;
        }

        public async Task<List<LeaveTypeDto>> GetLeaveType()
        {
            var result = await _context.PayLeaveTypes
                .Select(a => new LeaveTypeDto
                {
                    LeaveDesc = a.LeaveDesc,
                    LeaveTypeId = a.LeaveTypeId
                })
                .ToListAsync();
            return result;
        }

        public async Task<int> InsertAttendancePolicy(InsertAttendancePolicyDto request)
        {

            try
            {
                if (request.AttendancePolicyID == 0)
                {

                    var policy = new Attendancepolicy00
                    {
                        PolicyName = request.PolicyName,
                        Criteria = request.Criteria,
                        CheckDirection = request.CheckDirection,
                        LateIn = request.LateIn,
                        LateOut = request.LateOut,
                        EarlyIn = request.EarlyIn,
                        EarlyOut = request.EarlyOut,
                        RoundOf = request.RoundOf,
                        SpeacialSeasonId = request.SpeacialSeasonId,
                        StrictShiftTime = request.StrictShiftTime,
                        OverTimeInclude = request.OverTimeInclude,
                        CkhOtconsiderInShortage = request.CkhOtconsiderInShortage,
                        EnableOtonRequest = request.EnableOtonRequest,
                        StatusOnAbsentShortage = request.StatusOnAbsentShortage,
                        PresentForMinimumWorkHrs = request.PresentForMinimumWorkHrs,
                        MinimumWorkHrsForPrsnt = request.MinimumWorkHrsForPrsnt,
                        ShortageFreeMinutes = request.ShortageFreeMinutes,
                        EnableLateinPolicy = request.EnableLateinPolicy,
                        CountForLateIn = request.CountForLateIn,
                        TimeFrom = request.TimeFrom,
                        TimeTo = request.TimeTo
                    };
                    await _context.Attendancepolicy00s.AddAsync(policy);
                    await _context.SaveChangesAsync();
                    int insertedId = policy.AttendancePolicyId;

                    if (insertedId != 0)
                    {
                        var attenPolicy01 = request.attendancePolicy01Dto.Select(req => new Attendancepolicy01
                        {
                            AttendancePolicyId = insertedId,
                            MaxLateComingLimitNo = req.MaxLateComingLimitNo,
                            MaxEarlyOutLimitNo = req.MaxEarlyOutLimitNo,
                            MaxLateComingLimitMin = req.MaxLateComingLimitMin,
                            MaxEarlyOutLimitMin = req.MaxEarlyOutLimitMin,
                            EarlyGapLimitNo = req.EarlyGapLimitNo,
                            LateGapLimitNo = req.LateGapLimitNo,
                            PolicyConId = req.PolicyConId,
                            CreatedBy = req.EntryBy,
                            CreatedOn = DateTime.Now
                        }).ToList();

                        await _context.Attendancepolicy01s.AddRangeAsync(attenPolicy01);


                        var attenPolicy02 = request.OverTimeList.Select(overTime => new Attendancepolicy02
                        {
                            AttendancePolicyId = insertedId,
                            OverTimeTypeId = overTime.OverTimeTypeId,
                            Maximum = overTime.Maximum,
                            Minimum = overTime.Minimum,
                            WeekDay = overTime.WeekDay
                        }).ToList();

                        await _context.Attendancepolicy02s.AddRangeAsync(attenPolicy02);

                        if (request.SpecialOvertimes != null && request.SpecialOvertimes.Any())
                        {
                            var specialOvertimeEntities = request.SpecialOvertimes.Select(s => new Attendancepolicy02
                            {
                                AttendancePolicyId = insertedId,
                                OverTimeTypeId = s.OverTimeTypeId,
                                Maximum = s.Maximum,
                                Minimum = s.Minimum,
                                WeekDay = s.WeekDay,
                                StartTime = s.StartTime,
                                EndTime = s.EndTime,
                                PolicyDayType = s.PolicyDayType
                            }).ToList();

                            await _context.Attendancepolicy02s.AddRangeAsync(specialOvertimeEntities);

                        }
                        var shortageEntities = request.ShortageList?.Select(s => new Attendancepolicy03
                        {
                            AttendancePolicyId = insertedId,
                            ShortageId = s.ShortageId,
                            PercentageFrom = s.PercentageFrom,
                            PercentageTo = s.PercentageTo
                        }).ToList();

                        if (shortageEntities != null && shortageEntities.Any())
                        {
                            await _context.Attendancepolicy03s.AddRangeAsync(shortageEntities);

                        }
                        await _context.SaveChangesAsync();


                    }

                    return 1;
                }
                else
                {
                    var policy = await _context.Attendancepolicy00s
                        .FirstOrDefaultAsync(x => x.AttendancePolicyId == request.AttendancePolicyID);

                    if (policy != null)
                    {
                        policy.PolicyName = request.PolicyName;
                        policy.Criteria = request.Criteria;
                        policy.CheckDirection = request.CheckDirection;
                        policy.LateIn = request.LateIn;
                        policy.LateOut = request.LateOut;
                        policy.EarlyIn = request.EarlyIn;
                        policy.EarlyOut = request.EarlyOut;
                        policy.RoundOf = request.RoundOf;
                        policy.SpeacialSeasonId = request.SpeacialSeasonId;
                        policy.StrictShiftTime = request.StrictShiftTime;
                        policy.OverTimeInclude = request.OverTimeInclude;
                        policy.CkhOtconsiderInShortage = request.CkhOtconsiderInShortage;
                        policy.EnableOtonRequest = request.EnableOtonRequest;
                        policy.StatusOnAbsentShortage = request.StatusOnAbsentShortage;
                        policy.PresentForMinimumWorkHrs = request.PresentForMinimumWorkHrs;
                        policy.MinimumWorkHrsForPrsnt = request.MinimumWorkHrsForPrsnt;
                        policy.ShortageFreeMinutes = request.ShortageFreeMinutes;
                        policy.EnableLateinPolicy = request.EnableLateinPolicy;
                        policy.CountForLateIn = request.CountForLateIn;
                        policy.TimeFrom = request.TimeFrom;
                        policy.TimeTo = request.TimeTo;


                    }
                    var dto = request.attendancePolicy01Dto.FirstOrDefault();
                    if (dto != null)
                    {
                        var existing = await _context.Attendancepolicy01s
                            .FirstOrDefaultAsync(x => x.AttendancePolicyId == request.AttendancePolicyID);

                        if (existing != null)
                        {
                            existing.MaxLateComingLimitNo = dto.MaxLateComingLimitNo;
                            existing.MaxEarlyOutLimitNo = dto.MaxEarlyOutLimitNo;
                            existing.MaxLateComingLimitMin = dto.MaxLateComingLimitMin;
                            existing.MaxEarlyOutLimitMin = dto.MaxEarlyOutLimitMin;
                            existing.EarlyGapLimitNo = dto.EarlyGapLimitNo;
                            existing.LateGapLimitNo = dto.LateGapLimitNo;
                            existing.PolicyConId = dto.PolicyConId;

                        }


                    }
                    // Delete from ATTENDANCEPOLICY02
                    var policy02Records = await _context.Attendancepolicy02s
                        .Where(x => x.AttendancePolicyId == request.AttendancePolicyID)
                        .ToListAsync();

                    if (policy02Records != null)
                    {
                        _context.Attendancepolicy02s.RemoveRange(policy02Records);
                    }

                    // Delete from ATTENDANCEPOLICY03
                    var policy03Records = await _context.Attendancepolicy03s
                        .Where(x => x.AttendancePolicyId == request.AttendancePolicyID)
                        .ToListAsync();


                    if (policy02Records != null)
                    {
                        _context.Attendancepolicy03s.RemoveRange(policy03Records);
                    }

                    // Save changes
                    await _context.SaveChangesAsync();

                    var attenPolicy02 = request.OverTimeList.Select(overTime => new Attendancepolicy02
                    {
                        AttendancePolicyId = request.AttendancePolicyID,
                        OverTimeTypeId = overTime.OverTimeTypeId,
                        Maximum = overTime.Maximum,
                        Minimum = overTime.Minimum,
                        WeekDay = overTime.WeekDay
                    }).ToList();

                    await _context.Attendancepolicy02s.AddRangeAsync(attenPolicy02);

                    if (request.SpecialOvertimes != null && request.SpecialOvertimes.Any())
                    {
                        var specialOvertimeEntities = request.SpecialOvertimes.Select(s => new Attendancepolicy02
                        {
                            AttendancePolicyId = request.AttendancePolicyID,
                            OverTimeTypeId = s.OverTimeTypeId,
                            Maximum = s.Maximum,
                            Minimum = s.Minimum,
                            WeekDay = s.WeekDay,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime,
                            PolicyDayType = s.PolicyDayType
                        }).ToList();

                        await _context.Attendancepolicy02s.AddRangeAsync(specialOvertimeEntities);

                    }
                    var shortageEntities = request.ShortageList?.Select(s => new Attendancepolicy03
                    {
                        AttendancePolicyId = request.AttendancePolicyID,
                        ShortageId = s.ShortageId,
                        PercentageFrom = s.PercentageFrom,
                        PercentageTo = s.PercentageTo
                    }).ToList();

                    if (shortageEntities != null && shortageEntities.Any())
                    {
                        await _context.Attendancepolicy03s.AddRangeAsync(shortageEntities);

                    }

                    var history = new AttendancePolicyHistory
                    {
                        AttendancePolicyId = request.AttendancePolicyID, // from your method or DTO
                        EmployeeId = request.empId,
                        UpdatedBy = request.empId,
                        UpdatedDate = DateTime.UtcNow
                    };

                    await _context.AttendancePolicyHistories.AddAsync(history);

                    await _context.SaveChangesAsync();


                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
