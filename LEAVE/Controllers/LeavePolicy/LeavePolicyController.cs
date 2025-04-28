using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.LeavePolicy
{
    public class LeavePolicyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
