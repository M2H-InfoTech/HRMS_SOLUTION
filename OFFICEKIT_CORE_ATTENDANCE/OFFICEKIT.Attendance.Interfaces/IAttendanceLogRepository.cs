using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;
using OFFICEKIT_CORE_ATTENDANCE.Results.OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces
{
    public interface IAttendanceLogRepository
    {
        Task<Result<IEnumerable<EmployeeDetailsDto>>> GetAllEmployeeNamesAsync();
        Task<IEnumerable<AttendanceLogEmployeeDetailsDto>> GetEmployeeDetailsAsync(AttendanceLogEmployeeDetailsRequestDto request);
        Task<IEnumerable<AttLogListResponseDto>> GetAttendanceLogsAsync(AttLogListRequestDto request);
        Task<bool> AddManualAttendanceLogAsync(Attendancelog Request);
        Task<Attendancelog?> GetExistingLogAsync(ManualAttendanceLogRequestDto manualLogDto);

    }
}
