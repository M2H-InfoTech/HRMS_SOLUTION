using HRMS.EmployeeInformation.Service.InterfaceC;
using Microsoft.AspNetCore.Mvc;

namespace EMPLOYEE_INFORMATION.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeCController : Controller
    {
        private readonly IEmployeeInformationServiceC _employeeInformationC;
        public EmployeeCController(IEmployeeInformationServiceC employeeInformationC)
        {
            _employeeInformationC = employeeInformationC;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
