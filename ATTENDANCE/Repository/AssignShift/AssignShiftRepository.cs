using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Models.Entity;
using Microsoft.EntityFrameworkCore;


namespace ATTENDANCE.Service.AssignShift
{
    public class AssignShiftRepository:IAssignShiftRepository
    {
        
        //private readonly EmployeeDBContext _employeeDbContext;

        //// Normal constructor for DI
        //public AssignShiftRepository( EmployeeDBContext employeeDBContext)
        //{
            
        //    _employeeDbContext = employeeDBContext;
        //}
        //private static IEnumerable<string> SplitStrings_XML(string list, char delimiter)
        //{
        //    if (string.IsNullOrWhiteSpace(list))
        //        return Enumerable.Empty<string>();


        //    return list.Split(delimiter)
        //               .Select(item => item.Trim()) // Trim whitespace from each item
        //               .Where(item => !string.IsNullOrEmpty(item)); // Exclude empty items
        //}
        //private string ConvertDecimalToTime(decimal? decimalHours)
        //{
        //    if (decimalHours == null)
        //        return ""; // or "00:00:00" or "N/A" depending on your preference

        //    TimeSpan time = TimeSpan.FromHours((double)decimalHours.Value);
        //    return time.ToString(@"hh\:mm\:ss");
        //}

        //private async Task<(List<dynamic> FirstShifts, List<dynamic> SecondShifts)> GetShiftPeriodsAsync()
        //{
        //    var shiftPeriods = await _employeeDbContext.HrShift00s
        //        .Join(_employeeDbContext.HrShift01s,
        //              shift => shift.ShiftId,
        //              period => period.ShiftId,
        //              (shift, period) => new
        //              {
        //                  shift.ShiftCode,
        //                  shift.ShiftId,
        //                  shift.ShiftName,
        //                  shift.ShiftType,
        //                  period.StartTime,
        //                  period.EndTime,
        //                  period.Shift01Id
        //              })
        //        .ToListAsync() // Fetch data asynchronously from the database
        //        .ContinueWith(task => task.Result
        //            .GroupBy(x => x.ShiftId)  // Group by ShiftId in memory
        //            .SelectMany(g => g
        //                .OrderBy(x => x.Shift01Id)
        //                .Select((x, index) => new
        //                {
        //                    x.ShiftCode,
        //                    x.ShiftId,
        //                    x.ShiftName,
        //                    x.ShiftType,
        //                    x.StartTime,
        //                    x.EndTime,
        //                    PeriodNum = index + 1
        //                }))
        //            .ToList());  // Continue processing after fetching data



        //    var firstShifts = shiftPeriods
        //        .Where(x => x.PeriodNum == 1)
        //        .Cast<dynamic>()
        //        .ToList();

        //    var secondShifts = shiftPeriods
        //        .Where(x => x.PeriodNum == 2 && x.ShiftType == "Split")
        //        .Cast<dynamic>()
        //        .ToList();

        //    return (firstShifts, secondShifts);
        //}
        //private List<long> GetValidShiftAccessIds(DateTime currentDate)
        //{
        //    return _employeeDbContext.ShiftMasterAccesses
        //        .Where(s => currentDate >= s.ValidDatefrom &&
        //                   (s.ValidDateTo == null || currentDate <= s.ValidDateTo))
        //        .Select(s => s.ShiftAccessId)
        //        .ToList();
        //}




        //public async Task<object> GetShiftAccessDetails(int shiftAccessId, int entryBy, int roleId, int status, int empStatus, int pageNumber, int pageSize)
        //{
        //    DateTime shiftCurrentDate = DateTime.UtcNow.Date;

