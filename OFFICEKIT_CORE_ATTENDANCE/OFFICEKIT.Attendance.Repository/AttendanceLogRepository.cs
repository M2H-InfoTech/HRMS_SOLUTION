using Microsoft.EntityFrameworkCore;
using OFFICEKIT_CORE_ATTENDANCE.Mapper;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendace.Data;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;
using OFFICEKIT_CORE_ATTENDANCE.Results.OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Repository
{
    public class AttendanceLogRepository(AttendanceDbContext context) : IAttendanceLogRepository
    {
        /// <summary>
        /// Retrieves all employee names and their IDs.
        /// </summary>
        /// <returns>A list of employees containing EmpId and Name.</returns>
        public async Task<Result<IEnumerable<EmployeeDetailsDto>>> GetAllEmployeeNamesAsync()
        {
            try
            {
                var employeeDetails = await context.EmployeeDetails
                    .Select(e => new EmployeeDetailsDto
                    {
                        EmpId = e.EmpId,
                        Name = e.Name
                    })
                    .ToListAsync();

                return Result<IEnumerable<EmployeeDetailsDto>>.Success(employeeDetails);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<EmployeeDetailsDto>>.Failure($"An error occurred: {ex.Message}", 500);
            }
        }

        public async Task<IEnumerable<AttendanceLogEmployeeDetailsDto>> GetEmployeeDetailsAsync(AttendanceLogEmployeeDetailsRequestDto request)
        {
            // Convert EmployeeIdList (comma-separated string) into a list of integers
            var employeeIdList = request.EmployeeIdList
                .Split(',')
                .Select(int.Parse)
                .ToList();

            // Fetch Employee Details with Joins
            var result = await (
                from a in context.EmployeeDetails
                join b in context.DesignationDetails on a.DesigId equals b.LinkId
                join c in context.DepartmentDetails on a.DepId equals c.LinkId
                join d in context.Attendancelogs on a.EmpId equals d.EmployeeId into attendanceJoin
                from d in attendanceJoin.DefaultIfEmpty() // Left Join to include employees even without attendance logs
                where employeeIdList.Contains(a.EmpId)
                group new { a, b, c } by new { a.EmpId, a.EmpCode, a.Name, b.Designation, c.Department } into g
                select AttendanceLogMapper.AttLogEmployeeDetails(g.First().a, g.First().b, g.First().c)
            ).ToListAsync();

            return result;
        }
        public async Task<IEnumerable<AttLogListResponseDto>> GetAttendanceLogsAsync(AttLogListRequestDto request)
        {
            if (request == null || request.EmpId <= 0)
            {
                throw new ArgumentException("Invalid request parameters.");
            }

            var fromDate = request.FromDate ?? DateTime.MinValue;
            var toDate = request.ToDate ?? DateTime.MaxValue;

            var logs = await (
                from a in context.Attendancelogs
                join b in context.EmployeeDetails on a.EmployeeId equals b.EmpId
                where a.EmployeeId == request.EmpId && a.LogDate >= fromDate && a.LogDate <= toDate
                orderby a.LogDate ascending
                select new { a, b }
               ).ToListAsync();

            return logs.Select(x=>AttendanceLogMapper.AttendanceLogMap(x.a,x.b)).ToList();
        }



        public async Task<bool> AddManualAttendanceLogAsync(Attendancelog attendanceLog)
        {

            await context.Attendancelogs.AddAsync(attendanceLog);
            return await context.SaveChangesAsync() > 0;


        }
        public async Task<Attendancelog?> GetExistingLogAsync(ManualAttendanceLogRequestDto manualLogDto)
        {
            return await context.Attendancelogs
                .FirstOrDefaultAsync(a => a.EmployeeId == manualLogDto.EmployeeId
                                          && a.LogDate == manualLogDto.LogDate
                                          && a.Direction == manualLogDto.Direction);
        }




    }
}
