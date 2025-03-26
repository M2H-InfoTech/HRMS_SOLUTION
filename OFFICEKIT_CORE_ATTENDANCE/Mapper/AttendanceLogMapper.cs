using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

namespace OFFICEKIT_CORE_ATTENDANCE.Mapper
{
    public class AttendanceLogMapper
    {
        public static AttLogListResponseDto AttendanceLogMap(Attendancelog log, EmployeeDetail employee)
        {
            return new AttLogListResponseDto
            {
                AttLogId = log.AttLogId,
                EmpCode = employee.EmpCode,
                DummyDate = log.LogDate.ToString("yyyyMMdd"),
                LogDate = log.LogDate,
                EntryDate = log.UpdatedDate.HasValue ? log.UpdatedDate.Value : log.DownloadDate,
                EntryBy = log.IsManul == 0 ? "Device" : log.IsManul == 1 ? "Manual" : "Unknown",
                Direction = log.Direction ?? "",
                PunchTime = log.LogDate.ToString("HH:mm")
            };
        }
        public static AttendanceLogEmployeeDetailsDto AttLogEmployeeDetails(EmployeeDetail a, DesignationDetail b, DepartmentDetail c)
        {
            return new AttendanceLogEmployeeDetailsDto
            {
                EmpCode = a.EmpCode,
                Name = a.Name,
                EmpId = a.EmpId,
                Designation = b.Designation,
                Department = c.Department
            };
        }
        public static Attendancelog AttenManualDataMap(ManualAttendanceLogRequestDto request)
        {
            return new Attendancelog
            {
                DeviceLogId = 0,
                DownloadDate = DateTime.UtcNow,
                DeviceId = 0,
                UserId = request.EmployeeId.ToString(), // Assigning EmployeeId as UserId
                EmployeeId = request.EmployeeId, // The person whose attendance is logged
                LogDate = request.LogDate,
                Direction = request.Direction,
                IsManul = 1, // Marking it as a manual entry
                UpdatedDate = DateTime.UtcNow
            };
        }
    }
}
