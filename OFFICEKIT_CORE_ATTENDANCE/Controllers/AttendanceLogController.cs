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
    public class AttendanceLogController(IAttendanceLogRepository attendanceLog,IAttendanceLogService attendanceLogService) : ControllerBase
    {

        [HttpPost("employee-names")]
        public async Task<IActionResult> GetAllEmployeeNames()
        {
            var result = await attendanceLog.GetAllEmployeeNamesAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return StatusCode(result.StatusCode, result.Error);

        }
        [HttpPost("get-employee-details")]
        public async Task<IActionResult> GetEmployeeDetails([FromBody] AttendanceLogEmployeeDetailsRequestDto request)
        {
            var result = await attendanceLog.GetEmployeeDetailsAsync(request);

            if (result != null)
            {
                return Ok(result); // Return 200 OK with the data
            }

            return NotFound("Employee details not found."); // Return 404 if not found
        }
        [HttpPost("get-attendance-logs")]
        public async Task<IActionResult> GetAttendanceLogs([FromBody] AttLogListRequestDto request)
        {
            try
            {
                var logs = await attendanceLog.GetAttendanceLogsAsync(request);

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

        [HttpPost("add-manual-logs")]
        public async Task<IActionResult> AddAttManualLog(ManualAttendanceLogRequestDto manualLogDto)
        {
            
            if (manualLogDto == null) return BadRequest("Invalid request data.");

            bool isAdded = await attendanceLogService.AddManualLogAsync(manualLogDto);
            if (!isAdded) return Conflict("Duplicate log entry detected.");
            return isAdded ? Ok("Manual attendance log added successfully.") : StatusCode(500, "Failed to add log.");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendanc
    }
}
