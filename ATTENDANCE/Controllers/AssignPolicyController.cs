using ATTENDANCE.DTO.Request;
using ATTENDANCE.Service.AssignPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATTENDANCE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssignPolicyController(IAssignPolicyService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> DeleteEmployeeShiftpolicy(int shiftId)
        {
            var result = await service.DeleteEmployeeShiftpolicy(shiftId);
            return Ok(result);
        } 

        [HttpGet]
        public async Task<IActionResult> GetAttendancePolicy(int levelId, int empId, string linkId)
        {
            var result = await service.GetAllShiftPolicy(levelId, empId, linkId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> ViewEmployeeShiftPolicyFiltered(int attendanceAccessId, string employeeIds)
        {
            var result = await service.ViewEmployeeShiftPolicyFiltered(attendanceAccessId, employeeIds);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> BulkDeleteEmpPolicy(string ShiftIDs)
        {
            var result = await service.BulkDeleteEmpPolicy(ShiftIDs);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> ViewEmployeeShiftPolicy(ViewEmployeeShiftPolicyDto request)
        {
            var result = await service.ViewEmployeeShiftPolicy(request);
            return Ok(result);
        }
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
    }
}
