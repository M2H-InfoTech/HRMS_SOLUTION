using HRMS.EmployeeInformation.Service.InterfaceB;
using Microsoft.AspNetCore.Mvc;

namespace EMPLOYEE_INFORMATION.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeBController : Controller
    {
        private readonly IEmployeeInformationServiceB _employeeInformationB;
        public EmployeeBController(IEmployeeInformationServiceB employeeInformationB)
        {
            _employeeInformationB = employeeInformationB;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> QualificationDocumentsDetails(int QualificationId)
        {
            var Qualification = await _employeeInformationB.QualificationDocumentsDetails(QualificationId);
            return new JsonResult(Qualification);
        }

    }
}
