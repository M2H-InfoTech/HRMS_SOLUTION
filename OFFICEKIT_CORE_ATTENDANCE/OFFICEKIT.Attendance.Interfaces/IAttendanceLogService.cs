using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces
{
    public interface IAttendanceLogService
    {
        Task<IEnumerable<AttLogListResponseDto>> GetAttendanceLogsAsync(AttLogListRequestDto request);
        Task<bool> AddOrUpdateManualAttendanceLogAsync(ManualAttendanceLogRequestDto manualLogDto);
        Task<bool> DeleteAttendanceLogAsync(int id);
    }
}