        //    var result = await (
        //        from tm in _employeeDbContext.TransactionMasters.Where(x => x.TransactionType == "Shift")
        //        from eur in _employeeDbContext.HrEmployeeUserRelations.Where(x => x.UserId == entryBy).DefaultIfEmpty()
        //        from sar in _employeeDbContext.SpecialAccessRights.Where(x => x.RoleId == roleId).DefaultIfEmpty()
        //        select new
        //        {
        //            TransId = tm.TransactionId,
        //            NewEmpId = eur != null ? eur.EmpId : 0,
        //            LinkLevel = sar != null ? sar.LinkLevel : 0
        //        }).FirstOrDefaultAsync();



        //    int transId = result?.TransId ?? 0;
        //    int newEmpId = result?.NewEmpId ?? 0;
        //    int lnkLev = result?.LinkLevel ?? 0;


        //    if (lnkLev == 15)
        //    {

        //        var accessEntities = await _employeeDbContext.EntityAccessRights02s
        //           .Where(s => s.RoleId == roleId && s.LinkLevel == 15)
        //           .ToListAsync(); // async + efficient SQL execution

        //        var directAccessEntities = accessEntities
        //            .SelectMany(s => SplitStrings_XML(s.LinkId ?? string.Empty, ','))
        //            .Distinct()
        //            .ToList();


        //        var empEntityList = (await _employeeDbContext.HrEmpMasters
        //            .Where(e => e.EmpId == newEmpId)
        //            .ToListAsync()) // Asynchronously load matching employee(s)
        //            .SelectMany(e => SplitStrings_XML(e.EmpEntity ?? string.Empty, ','))
        //            .Select((item, index) => new { item, LinkLevel = index + 2 })
        //            .ToList();



        //        var applicableEntities = directAccessEntities
        //                .Select((item, index) => new { item, LinkLevel = index + 1 })
        //                .Union(
        //                    empEntityList
        //                        .Where(e => lnkLev > 0 && e.LinkLevel >= lnkLev && !string.IsNullOrWhiteSpace(e.item))
        //                )
        //                .ToList();

        //        var (firstShifts, secondShifts) = await GetShiftPeriodsAsync();
        //        var shiftAccessTable = GetValidShiftAccessIds(shiftCurrentDate);

        //        //var ResultData = GetShiftAccessData(shiftAccessId, status, empStatus, newEmpId, shiftAccessTable);





        //        var preJoinShiftData = await (
        //            from a in _employeeDbContext.ShiftMasterAccesses
        //            join ed in _employeeDbContext.EmployeeDetails on a.EmployeeId equals ed.EmpId
        //            join w in _employeeDbContext.WeekEndMasters on a.WeekEndMasterId equals w.WeekEndMasterId into wGroup
        //            from w in wGroup.DefaultIfEmpty()
        //            join p in _employeeDbContext.ProjectMasters on a.ProjectId equals p.Id into pGroup
        //            from p in pGroup.DefaultIfEmpty()
        //            join br in _employeeDbContext.BranchDetails on ed.BranchId equals br.LinkId
        //            join des in _employeeDbContext.DesignationDetails on ed.DesigId equals des.LinkId
        //            where
        //                (shiftAccessId == 0 || a.ShiftAccessId == shiftAccessId) &&
        //                ((status == 1 && shiftAccessTable.Contains(a.ShiftAccessId)) ||
        //                 (status == 2 && !shiftAccessTable.Contains(a.ShiftAccessId))) &&
        //                (
        //                    (empStatus == 1 && ed.SeperationStatus == 0) ||
        //                    (empStatus == 2 && ed.SeperationStatus > 0) ||
        //                    ((empStatus == 1 || empStatus == 2) &&
        //                        _employeeDbContext.HrEmpReportings.Any(er =>
        //                            er.EmpId == ed.EmpId &&
        //                            er.ReprotToWhome == newEmpId &&
        //                            _employeeDbContext.HrEmpMasters.Any(em =>
        //                                em.EmpId == er.EmpId &&
        //                                em.SeperationStatus == (empStatus == 1 ? 0 : 1))))
        //                )
        //            select new { a, ed, w, p, br, des }
        //        ).ToListAsync();

