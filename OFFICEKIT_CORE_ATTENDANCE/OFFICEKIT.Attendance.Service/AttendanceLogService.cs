  using OFFICEKIT_CORE_ATTENDANCE.Mapper;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Repository;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Service
{
    public class AttendanceLogService(IAttendanceLogRepository attendanceLogRepository,IShiftSettingsRepository shiftSettingsRepository) : IAttendanceLogService
    {
        

        public async Task<IEnumerable<AttLogListResponseDto>> GetAttendanceLogsAsync(AttLogListRequestDto request)
        {
            return await attendanceLogRepository.GetAttendanceLogsAsync(request);
        }
        public async Task<bool> AddOrUpdateManualAttendanceLogAsync(ManualAttendanceLogRequestDto manualLogDto)
        {
            var existingLog = await attendanceLogRepository.GetExistingLogAsync(manualLogDto);

            if (existingLog != null)
            {
                existingLog.LogDate = manualLogDto.LogDate;
                existingLog.Direction = manualLogDto.Direction;
                existingLog.UpdatedDate = DateTime.UtcNow;

                return await attendanceLogRepository.UpdateAttendanceLog(existingLog);
               
            }

            // Convert DTO to Entity using the custom mapper
            var mappedAttendanceLog = AttendanceLogMapper.AttenManualDataMap(manualLogDto);

            // Pass the mapped entity to the repository
            return await attendanceLogRepository.AddManualAttendanceLogAsync(mappedAttendanceLog);
        }

        public Task<bool> DeleteAttendanceLogAsync(int id)
        {
            return attendanceLogRepository.DeleteAttendanceLogAsync(id);
        }

        public async Task<PaginatedResultDto> GetShiftAccessDetails(int shiftAccessId, int entryBy, int roleId, int status, int empStatus, int pageNumber, int pageSize)
        {
            return await shiftSettingsRepository.GetShiftAccessDetails(shiftAccessId, entryBy, roleId, status, empStatus, pageNumber, pageSize);
        }
    }
}

