using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendace.Data;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace OFFICEKIT_CORE_ATTENDANCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftSettingsController(AttendanceDbContext _context) : ControllerBase
    {
        private static IEnumerable<string> SplitStrings_XML(string list, char delimiter)
        {
            if (string.IsNullOrWhiteSpace(list))
                return Enumerable.Empty<string>();


            return list.Split(delimiter)
                       .Select(item => item.Trim()) // Trim whitespace from each item
                       .Where(item => !string.IsNullOrEmpty(item)); // Exclude empty items
        }
        private string ConvertDecimalToTime(decimal? decimalHours)
        {
            if (decimalHours == null)
                return ""; // or "00:00:00" or "N/A" depending on your preference

            TimeSpan time = TimeSpan.FromHours((double)decimalHours.Value);
            return time.ToString(@"hh\:mm\:ss");
        }

        private (List<dynamic> FirstShifts, List<dynamic> SecondShifts) GetShiftPeriods()
        {
            var shiftPeriods = _context.HrShift00s
                .Join(_context.HrShift01s,
                      shift => shift.ShiftId,
                      period => period.ShiftId,
                      (shift, period) => new
                      {
                          shift.ShiftCode,
                          shift.ShiftId,
                          shift.ShiftName,
                          shift.ShiftType,
                          period.StartTime,
                          period.EndTime,
                          period.Shift01Id
                      })
                .AsEnumerable()
                .GroupBy(x => x.ShiftId)
                .SelectMany(g => g
                    .OrderBy(x => x.Shift01Id)
                    .Select((x, index) => new
                    {
                        x.ShiftCode,
                        x.ShiftId,
                        x.ShiftName,
                        x.ShiftType,
                        x.StartTime,
                        x.EndTime,
                        PeriodNum = index + 1
                    }))
                .ToList();

            var firstShifts = shiftPeriods
                .Where(x => x.PeriodNum == 1)
                .Cast<dynamic>()
                .ToList();

            var secondShifts = shiftPeriods
                .Where(x => x.PeriodNum == 2 && x.ShiftType == "Split")
                .Cast<dynamic>()
                .ToList();

            return (firstShifts, secondShifts);
        }
        private List<long> GetValidShiftAccessIds(DateTime currentDate)
        {
            return _context.ShiftMasterAccesses
                .Where(s => currentDate >= s.ValidDatefrom &&
                           (s.ValidDateTo == null || currentDate <= s.ValidDateTo))
                .Select(s => s.ShiftAccessId)
                .ToList();
        }

        private List<ShiftAccessDto> GetShiftAccessData(int shiftAccessId, int status, int empStatus, int newEmpId, List<long> shiftAccessTable)
        {

            var (firstShifts, secondShifts) = GetShiftPeriods();

            var shiftAccessData = (
                from a in _context.ShiftMasterAccesses
                join ed in _context.EmployeeDetails on a.EmployeeId equals ed.EmpId
                join w in _context.WeekEndMasters on a.WeekEndMasterId equals w.WeekEndMasterId into wGroup
                from w in wGroup.DefaultIfEmpty()
                join p in _context.ProjectMasters on a.ProjectId equals p.Id into pGroup
                from p in pGroup.DefaultIfEmpty()
                join br in _context.BranchDetails on ed.BranchId equals br.LinkId
                join des in _context.DesignationDetails on ed.DesigId equals des.LinkId
                where
                    (shiftAccessId == 0 || a.ShiftAccessId == shiftAccessId) &&
                    ((status == 1 && shiftAccessTable.Contains(a.ShiftAccessId)) ||
                     (status == 2 && !shiftAccessTable.Contains(a.ShiftAccessId))) &&
                    (
                        (empStatus == 1 && ed.SeperationStatus == 0) ||
                        (empStatus == 2 && ed.SeperationStatus > 0) ||
                        ((empStatus == 1 || empStatus == 2) &&
                            _context.HrEmpReportings.Any(er =>
                                er.EmpId == ed.EmpId &&
                                er.ReprotToWhome == newEmpId &&
                                _context.HrEmpMasters.Any(em =>
                                    em.EmpId == er.EmpId &&
                                    em.SeperationStatus == (empStatus == 1 ? 0 : 1))))
                    )
                select new { a, ed, w, p, br, des })
                .AsEnumerable()
                .Join(firstShifts, x => x.a.ShiftId, fs => fs.ShiftId, (x, fs) => new { x, fs })
                .GroupJoin(secondShifts, xf => xf.fs.ShiftId, ss => ss.ShiftId, (xf, ssGroup) => new { xf, ssGroup })
                .SelectMany(
    temp => temp.ssGroup.DefaultIfEmpty(),
    (temp, ss) => new ShiftAccessDto
    {
        ShiftAccessID = temp.xf.x.a.ShiftAccessId,
        Emp_Id = temp.xf.x.ed.EmpId,
        EmployeeName = temp.xf.x.ed.Name + "|| " + temp.xf.x.ed.EmpCode,
        ShiftID = temp.xf.x.a.ShiftId,
        ShiftName = temp.xf.fs.ShiftName,
        ShiftCodeName = temp.xf.fs.ShiftCode + " (" +
                        ConvertDecimalToTime(temp.xf.fs.StartTime) + " to " +
                        ConvertDecimalToTime(temp.xf.fs.EndTime) +
                        (ss != null ? " - " + ConvertDecimalToTime(ss.StartTime) + " to " + ConvertDecimalToTime(ss.EndTime) : "") + ")",
        WeekName = temp.xf.x.w?.Name,
        EmployeeID = temp.xf.x.a.EmployeeId,
        WeekEndMasterID = temp.xf.x.w?.WeekEndMasterId,
        StartTime = temp.xf.fs.StartTime.ToString(),
        EndTime = ConvertDecimalToTime(ss?.EndTime ?? temp.xf.fs.EndTime),
        ShiftStartEndTime = "(" +
                            ConvertDecimalToTime(temp.xf.fs.StartTime) + " to " +
                            ConvertDecimalToTime(temp.xf.fs.EndTime) +
                            (ss != null ? " - " + ConvertDecimalToTime(ss.StartTime) + " to " + ConvertDecimalToTime(ss.EndTime) : "") + ")",
        ValidDatefrom = temp.xf.x.a.ValidDatefrom?.ToString("dd/MM/yyyy") ?? "",
        ValidDateTo = temp.xf.x.a.ValidDateTo?.ToString("dd/MM/yyyy") ?? "",
        Branch = temp.xf.x.br.Branch,
        Designation = temp.xf.x.des.Designation,
        ProjectName = temp.xf.x.p?.ProjectName
    })

                .ToList();

            return shiftAccessData;
        }




        [HttpPost]
        public IActionResult GetShiftAccessDetails(int shiftAccessId = 0, int entryBy = 19, int roleId = 3, int status = 1, int empStatus = 1, int pageNumber = 1, int pageSize = 50)
        {
            DateTime shiftCurrentDate = DateTime.UtcNow.Date;

            var result = (
                from tm in _context.TransactionMasters.Where(x => x.TransactionType == "Shift")
                from eur in _context.HrEmployeeUserRelations.Where(x => x.UserId == entryBy).DefaultIfEmpty()
                from sar in _context.SpecialAccessRights.Where(x => x.RoleId == roleId).DefaultIfEmpty()
                select new
                {
                    TransId = tm.TransactionId,
                    NewEmpId = eur != null ? eur.EmpId : 0,
                    LinkLevel = sar != null ? sar.LinkLevel : 0
                }).FirstOrDefault();

            int transId = result?.TransId ?? 0;
            int newEmpId = result?.NewEmpId ?? 0;
            int lnkLev = result?.LinkLevel ?? 0;


            if (lnkLev == 15)
            {

                var directAccessEntities = _context.EntityAccessRights02s
                    .Where(s => s.RoleId == roleId && s.LinkLevel == 15)
                    .AsEnumerable() // Switch to client-side evaluation
                    .SelectMany(s => SplitStrings_XML(s.LinkId ?? string.Empty, ','))
                    .Distinct()
                    .ToList();


                var empEntityList = _context.HrEmpMasters
        .Where(e => e.EmpId == newEmpId)
        .AsEnumerable() // Switch to client-side evaluation
        .SelectMany(e => SplitStrings_XML(e.EmpEntity ?? string.Empty, ','))
        .Select((item, index) => new { item, LinkLevel = index + 2 })
        .ToList();


                var applicableEntities = directAccessEntities
                        .Select((item, index) => new { item, LinkLevel = index + 1 })
                        .Union(
                            empEntityList
                                .Where(e => lnkLev > 0 && e.LinkLevel >= lnkLev && !string.IsNullOrWhiteSpace(e.item))
                        )
                        .ToList();

                var (firstShifts, secondShifts) = GetShiftPeriods();
                var shiftAccessTable = GetValidShiftAccessIds(shiftCurrentDate);

                //var ResultData = GetShiftAccessData(shiftAccessId, status, empStatus, newEmpId, shiftAccessTable);



                var shiftAccessData = (
                    from a in _context.ShiftMasterAccesses
                    join ed in _context.EmployeeDetails on a.EmployeeId equals ed.EmpId
                    join w in _context.WeekEndMasters on a.WeekEndMasterId equals w.WeekEndMasterId into wGroup
                    from w in wGroup.DefaultIfEmpty()
                    join p in _context.ProjectMasters on a.ProjectId equals p.Id into pGroup
                    from p in pGroup.DefaultIfEmpty()
                    join br in _context.BranchDetails on ed.BranchId equals br.LinkId
                    join des in _context.DesignationDetails on ed.DesigId equals des.LinkId
                    where
                        (shiftAccessId == 0 || a.ShiftAccessId == shiftAccessId) &&
                        ((status == 1 && shiftAccessTable.Contains(a.ShiftAccessId)) ||
                         (status == 2 && !shiftAccessTable.Contains(a.ShiftAccessId))) &&
                        (
                            (empStatus == 1 && ed.SeperationStatus == 0) ||
                            (empStatus == 2 && ed.SeperationStatus > 0) ||
                            ((empStatus == 1 || empStatus == 2) &&
                                _context.HrEmpReportings.Any(er =>
                                    er.EmpId == ed.EmpId &&
                                    er.ReprotToWhome == newEmpId &&
                                    _context.HrEmpMasters.Any(em =>
                                        em.EmpId == er.EmpId &&
                                        em.SeperationStatus == (empStatus == 1 ? 0 : 1))))
                        )
                    select new { a, ed, w, p, br, des })
                    .AsEnumerable()
                    .Join(firstShifts, x => x.a.ShiftId, fs => fs.ShiftId, (x, fs) => new { x, fs })
                    .GroupJoin(secondShifts, xf => xf.fs.ShiftId, ss => ss.ShiftId, (xf, ssGroup) => new { xf, ssGroup })
                    .SelectMany(
        temp => temp.ssGroup.DefaultIfEmpty(),
        (temp, ss) => new ShiftAccessDto
        {
            ShiftAccessID = temp.xf.x.a.ShiftAccessId,
            Emp_Id = temp.xf.x.ed.EmpId,
            EmployeeName = temp.xf.x.ed.Name + "|| " + temp.xf.x.ed.EmpCode,
            ShiftID = temp.xf.x.a.ShiftId,
            ShiftName = temp.xf.fs.ShiftName,
            ShiftCodeName = temp.xf.fs.ShiftCode + " (" +
                            ConvertDecimalToTime(temp.xf.fs.StartTime) + " to " +
                            ConvertDecimalToTime(temp.xf.fs.EndTime) +
                            (ss != null ? " - " + ConvertDecimalToTime(ss.StartTime) + " to " + ConvertDecimalToTime(ss.EndTime) : "") + ")",
            WeekName = temp.xf.x.w?.Name,
            EmployeeID = temp.xf.x.a.EmployeeId,
            WeekEndMasterID = temp.xf.x.w?.WeekEndMasterId,
            StartTime = temp.xf.fs.StartTime.ToString(),
            EndTime = ConvertDecimalToTime(ss?.EndTime ?? temp.xf.fs.EndTime),
            ShiftStartEndTime = "(" +
                                ConvertDecimalToTime(temp.xf.fs.StartTime) + " to " +
                                ConvertDecimalToTime(temp.xf.fs.EndTime) +
                                (ss != null ? " - " + ConvertDecimalToTime(ss.StartTime) + " to " + ConvertDecimalToTime(ss.EndTime) : "") + ")",
            ValidDatefrom = temp.xf.x.a.ValidDatefrom?.ToString("dd/MM/yyyy") ?? "",
            ValidDateTo = temp.xf.x.a.ValidDateTo?.ToString("dd/MM/yyyy") ?? "",
            Branch = temp.xf.x.br.Branch,
            Designation = temp.xf.x.des.Designation,
            ProjectName = temp.xf.x.p?.ProjectName
        })

                    .ToList();

                var totalCount = shiftAccessData.Count();
                var pagedData = shiftAccessData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Data = pagedData
                });
            }






            //        else
            //        {
            //            // 1. Get EmpEntity string of logged-in employee
            //            var empEntityString = _context.HrEmpMasters
            //                .Where(e => e.EmpId == newEmpId)
            //                .Select(e => e.EmpEntity)
            //                .FirstOrDefault();

            //            // 2. Split EmpEntity into a list
            //            var splitItems = SplitStrings_XML(empEntityString, ',').ToList();

            //            // 3. Assign LinkLevel starting from 2
            //            var ctNewList = splitItems
            //                .Select((item, index) => new
            //                {
            //                    Item = item,
            //                    LinkLevel = index + 2
            //                })
            //                .ToList();

            //            // 4. Build applicable final access list
            //            var accessItemsFromRole = _context.EntityAccessRights02s
            //.Where(x => x.RoleId == roleId)
            //.AsEnumerable()
            //.SelectMany(s => SplitStrings_XML(s.LinkId, ','))
            //.Where(item => !string.IsNullOrWhiteSpace(item))
            //.Select(item => item.Trim())
            //.Distinct()
            //.ToList();

            //            var applicableFinal = ctNewList
            // .Where(x => lnkLev > 0 && x.LinkLevel >= lnkLev && !string.IsNullOrWhiteSpace(x.Item))
            // .Select(x => new
            // {
            //     x.Item,
            //     x.LinkLevel
            // })
            // .ToList();

            //            var parsedAccessList = new List<(int ItemId, int? LinkLevel)>();

            //            // Add role-based access items (LinkLevel = null)
            //            parsedAccessList.AddRange(
            //                accessItemsFromRole
            //                    .Select(item =>
            //                    {
            //                        var success = int.TryParse(item, out var id);
            //                        return (ItemId: success ? (int?)id : null, LinkLevel: (int?)null);
            //                    })
            //                    .Where(x => x.ItemId.HasValue)
            //                    .Select(x => (x.ItemId.Value, x.LinkLevel))
            //            );

            //            // Add ctNewList items (which have LinkLevel)
            //            parsedAccessList.AddRange(
            //                applicableFinal
            //                    .Select(x =>
            //                    {
            //                        var success = int.TryParse(x.Item, out var id);
            //                        return (ItemId: success ? (int?)id : null, LinkLevel: (int?)x.LinkLevel);
            //                    })
            //                    .Where(x => x.ItemId.HasValue)
            //                    .Select(x => (ItemId: x.ItemId.Value, LinkLevel: x.LinkLevel))
            //            );


            //            // switch to LINQ-to-Objects for client-side filtering

            //            // 7. Apply hierarchical access logic using parsedAccessList
            //            var employeesWithAccess = empData
            //.Where(x =>
            //    (x.d.IsSave == null || x.d.IsSave == 0) &&
            //    (x.d.IsDelete == null || x.d.IsDelete == false) &&
            //    parsedAccessList.Any(access =>
            //        access.LinkLevel.HasValue && (
            //            (access.LinkLevel == 1 && x.h.LevelOneId == access.ItemId) ||
            //            (access.LinkLevel == 2 && x.h.LevelTwoId == access.ItemId) ||
            //            (access.LinkLevel == 3 && x.h.LevelThreeId == access.ItemId) ||
            //            (access.LinkLevel == 4 && x.h.LevelFourId == access.ItemId) ||
            //            (access.LinkLevel == 5 && x.h.LevelFiveId == access.ItemId) ||
            //            (access.LinkLevel == 6 && x.h.LevelSixId == access.ItemId) ||
            //            (access.LinkLevel == 7 && x.h.LevelSevenId == access.ItemId) ||
            //            (access.LinkLevel == 8 && x.h.LevelEightId == access.ItemId) ||
            //            (access.LinkLevel == 9 && x.h.LevelNineId == access.ItemId) ||
            //            (access.LinkLevel == 10 && x.h.LevelTenId == access.ItemId) ||
            //            (access.LinkLevel == 11 && x.h.LevelElevenId == access.ItemId) ||
            //            (access.LinkLevel == 12 && x.h.LevelTwelveId == access.ItemId)
            //        )
            //    ) &&
            //    (
            //        (empStatus == 1 && x.d.SeperationStatus == 0) ||
            //        (empStatus == 2 && x.d.SeperationStatus > 0)
            //    )
            //)
            //.Select(x => x.d.EmpId)
            //.Distinct()
            //.ToList();


            //            // 8. Direct reportees logic (EF-friendly)
            //            var directReportees = (
            //                from rep in _context.HrEmpReportings
            //                join emp in _context.HrEmpMasters on rep.EmpId equals emp.EmpId
            //                where rep.ReprotToWhome == newEmpId &&
            //                      (
            //                          (empStatus == 1 && emp.SeperationStatus == 0) ||
            //                          (empStatus == 2 && emp.SeperationStatus > 0)
            //                      ) &&
            //                      (emp.IsSave == null || emp.IsSave == 0) &&
            //                      (emp.IsDelete == null || emp.IsDelete == false)
            //                select emp.EmpId
            //            ).Distinct().ToList();

            //            // 9. Union both employee groups
            //            var cteEmployeeList = employeesWithAccess
            //                .Union(directReportees)
            //                .ToList();

            //            // 10. Continue with shift period logic
            //            var (firstShifts, secondShifts) = GetShiftPeriods();

            //            return Ok(); // Return your desired output here
            //        }
            return Ok();

        }
    }
}