        //        var shiftAccessData = preJoinShiftData
        //            .Join(firstShifts, x => x.a.ShiftId, fs => fs.ShiftId, (x, fs) => new { x, fs })
        //            .GroupJoin(secondShifts, xf => xf.fs.ShiftId, ss => ss.ShiftId, (xf, ssGroup) => new { xf, ssGroup })
        //            .SelectMany(
        //                temp => temp.ssGroup.DefaultIfEmpty(),
        //                (temp, ss) => new ShiftAccessDto
        //                {
        //                    ShiftAccessID = temp.xf.x.a.ShiftAccessId,
        //                    Emp_Id = temp.xf.x.ed.EmpId,
        //                    EmployeeName = temp.xf.x.ed.Name + "|| " + temp.xf.x.ed.EmpCode,
        //                    ShiftID = temp.xf.x.a.ShiftId,
        //                    ShiftName = temp.xf.fs.ShiftName,
        //                    ShiftCodeName = temp.xf.fs.ShiftCode + " (" +
        //                                    ConvertDecimalToTime(temp.xf.fs.StartTime) + " to " +
        //                                    ConvertDecimalToTime(temp.xf.fs.EndTime) +
        //                                    (ss != null ? " - " + ConvertDecimalToTime(ss.StartTime) + " to " + ConvertDecimalToTime(ss.EndTime) : "") + ")",
        //                    WeekName = temp.xf.x.w?.Name,
        //                    EmployeeID = temp.xf.x.a.EmployeeId,
        //                    WeekEndMasterID = temp.xf.x.w?.WeekEndMasterId,
        //                    StartTime = temp.xf.fs.StartTime.ToString(),
        //                    EndTime = ConvertDecimalToTime(ss?.EndTime ?? temp.xf.fs.EndTime),
        //                    ShiftStartEndTime = "(" +
        //                                        ConvertDecimalToTime(temp.xf.fs.StartTime) + " to " +
        //                                        ConvertDecimalToTime(temp.xf.fs.EndTime) +
        //                                        (ss != null ? " - " + ConvertDecimalToTime(ss.StartTime) + " to " + ConvertDecimalToTime(ss.EndTime) : "") + ")",
        //                    ValidDatefrom = temp.xf.x.a.ValidDatefrom?.ToString("dd/MM/yyyy") ?? "",
        //                    ValidDateTo = temp.xf.x.a.ValidDateTo?.ToString("dd/MM/yyyy") ?? "",
        //                    Branch = temp.xf.x.br.Branch,
        //                    Designation = temp.xf.x.des.Designation,
        //                    ProjectName = temp.xf.x.p?.ProjectName
        //                })
        //            .ToList();

        //        var totalCount = shiftAccessData.Count();
        //        var pagedData = shiftAccessData
        //            .Skip((pageNumber - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToList();
        //        return new PaginatedResultDto
        //        {
        //            TotalCount = totalCount,
        //            PageNumber = pageNumber,
        //            PageSize = pageSize,
        //            Data = pagedData
        //        };
        //    }



        //    else
        //    {
        //        // 1. Get EmpEntity string of logged-in employee
        //        var empEntityString = await _employeeDbContext.HrEmpMasters
        //            .Where(e => e.EmpId == newEmpId)
        //            .Select(e => e.EmpEntity)
        //            .FirstOrDefaultAsync();

        //        // 2. Split EmpEntity into a list
        //        var splitItems = empEntityString.Split(',').Select(int.Parse).ToList();


        //        var ctnew = splitItems
        //            .Select((item, index) => new
        //            {
        //                Item = item.ToString(),            // string
        //                LinkLevel = (int?)(index + 2)      // nullable int
        //            })
        //            .ToList();




        //        // 4. Build applicable final access list
        //        var entityAccessRights = await _employeeDbContext.EntityAccessRights02s
        //            .Where(s => s.RoleId == roleId)
        //            .ToListAsync(); // Asynchronous database call

