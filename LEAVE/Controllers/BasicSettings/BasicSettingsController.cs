using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.BasicSettings
{
    public class BasicSettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
