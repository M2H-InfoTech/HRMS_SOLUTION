using LEAVE.Service.LeaveBalance;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.LeaveBalance
{
    [Route("api/[controller]/[action]")]
    public class LeaveBalanceController : Controller
    {

        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly ILeaveBalanceService _leaveBalanceService;
        public LeaveBalanceController(ILeaveBalanceService leaveBalanceService)
        {

            _leaveBalanceService = leaveBalanceService;
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveBranchDetails(int instID, int branchID)
        {
            var retrieveBranchDetails = await _leaveBalanceService.RetrieveBranchDetails(instID, branchID);
            return Ok(retrieveBranchDetails);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveApplicationsAsync(int employeeId, int leaveId, string approvalStatus, string? flowStatus, DateTime? leaveFrom, DateTime? leaveTo)
        {

            var fillweekendinclude = await _leaveBalanceService.GetLeaveApplicationsAsync(employeeId, leaveId, approvalStatus, flowStatus, leaveFrom, leaveTo);
            return Ok(fillweekendinclude);
        }
        [HttpGet]
        public async Task<IActionResult> GetLeaveAssignmentEligibleEmployeesAsync(int entryByUserId, int? roleId)
        {
            var leaveAssignmentEligibleEmployees = await _leaveBalanceService.GetLeaveAssignmentEligibleEmployeesAsync(entryByUserId, roleId);
            return Ok(leaveAssignmentEligibleEmployees);
        }

    }
}
