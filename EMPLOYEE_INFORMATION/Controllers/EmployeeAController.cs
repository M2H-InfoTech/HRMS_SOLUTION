using HRMS.EmployeeInformation.Service.InterfaceA;
using Microsoft.AspNetCore.Mvc;

namespace EMPLOYEE_INFORMATION.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeAController : Controller
    {
        private readonly IEmployeeInformationServiceA _employeeInformationA;
        public EmployeeAController(IEmployeeInformationServiceA employeeInformationA)
        {
            _employeeInformationA = employeeInformationA;
        }
        [HttpGet]
        public string Index()
        {
            return "View()";
        }
    }
}
