  using OFFICEKIT_CORE_ATTENDANCE.Mapper;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Repository;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Service
{
    public class AttendanceLogService(IAttendanceLogRepository attendanceLogRepository) : IAttendanceLogService
    {
        

        public async Task<IEnumerable<AttLogListResponseDto>> GetAttendanceLogsAsync(AttLogListRequestDto request)
        {
            return await attendanceLogRepository.GetAttendanceLogsAsync(request);
        }
        public async Task<bool> AddManualLogAsync(ManualAttendanceLogRequestDto manualLogDto)
        {
            var existingLog = await attendanceLogRepository.GetExistingLogAsync(manualLogDto);

            if (existingLog != null)
            {
                return false; // Duplicate log found, reject the request
            }

            // Convert DTO to Entity using the custom mapper
            var mappedAttendanceLog = AttendanceLogMapper.AttenManualDataMap(manualLogDto);

            // Pass the mapped entity to the repository
            return await attendanceLogRepository.AddManualAttendanceLogAsync(mappedAttendanceLog);
        }

    }
}