        //        var accessItemsFromRole = entityAccessRights
        //            .SelectMany(s =>
        //                SplitStrings_XML(s.LinkId, ',')
        //                    .Where(item => !string.IsNullOrWhiteSpace(item))
        //                    .Select(item => new
        //                    {
        //                        Item = item,
        //                        LinkLevel = s.LinkLevel
        //                    })
        //            )
        //            .ToList();


        //        var filteredCtnew = ctnew
        //            .Where(x => lnkLev > 0 && x.LinkLevel >= lnkLev)
        //            .ToList();


        //        var applicableFinal = accessItemsFromRole
        //            .Union(filteredCtnew)
        //            .ToList();

        //        var data = await _employeeDbContext.HrEmpMasters.ToListAsync();
        //        var hrmReportings = await _employeeDbContext.HrEmpReportings.ToListAsync();

        //        var highLevelViews = await _employeeDbContext.HighLevelViewTables.ToListAsync();

        //        var hierarchicalEmpIds = data
        //            .Join(highLevelViews, d => d.LastEntity, hv => hv.LastEntityId, (d, hv) => new { d, hv })
        //            .SelectMany(
        //                temp => applicableFinal,
        //                (temp, b) => new { temp.d, temp.hv, b }
        //            )
        //            .Where(temp =>
        //                (temp.d.IsSave ?? 0) == 0 &&
        //                !temp.d.IsDelete.GetValueOrDefault() &&
        //                (
        //                    (temp.hv.LevelOneId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 1) ||
        //                    (temp.hv.LevelTwoId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 2) ||
        //                    (temp.hv.LevelThreeId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 3) ||
        //                    (temp.hv.LevelFourId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 4) ||
        //                    (temp.hv.LevelFiveId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 5) ||
        //                    (temp.hv.LevelSixId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 6) ||
        //                    (temp.hv.LevelSevenId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 7) ||
        //                    (temp.hv.LevelEightId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 8) ||
        //                    (temp.hv.LevelNineId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 9) ||
        //                    (temp.hv.LevelTenId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 10) ||
        //                    (temp.hv.LevelElevenId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 11) ||
        //                    (temp.hv.LevelTwelveId == int.Parse(temp.b.Item) && temp.b.LinkLevel == 12)
        //                ) &&
        //                (
        //                    (empStatus == 1 && temp.d.SeperationStatus == 0) ||
        //                    (empStatus == 2 && temp.d.SeperationStatus > 0)
        //                )
        //            )
        //            .Select(temp => temp.d.EmpId)
        //            .Distinct()
        //            .ToList();
        //        // 👈 Materialize here

        //        var directReportingEmpIds = (
        //            from r in hrmReportings
        //            join d in data on r.EmpId equals d.EmpId
        //            where
        //                r.ReprotToWhome == newEmpId &&
        //                d.SeperationStatus == 0 &&
        //                d.IsSave == 0 &&
        //                !d.IsDelete.GetValueOrDefault()
        //            select r.EmpId
        //        ).Distinct().ToList(); // 👈 Materialize here

        //        var employeeIds = _employeeDbContext.EmployeeDetails
        //            .Where(a =>
        //                hierarchicalEmpIds.Contains(a.EmpId) ||
        //                (empStatus == 1 && directReportingEmpIds.Contains(a.EmpId)) ||
        //                (empStatus == 2 && directReportingEmpIds.Contains(a.EmpId))
        //            )
        //            .Select(a => a.EmpId)
        //            .Distinct()
        //            .ToList();

        //        var (firstShifts, secondShifts) = await GetShiftPeriodsAsync();
        //        var shiftAccessTable = GetValidShiftAccessIds(shiftCurrentDate);

