using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;
using ATTENDANCE.DTO.Response.shift;

using Azure.Core;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.Models.Models.Entity;
using HRMS.EmployeeInformation.Repository.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ATTENDANCE.Repository.ShiftSettings
{
    public class ShiftSettingsRepository(EmployeeDBContext _context) : IShiftSettingsRepository
    {
        private static IEnumerable<string> SplitStrings_XML(string list)
        {
            if (string.IsNullOrWhiteSpace(list))
                return Enumerable.Empty<string>();

            // Split the input string by the delimiter and return as IEnumerable
            return list.Split(',')
            .Select(item => item.Trim()) // Trim whitespace from each item
            .Where(item => !string.IsNullOrEmpty(item)); // Exclude empty items
        }
        public async Task<List<HrmValueType>> GetShiftDayTypesAsync()
        {
            return await _context.HrmValueTypes
                .Where(v => v.Type == "ShiftDayType")
                .Select(v => new HrmValueType
                {
                    Id = v.Id,
                    Type = v.Type,
                    Value = v.Value,
                    Description = v.Description
                })
                .ToListAsync();
        }
        //public async Task<ShiftDetailsCollectionDto> ViewAllshiftAsync(int shiftId,int entryBy, int roleId)
        //{
        //   var shiftResponse = new ShiftDetailsCollectionDto();
        //    if (shiftId != 0)
        //    {
        //        var shifts = await _context.HrShift00s
        //            .Where(s => s.ShiftId == shiftId)
        //            .Select(s => new ShiftDto
        //            {
        //                ShiftId = s.ShiftId,
        //                ShiftCode = s.ShiftCode,
        //                ShiftName = s.ShiftName,
        //                ShiftType = s.ShiftType,
        //                EndwithNextDay = s.EndwithNextDay,
        //                ToleranceForward = s.ToleranceForward,
        //                ToleranceBackward = s.ToleranceBackward
        //            }).ToListAsync();

        //        shiftResponse.Shifts = shifts;

        //    }

        //    else
        //    {
        //        int transId =await  _context.TransactionMasters
        //            .Where(t => t.TransactionType == "Shift")
        //            .Select(t => t.TransactionId)
        //            .FirstOrDefaultAsync();

        //        int newEmpId = await _context.HrEmployeeUserRelations
        //            .Where(r => r.UserId == entryBy)
        //            .Select(r => r.EmpId)
        //            .FirstOrDefaultAsync();

        //        int? linkLevel =await  _context.SpecialAccessRights
        //            .Where(r => r.RoleId == roleId)
        //            .Select(r => r.LinkLevel)
        //            .FirstOrDefaultAsync();


        //        string empEntity = await _context.HrEmpMasters
        //            .Where(e => e.EmpId == newEmpId)
        //            .Select(e => e.EmpEntity)
        //            .FirstOrDefaultAsync();

        //        var level15Exists = await  _context.EntityAccessRights02s
        //            .Where(s => s.RoleId == roleId && s.LinkLevel == 15)
        //            .SelectMany(s => SplitStrings_XML(s.LinkId),
        //                        (s, link) => new { s.LinkLevel, LinkId = link })
        //            .AnyAsync();

        //        if (level15Exists) {
        //            var shifts =await  _context.HrShift00s
        //                .Where(s => shiftId == 0 || s.ShiftId == shiftId)
        //            .Select(s => new ShiftDto
        //            {
        //                ShiftId = s.ShiftId,
        //                ShiftCode = s.ShiftCode,
        //                ShiftName = s.ShiftName,
        //                ShiftType = s.ShiftType,
        //                EndwithNextDay = s.EndwithNextDay
        //            }).ToListAsync();

        //            shiftResponse.Shifts = shifts;

        //        }
        //        else
        //        {
        //            var newHighList = await employeeRepository.GetNewHighListAsync(newEmpId, roleId, transId, linkLevel);


        //            // Step 1: Union ApplicableFinal + ApplicableFinal02


        //            // Step 2: Flatten EntityApplicable00 joins by level
        //            var entityApplicableMapped = await (
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelOneId
        //                where ea.TransactionId == transId && ea.LinkLevel == 1
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelTwoId
        //                where ea.TransactionId == transId && ea.LinkLevel == 2
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelThreeId
        //                where ea.TransactionId == transId && ea.LinkLevel == 3
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelFourId
        //                where ea.TransactionId == transId && ea.LinkLevel == 4
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelFiveId
        //                where ea.TransactionId == transId && ea.LinkLevel == 5
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelSixId
        //                where ea.TransactionId == transId && ea.LinkLevel == 6
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelSevenId
        //                where ea.TransactionId == transId && ea.LinkLevel == 7
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelEightId
        //                where ea.TransactionId == transId && ea.LinkLevel == 8
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelNineId
        //                where ea.TransactionId == transId && ea.LinkLevel == 9
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelTenId
        //                where ea.TransactionId == transId && ea.LinkLevel == 10
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelElevenId
        //                where ea.TransactionId == transId && ea.LinkLevel == 11
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).Union(
        //                from ea in _context.EntityApplicable00s
        //                join hlv in _context.HighLevelViewTables on ea.LinkId equals hlv.LevelTwelveId
        //                where ea.TransactionId == transId && ea.LinkLevel == 12
        //                select new { hlv.LastEntityId, ea.MasterId }
        //            ).ToListAsync();

        //            var finalMasterIds =
        //                from map in entityApplicableMapped
        //                join hlv in _context.HighLevelViewTables on map.LastEntityId equals hlv.LastEntityId
        //                where newHighList.Contains(map.MasterId)
        //                select map.MasterId;

        //            // Step 3: Final Join



        //            var Shifts = await _context.HrShift00s
        //                .Where(h => (shiftId == 0 || h.ShiftId == shiftId) && finalMasterIds.Contains(h.ShiftId))
        //                .Select(s => new ShiftDto
        //                {
        //                    ShiftId = s.ShiftId,
        //                    ShiftCode = s.ShiftCode,
        //                    ShiftName = s.ShiftName,
        //                    ShiftType = s.ShiftType,
        //                    EndwithNextDay = s.EndwithNextDay
        //                }).ToListAsync();
        //            shiftResponse.Shifts = Shifts;


        //        }



        //    }

        //    var shiftIdParam = shiftId; // your input ShiftID

        //    var shiftdata1 = await (
        //        from s in _context.HrShift01s
        //        where shiftIdParam == 0 || s.ShiftId == shiftIdParam
        //        select new ShiftDetailDto
        //        {
        //            Shift01Id = s.Shift01Id,
        //            ShiftId = s.ShiftId,
        //            ShiftStartTypeID = s.ShiftStartType,
        //            ShiftStartType = _context.HrmValueTypes
        //                   .Where(v => v.Type == "ShiftDayType" && v.Value == s.ShiftStartType)
        //                   .Select(v => v.Description)
        //                   .FirstOrDefault(),
        //            FirstHalf = s.FirstHalf,
        //            SecondHalf = s.SecondHalf,
        //            ShiftEndTypeID = s.ShiftEndType,
        //            StartTime = s.StartTime.ToString().Replace(".", ":"),
        //            ShiftEndType = _context.HrmValueTypes
        //                   .Where(v => v.Type == "ShiftDayType" && v.Value == s.ShiftEndType)
        //                   .Select(v => v.Description)
        //                   .FirstOrDefault(),
        //            EndTime = s.EndTime.ToString().Replace(".", ":"),
        //            TotalHours = s.TotalHours,
        //            EffectiveFrom = s.EffectiveFrom.HasValue ? s.EffectiveFrom.Value.ToString("MM/dd/yyyy") : null,
        //            MinimumWorkHours = s.MinimumWorkHours
        //        }

        //    ).ToListAsync();  
        //    shiftResponse.ShiftDetails = shiftdata1;

        //    // Check if ShiftType is "Split"
        //    var isSplitShift = await _context.HrShift00s
        //        .Where(s => s.ShiftId == shiftId)
        //        .Select(s => s.ShiftType)
        //        .FirstOrDefaultAsync() == "Split";

        //    if (isSplitShift)
        //    {
        //        var shiftTable = await (
        //            from s2 in _context.HrShift02s
        //            join s1 in _context.HrShift01s on s2.Shift01Id equals s1.Shift01Id
        //            where shiftId == 0 || s2.ShiftId == shiftId
        //            select new ShiftBreakDto
        //            {
        //                StartTime = Math.Round((decimal)s1.StartTime, 2),

        //                EndTime = Math.Round((decimal)s1.EndTime, 2),
        //                Shift02Id = s2.Shift02Id,
        //                ShiftId = s2.ShiftId,
        //                BreakStartTypeID = s2.BreakStartType,
        //                BreakStartType = _context.HrmValueTypes
        //                    .Where(v => v.Type == "ShiftDayType" && v.Value == s2.BreakStartType)
        //                    .Select(v => v.Description)
        //                    .FirstOrDefault(),
        //                BreakStartTime = ((int)s2.BreakStartTime / 100).ToString("D2") + ":" + ((int)s2.BreakStartTime % 100).ToString("D2"),
        //                BreakEndTypeID = s2.BreakEndType,
        //                BreakEndTime = ((int)s2.BreakEndTime / 100).ToString("D2") + ":" + ((int)s2.BreakEndTime % 100).ToString("D2"),
        //                BreakEndType = _context.HrmValueTypes
        //                    .Where(v => v.Type == "ShiftDayType" && v.Value == s2.BreakEndType)
        //                    .Select(v => v.Description)
        //                    .FirstOrDefault(),
        //                TotalBreakHours = s2.TotalBreakHours,
        //                EffectiveFrom = s2.EffectiveFrom.HasValue
        //                    ? s2.EffectiveFrom.Value.ToString("MM/dd/yyyy")
        //                    : null,
        //                IsPaid = s2.IsPaid
        //            }
        //        ).ToListAsync();
        //        shiftResponse.ShiftBreaks = shiftTable;
        //    }
        //    else
        //    {
        //        var shiftTable = await (
        //            from s in _context.HrShift02s
        //            where shiftId == 0 || s.ShiftId == shiftId
        //            select new ShiftBreakDto
        //            {
        //                Shift02Id = s.Shift02Id,
        //                ShiftId = s.ShiftId,
        //                BreakStartTypeID = s.BreakStartType,
        //                BreakStartType = _context.HrmValueTypes
        //                    .Where(v => v.Type == "ShiftDayType" && v.Value == s.BreakStartType)
        //                    .Select(v => v.Description)
        //                    .FirstOrDefault(),
        //                BreakStartTime = ((int)s.BreakStartTime / 100).ToString("D2") + ":" + ((int)s.BreakStartTime % 100).ToString("D2"),
        //                BreakEndTypeID = s.BreakEndType,
        //                BreakEndTime = ((int)s.BreakEndTime / 100).ToString("D2") + ":" + ((int)s.BreakEndTime % 100).ToString("D2"),
        //                BreakEndType = _context.HrmValueTypes
        //                    .Where(v => v.Type == "ShiftDayType" && v.Value == s.BreakEndType)
        //                    .Select(v => v.Description)
        //                    .FirstOrDefault(),
        //                TotalBreakHours = s.TotalBreakHours,
        //                EffectiveFrom = s.EffectiveFrom.HasValue
        //                    ? s.EffectiveFrom.Value.ToString("MM/dd/yyyy")
        //                    : null,
        //                IsPaid = s.IsPaid
        //            }).ToListAsync();

        //        shiftResponse.ShiftBreaks = shiftTable;


        //    }
        //    var result = await (
        //        from a in _context.HrShiftOpens
        //        join b in _context.HrShift00s on a.ShiftMasterId equals b.ShiftId
        //        join c in _context.HrShift00s on a.ShiftId equals c.ShiftId
        //        where a.ShiftMasterId == shiftId
        //        select new ShiftMasterDto
        //        {
        //            ShiftMasterId = a.ShiftMasterId,
        //            ShiftId = a.ShiftId,
        //            ShiftName = c.ShiftName
        //        }).ToListAsync();
        //    shiftResponse.ShiftMasters = result;


        //    var shiftSeasons = await _context.HrShiftseason00s
        //        .Where(s => s.ShiftId == shiftId || shiftId == 0)
        //        .Select(s => new ShiftSeasonDto
        //        {
        //            ShiftSeason01Id = s.ShiftSeason01Id,
        //            ShiftId = s.ShiftId,
        //            ShiftStartTypeID = s.ShiftStartType,
        //            ShiftStartType = _context.HrmValueTypes
        //                .Where(v => v.Type == "ShiftDayType" && v.Value == s.ShiftStartType)
        //                .Select(v => v.Description)
        //                .FirstOrDefault(),
        //            ShiftEndTypeID = s.ShiftEndType,
        //            StartTime = s.StartTime.ToString().Replace(".", ":"),
        //            ShiftEndType = _context.HrmValueTypes
        //                .Where(v => v.Type == "ShiftDayType" && v.Value == s.ShiftEndType)
        //                .Select(v => v.Description)
        //                .FirstOrDefault(),
        //            EndTime = s.EndTime.ToString().Replace(".", ":"),
        //            TotalHours = s.TotalHours,
        //            EffectiveFrom = s.EffectiveFrom.HasValue
        //                ? s.EffectiveFrom.Value.ToString("MM/dd/yyyy")
        //                : null,
        //            MinimumWorkHours = s.MinimumWorkHours
        //        })
        //        .ToListAsync();
        //    shiftResponse.ShiftSeasons = shiftSeasons;


        //    var shiftSeasonBreaks = await _context.HrShiftseason01s
        //        .Where(s => s.ShiftId == shiftId || shiftId == 0)
        //        .Select(s => new ShiftSeasonBreakDto
        //        {
        //            ShiftSeason02Id = s.Shiftseason02Id,
        //            ShiftId = s.ShiftId,
        //            BreakStartTypeID = s.BreakStartType,
        //            BreakStartType = _context.HrmValueTypes
        //                .Where(v => v.Type == "ShiftDayType" && v.Value == s.BreakStartType)
        //                .Select(v => v.Description)
        //                .FirstOrDefault(),
        //            BreakStartTime = s.BreakStartTime.ToString().Replace(".", ":"),
        //            BreakEndTypeID = s.BreakEndType,
        //            BreakEndTime = s.BreakEndTime.ToString().Replace(".", ":"),
        //            BreakEndType = _context.HrmValueTypes
        //                .Where(v => v.Type == "ShiftDayType" && v.Value == s.BreakEndType)
        //                .Select(v => v.Description)
        //                .FirstOrDefault(),
        //            TotalBreakHours = s.TotalBreakHours,
        //            EffectiveFrom = s.EffectiveFrom.HasValue
        //                ? s.EffectiveFrom.Value.ToString("MM/dd/yyyy")
        //                : null,
        //            IsPaid = s.IsPaid
        //        })
        //        .ToListAsync();
        //    shiftResponse.ShiftSeasonBreaks = shiftSeasonBreaks;


        //    return shiftResponse;
        //}

        // Helper function to convert decimal time (hh.mm) to minutes
        public int HourToMinutes(decimal time)
        {
            // Convert decimal time to string in 'hh:mm' format
            string timeString = time.ToString().Replace('.', ':').Trim();

            // Split the string into hours and minutes
            var timeParts = timeString.Split(':');
            if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int hours) && int.TryParse(timeParts[1], out int minutes))
            {
                // Return the total minutes
                return (hours * 60) + minutes;
            }

            // If the input is invalid, return 0 minutes
            return 0;
        }
        public async Task<string> CreateSplitShiftAsync(ShiftInsertRequestDto Request)
        {
            try
            {

                if (Request.ShiftID == 0)
                {
                    var result = await AddShiftHelper(Request);
                    if (result == null)
                        return "Shift with the same code already exists.";

                    var shiftId = result.ShiftId;
                    var shift01List = result.Shift01List;


                    var shift02List = Request.BreakTimeList.Select(item => new HrShift02
                    {
                        ShiftId = shiftId,
                        BreakStartType = item.BreakStartType,
                        BreakStartTime = item.BreakStartTime,
                        BreakEndType = item.BreakEndType,
                        BreakEndTime = item.BreakEndTime,
                        TotalBreakHours = item.TotalBreakHours,
                        EffectiveFrom = item.EffectiveFrom,
                        IsPaid = item.IsPaid,
                        TotalBreakMinutes = HourToMinutes(item.TotalBreakHours),
                        Shift01Id = shift01List
                            .FirstOrDefault(x => x.StartTime == item.ShiftStartTime && x.EndTime == item.ShiftEndTime)
                            ?.Shift01Id ?? 0
                    }).ToList();

                    await _context.HrShift02s.AddRangeAsync(shift02List);
                    await _context.SaveChangesAsync();

                    return "Shift created successfully.";
                }
                else
                {
                    var shift = await _context.HrShift00s.FirstOrDefaultAsync(s => s.ShiftId == Request.ShiftID);
                    if (shift == null)
                        return "Shift not found for update.";

                    shift.ShiftCode = Request.ShiftCode;
                    shift.ShiftName = Request.ShiftName;
                    shift.ShiftType = Request.ShiftType;
                    shift.EndwithNextDay = Request.EndwithNextDay;
                    shift.ToleranceForward = Request.ToleranceForward;
                    shift.ToleranceBackward = Request.ToleranceBackward;
                    shift.EntryBy = Request.EntryBy;
                    shift.EntryDate = DateTime.UtcNow;

                    _context.HrShift01s.RemoveRange(await _context.HrShift01s
                        .Where(s => s.ShiftId == Request.ShiftID)
                        .ToListAsync());

                    _context.HrShift02s.RemoveRange(await _context.HrShift02s
                        .Where(s => s.ShiftId == Request.ShiftID)
                        .ToListAsync());

                    await _context.SaveChangesAsync();

                    var shift01List = Request.TypeShiftTimeList.Select(item => new HrShift01
                    {
                        ShiftId = Request.ShiftID,
                        ShiftStartType = item.ShiftStartType,
                        StartTime = item.ShiftStartTime,
                        ShiftEndType = item.ShiftEndType,
                        EndTime = item.ShiftEndTime,
                        TotalHours = item.TotalWorkHours,
                        EffectiveFrom = item.EffectiveFrom,
                        MinimumWorkHours = item.MinimumWorkHours,
                        FirstHalf = item.FirstHalf,
                        SecondHalf = item.SecondHalf,
                        StartTimeMinutes = HourToMinutes(item.ShiftStartTime),
                        EndTimeMinutes = HourToMinutes(item.ShiftEndTime),
                        TotalMinutes = HourToMinutes(item.TotalWorkHours),
                        MinimumWorkMinutes = HourToMinutes(item.MinimumWorkHours),
                        FirstHalfMinutes = item.FirstHalf,
                        SecondHalfMinutes = item.SecondHalf
                    }).ToList();

                    await _context.HrShift01s.AddRangeAsync(shift01List);

                    var shift02List = Request.BreakTimeList.Select(item => new HrShift02
                    {
                        ShiftId = shift.ShiftId,
                        BreakStartType = item.BreakStartType,
                        BreakStartTime = item.BreakStartTime,
                        BreakEndType = item.BreakEndType,
                        BreakEndTime = item.BreakEndTime,
                        TotalBreakHours = item.TotalBreakHours,
                        EffectiveFrom = item.EffectiveFrom,
                        IsPaid = item.IsPaid,
                        TotalBreakMinutes = HourToMinutes(item.TotalBreakHours),
                        Shift01Id = shift01List
                            .FirstOrDefault(x => x.StartTime == item.ShiftStartTime && x.EndTime == item.ShiftEndTime)
                            ?.Shift01Id ?? 0
                    }).ToList();

                    await _context.HrShift02s.AddRangeAsync(shift02List);
                    await _context.SaveChangesAsync();

                    return "Shift updated successfully.";
                }
            }
            catch (Exception ex)
            {
                return $"Error occurred: {ex.Message}";
            }
        }

        public async Task<string> CreateNormalShiftAsync(ShiftInsertRequestDto Request)
        {
            try
            {
                if (Request.ShiftID == 0)
                {
                    var result = await AddShiftHelper(Request);
                    if (result == null)
                        return "Shift with the same code already exists.";

                    var shiftId = result.ShiftId;
                    var shift01List = result.Shift01List;

                    var shift02List = Request.BreakTimeList.Select(item => new HrShift02
                    {
                        ShiftId = shiftId,
                        BreakStartType = item.BreakStartType,
                        BreakStartTime = item.BreakStartTime,
                        BreakEndType = item.BreakEndType,
                        BreakEndTime = item.BreakEndTime,
                        TotalBreakHours = item.TotalBreakHours,
                        EffectiveFrom = item.EffectiveFrom,
                        IsPaid = item.IsPaid,
                        TotalBreakMinutes = HourToMinutes(item.TotalBreakHours)
                    }).ToList();

                    await _context.HrShift02s.AddRangeAsync(shift02List);
                    await _context.SaveChangesAsync();

                    return "Shift created successfully.";

                }
                else
                {
                    var shift = await _context.HrShift00s.FirstOrDefaultAsync(s => s.ShiftId == Request.ShiftID);
                    if (shift == null)
                        return "Shift not found for update.";

                    shift.ShiftCode = Request.ShiftCode;
                    shift.ShiftName = Request.ShiftName;
                    shift.ShiftType = Request.ShiftType;
                    shift.EndwithNextDay = Request.EndwithNextDay;
                    shift.ToleranceForward = Request.ToleranceForward;
                    shift.ToleranceBackward = Request.ToleranceBackward;
                    shift.EntryBy = Request.EntryBy;
                    shift.EntryDate = DateTime.UtcNow;

                    _context.HrShift01s.RemoveRange(await _context.HrShift01s
                        .Where(s => s.ShiftId == Request.ShiftID)
                        .ToListAsync());

                    _context.HrShift02s.RemoveRange(await _context.HrShift02s
                        .Where(s => s.ShiftId == Request.ShiftID)
                        .ToListAsync());

                    await _context.SaveChangesAsync();

                    var shift01List = Request.TypeShiftTimeList.Select(item => new HrShift01
                    {
                        ShiftId = Request.ShiftID,
                        ShiftStartType = item.ShiftStartType,
                        StartTime = item.ShiftStartTime,
                        ShiftEndType = item.ShiftEndType,
                        EndTime = item.ShiftEndTime,
                        TotalHours = item.TotalWorkHours,
                        EffectiveFrom = item.EffectiveFrom,
                        MinimumWorkHours = item.MinimumWorkHours,
                        FirstHalf = item.FirstHalf,
                        SecondHalf = item.SecondHalf,
                        StartTimeMinutes = HourToMinutes(item.ShiftStartTime),
                        EndTimeMinutes = HourToMinutes(item.ShiftEndTime),
                        TotalMinutes = HourToMinutes(item.TotalWorkHours),
                        MinimumWorkMinutes = HourToMinutes(item.MinimumWorkHours),
                        FirstHalfMinutes = item.FirstHalf,
                        SecondHalfMinutes = item.SecondHalf
                    }).ToList();

                    await _context.HrShift01s.AddRangeAsync(shift01List);

                    var shift02List = Request.BreakTimeList.Select(item => new HrShift02
                    {
                        ShiftId = shift.ShiftId, // corresponds to @Error_Message
                        BreakStartType = item.BreakStartType,
                        BreakStartTime = item.BreakStartTime,
                        BreakEndType = item.BreakEndType,
                        BreakEndTime = item.BreakEndTime,
                        TotalBreakHours = item.TotalBreakHours,
                        EffectiveFrom = item.EffectiveFrom,
                        IsPaid = item.IsPaid,
                        TotalBreakMinutes = HourToMinutes(item.TotalBreakHours)
                    }).ToList();

                    await _context.HrShift02s.AddRangeAsync(shift02List);
                    await _context.SaveChangesAsync();
                    return "Shift updated successfully.";
                }
            }
            catch (Exception ex)
            {
                return $"Error occurred: {ex.Message}";
            }
        }
        //helper function to add shift
        public async Task<ShiftInsertResultDto?> AddShiftHelper(ShiftInsertRequestDto request)
        {
            var existingShift = await _context.HrShift00s
                .Where(s => s.ShiftCode == request.ShiftCode)
                .Select(s => s.ShiftId)
                .FirstOrDefaultAsync();

            if (existingShift != 0)
                return null;

            var newShift = new HrShift00
            {
                CompanyId = request.CompanyID,
                ShiftCode = request.ShiftCode,
                ShiftName = request.ShiftName,
                ShiftType = request.ShiftType,
                EndwithNextDay = request.EndwithNextDay,
                ToleranceForward = request.ToleranceForward,
                ToleranceBackward = request.ToleranceBackward,
                EntryBy = request.EntryBy,
                EntryDate = DateTime.UtcNow
            };

            await _context.HrShift00s.AddAsync(newShift);
            await _context.SaveChangesAsync();

            var shiftId = newShift.ShiftId;

            var shift01List = request.TypeShiftTimeList.Select(item => new HrShift01
            {
                ShiftId = shiftId,
                ShiftStartType = item.ShiftStartType,
                StartTime = item.ShiftStartTime,
                ShiftEndType = item.ShiftEndType,
                EndTime = item.ShiftEndTime,
                TotalHours = item.TotalWorkHours,
                EffectiveFrom = item.EffectiveFrom,
                MinimumWorkHours = item.MinimumWorkHours,
                FirstHalf = item.FirstHalf,
                SecondHalf = item.SecondHalf,
                StartTimeMinutes = HourToMinutes(item.ShiftStartTime),
                EndTimeMinutes = HourToMinutes(item.ShiftEndTime),
                TotalMinutes = HourToMinutes(item.TotalWorkHours),
                MinimumWorkMinutes = HourToMinutes(item.MinimumWorkHours),
                FirstHalfMinutes = item.FirstHalf,
                SecondHalfMinutes = item.SecondHalf
            }).ToList();

            await _context.HrShift01s.AddRangeAsync(shift01List);

            return new ShiftInsertResultDto
            {
                ShiftId = shiftId,
                Shift01List = shift01List
            };
        }

        public async Task<string> CreateOpenShiftAsync(ShiftInsertRequestDto Request)
        {
            var newOpenshift = new HrShift00
            {
                CompanyId = Request.CompanyID,
                ShiftCode = Request.ShiftCode,
                ShiftName = Request.ShiftName,
                ShiftType = Request.ShiftType,
                EndwithNextDay = Request.EndwithNextDay,
                ToleranceForward = Request.ToleranceForward,
                ToleranceBackward = Request.ToleranceBackward,
                EntryBy = Request.EntryBy,
                EntryDate = DateTime.UtcNow
            };
            await _context.HrShift00s.AddAsync(newOpenshift);
            await _context.SaveChangesAsync();

            var shiftId = newOpenshift.ShiftId;

            var shift01List = Request.ShiftTimeList.Select(item => new HrShiftOpen
            {
                ShiftMasterId = shiftId,
                EntryBy = Request.EntryBy,
                EntryDate = DateTime.UtcNow,
                ShiftId = item.ShiftId,
            }).ToList();

            await _context.HrShiftOpens.AddRangeAsync(shift01List);
            await _context.SaveChangesAsync();

            return "Open Shift created successfully.";

        }

        public async Task<(int ErrorID, string ErrorMessage)> UpdateOpenShiftAsync(ShiftInsertRequestDto Request)
        {
            var shift = await _context.HrShift00s.FirstOrDefaultAsync(s => s.ShiftId == Request.ShiftID);

            if (shift != null)
            {
                shift.CompanyId = Request.CompanyID;
                shift.ShiftCode = Request.ShiftCode;
                shift.ShiftName = Request.ShiftName;
                shift.ShiftType = Request.ShiftType;
                shift.EndwithNextDay = Request.EndwithNextDay;
                shift.EntryBy = Request.EntryBy;
                shift.EntryDate = DateTime.UtcNow;
                shift.ToleranceForward = Request.ToleranceForward;
                shift.ToleranceBackward = Request.ToleranceBackward;

                var openSHiftRemove = await _context.HrShiftOpens.FirstOrDefaultAsync(o => o.ShiftMasterId == Request.ShiftID);

                _context.HrShiftOpens.RemoveRange(openSHiftRemove);


                var newOpenShift = Request.ShiftTimeList.Select(item => new HrShiftOpen
                {
                    ShiftMasterId = Request.ShiftID,
                    ShiftId = item.ShiftId,
                    EntryBy = Request.EntryBy,
                    EntryDate = DateTime.UtcNow

                });

                await _context.HrShiftOpens.AddRangeAsync(newOpenShift);

                await _context.SaveChangesAsync();
                return (0, Request.ShiftID.ToString());

            }
            else
            {
                // Handle not found case
                return (1, "Shift not found");
            }
        }


        public async Task<int> InsertShiftNormalSeasonAsync(ShiftInsertRequestDto Request)
        {
            if (Request.ShiftID != 0)
            {
                var existingSeasonData = _context.HrShiftseason00s.Where(x => x.ShiftId == Request.ShiftID);
                _context.HrShiftseason00s.RemoveRange(existingSeasonData);

                var existingBreakData = _context.HrShiftseason01s.Where(x => x.ShiftId == Request.ShiftID);
                _context.HrShiftseason01s.RemoveRange(existingBreakData);

                if (Request.BreakTimeList != null)
                {
                    var newSeasonData = Request.TypeShiftTimeList.Select(s => new HrShiftseason00
                    {
                        ShiftId = Request.ShiftID,
                        ShiftStartType = s.ShiftStartType,
                        StartTime = s.ShiftStartTime,
                        ShiftEndType = s.ShiftEndType,
                        EndTime = s.ShiftEndTime,
                        TotalHours = s.TotalWorkHours,
                        EffectiveFrom = s.EffectiveFrom,
                        MinimumWorkHours = s.MinimumWorkHours
                    });

                    await _context.HrShiftseason00s.AddRangeAsync(newSeasonData);
                }
                if (Request.BreakTimeList != null)
                {
                    var newBreakData = Request.BreakTimeList.Select(b => new HrShiftseason01
                    {
                        ShiftId = Request.ShiftID,
                        BreakStartType = b.BreakStartType,
                        BreakStartTime = b.BreakStartTime,
                        BreakEndType = b.BreakEndType,
                        BreakEndTime = b.BreakEndTime,
                        TotalBreakHours = b.TotalBreakHours,
                        EffectiveFrom = b.EffectiveFrom,
                        IsPaid = b.IsPaid
                    });

                    await _context.HrShiftseason01s.AddRangeAsync(newBreakData);
                }


                await _context.SaveChangesAsync();

                return 1;

            }


            return 0;
        }

        //public async Task<List<FillAllShiftDto>> FillAllShift(ShiftViewDto Request)
        //{
        //    var transId = await _context.TransactionMasters.Where(t=>t.TransactionType == "Shift")
        //        .Select(t => t.TransactionId)
        //        .FirstOrDefaultAsync();
        //    var newEmpId = await _context.HrEmployeeUserRelations
        //        .Where(r => r.UserId == Request.EntryBy)
        //        .Select(r => r.EmpId)
        //        .FirstOrDefaultAsync();
        //    var LinkLevel = await _context.SpecialAccessRights
        //        .Where(r=>r.RoleId == Request.RoleId)
        //        .Select(r => r.LinkLevel)
        //        .FirstOrDefaultAsync();

        //    var ifLevel15Exist = await _context.EntityAccessRights02s
        //        .Where(s=>s.RoleId == Request.RoleId && s.LinkLevel == 15)
        //        .SelectMany(s=>SplitStrings_XML(s.LinkId))
        //        .AnyAsync();

        //    if (ifLevel15Exist)
        //    {
        //       var shiftData = await _context.HrShift00s
        //            .Where(s=>s.ShiftId == Request.ShiftId || Request.ShiftId == 0)
        //            .Select(s=> new FillAllShiftDto
        //            {
        //                ShiftId = s.ShiftId,
        //                ShiftCode = s.ShiftCode,
        //                ShiftName = s.ShiftName,
        //                ShiftType = s.ShiftType,
        //                EndwithNextDay = s.EndwithNextDay
        //            }).ToListAsync();

        //        return shiftData;
        //    }


        //}
    }
}

