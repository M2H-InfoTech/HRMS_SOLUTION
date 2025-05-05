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
    }
}