        //        var shiftAccessRaw = await (
        //            from a in _employeeDbContext.ShiftMasterAccesses
        //            join ed in _employeeDbContext.EmployeeDetails on a.EmployeeId equals ed.EmpId
        //            join w in _employeeDbContext.WeekEndMasters on a.WeekEndMasterId equals w.WeekEndMasterId into wGroup
        //            from w in wGroup.DefaultIfEmpty()
        //            join p in _employeeDbContext.ProjectMasters on a.ProjectId equals p.Id into pGroup
        //            from p in pGroup.DefaultIfEmpty()
        //            join br in _employeeDbContext.BranchDetails on ed.BranchId equals br.LinkId
        //            join des in _employeeDbContext.DesignationDetails on ed.DesigId equals des.LinkId
        //            where employeeIds.Contains(ed.EmpId)
        //            where
        //                (shiftAccessId == 0 || a.ShiftAccessId == shiftAccessId) &&
        //                ((status == 1 && shiftAccessTable.Contains(a.ShiftAccessId)) ||
        //                 (status == 2 && !shiftAccessTable.Contains(a.ShiftAccessId)))
        //            select new { a, ed, w, p, br, des }
        //        ).AsNoTracking().ToListAsync();

        //        var shiftAccessDtoList = shiftAccessRaw
        //            .Join(firstShifts, x => x.a.ShiftId, fs => fs.ShiftId, (x, fs) => new { x, fs })
        //            .GroupJoin(secondShifts, xf => xf.fs.ShiftId, ss => ss.ShiftId, (xf, ssGroup) => new { xf, ssGroup })
        //            .SelectMany(
        //                temp => temp.ssGroup.DefaultIfEmpty(),
        //                (temp, ss) => new ShiftAccessDto
        //                {
        //                    ShiftAccessID = temp.xf.x.a.ShiftAccessId,
        //                    Emp_Id = temp.xf.x.ed.EmpId,
        //                    EmployeeName = temp.xf.x.ed.Name + "|| " + temp.xf.x.ed.EmpCode,
        //                    ShiftID = temp.xf.x.a.ShiftId,
        //                    ShiftName = temp.xf.fs.ShiftName,
        //                    ShiftCodeName = temp.xf.fs.ShiftCode + " (" +
        //                                    ConvertDecimalToTime(temp.xf.fs.StartTime) + " to " +
        //                                    ConvertDecimalToTime(temp.xf.fs.EndTime) +
        //                                    (ss != null ? " - " + ConvertDecimalToTime(ss.StartTime) + " to " + ConvertDecimalToTime(ss.EndTime) : "") + ")",
        //                    WeekName = temp.xf.x.w?.Name,
        //                    EmployeeID = temp.xf.x.a.EmployeeId,
        //                    WeekEndMasterID = temp.xf.x.w?.WeekEndMasterId,
        //                    StartTime = temp.xf.fs.StartTime.ToString(),
        //                    EndTime = ConvertDecimalToTime(ss?.EndTime ?? temp.xf.fs.EndTime),
        //                    ShiftStartEndTime = "(" +
        //                                        ConvertDecimalToTime(temp.xf.fs.StartTime) + " to " +
        //                                        ConvertDecimalToTime(temp.xf.fs.EndTime) +
        //                                        (ss != null ? " - " + ConvertDecimalToTime(ss.StartTime) + " to " + ConvertDecimalToTime(ss.EndTime) : "") + ")",
        //                    ValidDatefrom = temp.xf.x.a.ValidDatefrom?.ToString("dd/MM/yyyy") ?? "",
        //                    ValidDateTo = temp.xf.x.a.ValidDateTo?.ToString("dd/MM/yyyy") ?? "",
        //                    Branch = temp.xf.x.br.Branch,
        //                    Designation = temp.xf.x.des.Designation,
        //                    ProjectName = temp.xf.x.p?.ProjectName
        //                }
        //            )
        //            .ToList();



        //        var totalCount = shiftAccessDtoList.Count();
        //        var pagedData = shiftAccessDtoList
        //            .Skip((pageNumber - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToList();

        //        var resultData = new PaginatedResultDto
        //        {
        //            TotalCount = totalCount,
        //            PageNumber = pageNumber,
        //            PageSize = pageSize,
        //            Data = pagedData
        //        };

        //        return resultData;

        //    }


        //}

    

    }
}
