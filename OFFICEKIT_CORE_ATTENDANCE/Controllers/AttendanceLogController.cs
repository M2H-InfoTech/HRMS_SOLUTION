using Microsoft.AspNetCore.Mvc;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Request;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Repository;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Service;


namespace OFFICEKIT_CORE_ATTENDANCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceLogController : ControllerBase
    {
        private readonly IAttendanceLogRepository _attendanceLog;
        private readonly IAttendanceLogService _attendanceLogService;

        public AttendanceLogController(IAttendanceLogRepository attendanceLog, IAttendanceLogService attendanceLogService)
        {
            _attendanceLog = attendanceLog;
            _attendanceLogService = attendanceLogService;
        }

        [HttpGet("employee/names")]
        public async Task<IActionResult> GetAllEmployeeNames()
        {
            var result = await _attendanceLog.GetAllEmployeeNamesAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return StatusCode(result.StatusCode, result.Error);
        }

        [HttpPost("employee/details")]
        public async Task<IActionResult> GetEmployeeDetails([FromBody] AttendanceLogEmployeeDetailsRequestDto request)
        {
            var result = await _attendanceLog.GetEmployeeDetailsAsync(request);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound("Employee details not found.");
        }

        [HttpPost("attendance/logs")]
        public async Task<IActionResult> GetAttendanceLogs([FromBody] AttLogListRequestDto request)
        {
            try
            {
                var logs = await _attendanceLog.GetAttendanceLogsAsync(request);

                if (logs == null || !logs.Any())
                {
                    return NotFound("No attendance logs found for the given employee.");
                }

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("manual/log")]
        public async Task<IActionResult> AddOrUpdateAttManualLog([FromBody] ManualAttendanceLogRequestDto manualLogDto)
        {
            if (manualLogDto == null)
                return BadRequest("Invalid request data.");

            bool isAdded = await _attendanceLogService.AddOrUpdateManualAttendanceLogAsync(manualLogDto);
            if (!isAdded)
                return Conflict("Duplicate log entry detected.");

            return Ok("Manual attendance log added successfully.");
        }

        [HttpDelete("log/{id}")]
        public async Task<IActionResult> DeleteAttendanceLog(int id)
        {
            var result = await _attendanceLogService.DeleteAttendanceLogAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Attendance log not found or could not be deleted." });
            }

            return Ok(new { message = "Attendance log deleted successfully." });
        }
    }

}

