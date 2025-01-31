using System.Diagnostics;
using HRMS_WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MicroServiceClient _microserviceClient;
        public HomeController(ILogger<HomeController> logger, MicroServiceClient microserviceClient)
        {
            _logger = logger;
            _microserviceClient = microserviceClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _microserviceClient.GetDataFromMicroservice();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
