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

        [HttpPost]
        public async Task<IActionResult> GetAllEmployeeNames()
        {
            var result = await attendanceLog.GetAllEmployeeNamesAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return StatusCode(result.StatusCode, result.Error);

        }
        [HttpPost]
        public async Task<IActionResult> GetEmployeeDetails([FromBody] AttendanceLogEmployeeDetailsRequestDto request)
        {
            var result = await attendanceLog.GetEmployeeDetailsAsync(request);

            if (result != null)
            {
                return Ok(result); // Return 200 OK with the data
            }

            return NotFound("Employee details not found."); // Return 404 if not found
        }
        [HttpPost]
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

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateAttManualLog(ManualAttendanceLogRequestDto manualLogDto)
        {
            
            if (manualLogDto == null) return BadRequest("Invalid request data.");

            bool isAdded = await attendanceLogService.AddOrUpdateManualAttendanceLogAsync(manualLogDto);
            if (!isAdded) return Conflict("Duplicate log entry detected.");
            return isAdded ? Ok("Manual attendance log added successfully.") : StatusCode(500, "Failed to add log.");
        }
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendanceLog(int id)
        {
            var result = await attendanceLogService.DeleteAttendanceLogAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Attendance log not found or could not be deleted." });
            }

            return Ok(new { message = "Attendance log deleted successfully." });
        }
    }
}
