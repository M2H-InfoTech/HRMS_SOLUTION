using ATTENDANCE.DTO.Request;
using ATTENDANCE.Service.AssignPolicy;
using ATTENDANCE.Service.AttendancePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATTENDANCE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttendancePolicyController(IAttendancePolicyService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> FillOverTime()
        {
            var result = await service.FillOverTime();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> InsertAttendancePolicy(InsertAttendancePolicyDto request)
        {
            var result = await service.InsertAttendancePolicy(request);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> InsertAttendancePolicyBulk()
        {
            var result = await service.GetLeaveType();
            return Ok(result);
        }
    }
}
