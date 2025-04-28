using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.LeaveBalance
{
    public class LeaveBalanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
