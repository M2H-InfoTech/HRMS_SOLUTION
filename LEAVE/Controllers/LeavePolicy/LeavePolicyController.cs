using LEAVE.Dto;
using LEAVE.Service.LeavePolicy;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.LeavePolicy
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize]
    public class LeavePolicyController : ControllerBase
    {
        private readonly ILeavePolicyService _leavePolicyService;
        public LeavePolicyController(ILeavePolicyService leavePolicyService)
        {
            _leavePolicyService = leavePolicyService;
        }
        [HttpGet]
        public async Task<IActionResult> FillLeavepolicyAsync(int SecondEntityId, int EmpId)
        {

            var fillLeavepolicy = await _leavePolicyService.FillLeavepolicyAsync(SecondEntityId, EmpId);
            return Ok(fillLeavepolicy);
        }

        [HttpPost]
        public async Task<IActionResult> CreatepolicyAsync(CreatePolicyDto createPolicyDto)
        {
            if (createPolicyDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var createPolicy = await _leavePolicyService.CreatepolicyAsync(createPolicyDto);
            return Ok(createPolicy);
        }
        [HttpGet]
        public async Task<IActionResult> FillleaveAsync(int SecondEntityId, int EmpId)
        {

            var fillleave = await _leavePolicyService.FillleaveAsync(SecondEntityId, EmpId);
            return Ok(fillleave);
        }
        [HttpGet]
        public async Task<IActionResult> FillInstatntLimitAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {

            var fillInstatntLimit = await _leavePolicyService.FillInstatntLimitAsync(LeavePolicyMasterID, LeavePolicyInstanceLimitID);
            return Ok(fillInstatntLimit);
        }
        [HttpGet]
        public async Task<IActionResult> EditFillInstatntLimitLeaveAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {

            var editFillInstatntLimitLeave = await _leavePolicyService.EditFillInstatntLimitLeaveAsync(LeavePolicyMasterID, LeavePolicyInstanceLimitID);
            return Ok(editFillInstatntLimitLeave);
        }

        [HttpGet]
        public async Task<IActionResult> fillweekendinclude(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {

            var fillweekendinclude = await _leavePolicyService.fillweekendinclude(LeavePolicyMasterID, LeavePolicyInstanceLimitID);
            return Ok(fillweekendinclude);
        }
        [HttpGet]
        public async Task<IActionResult> FillHolidayincludeAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {

            var fillweekendinclude = await _leavePolicyService.FillHolidayincludeAsync(LeavePolicyMasterID, LeavePolicyInstanceLimitID);
            return Ok(fillweekendinclude);
        }
        [HttpPost]
        public async Task<IActionResult> InsertInstanceLeaveLimitAsync(LeavePolicyInstanceLimitDto leavePolicyInstanceLimitDto, string compLeaveIDs, int empId)
        {
            if (leavePolicyInstanceLimitDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var leavePolicyInstanceLimit = await _leavePolicyService.InsertInstanceLeaveLimitAsync(leavePolicyInstanceLimitDto, compLeaveIDs, empId);
            return Ok(leavePolicyInstanceLimit);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInstanceLimit(int LeavePolicyInstanceLimitID)
        {

            var createPolicy = await _leavePolicyService.DeleteInstanceLimit(LeavePolicyInstanceLimitID);
            return Ok(createPolicy);
        }        

    }
}
