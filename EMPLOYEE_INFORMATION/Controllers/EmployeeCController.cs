using HRMS.EmployeeInformation.DTO.DTOs;
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

        [HttpGet]
        public async Task<IActionResult> FillTravelType ()
            {
            var fillTravelType = await _employeeInformationC.FillTravelType ();
            return new JsonResult (fillTravelType);
            }
        [HttpGet]
        public async Task<IActionResult> FillEmployeesBasedOnRole (int firstEntityId, int secondEntityId,string transactionType)
            {
            var fillEmployeesBasedOnRole = await _employeeInformationC.FillEmployeesBasedOnRole (firstEntityId, secondEntityId, transactionType);
            return new JsonResult (fillEmployeesBasedOnRole);
            }
        [HttpGet]   
        public async Task<IActionResult> GetDependentDetails (int employeeId)
            {
            var getDependentDetails = await _employeeInformationC.GetDependentDetails (employeeId);
            return new JsonResult (getDependentDetails);
            }
        [HttpPost]
        public async Task<IActionResult> SaveDependentEmp ([FromBody] SaveDependentEmpDto SaveDependentEmp)   // For Document and Bank Insertion
            {
            var saveDependentEmp = await _employeeInformationC.SaveDependentEmp (SaveDependentEmp);

            return Ok (saveDependentEmp);
            }
        [HttpGet]
        public async Task<IActionResult> RetrieveEducation ( )
            {
            var retrieveEducation = await _employeeInformationC.RetrieveEducation ( );
            return new JsonResult (retrieveEducation);

            }
        [HttpGet]
        public async Task<IActionResult> RetrieveCourse ( )
            {
            var retrieveCourse = await _employeeInformationC.RetrieveCourse ( );
            return new JsonResult (retrieveCourse);

            }
        [HttpGet]
        public async Task<IActionResult> RetrieveSpecial ( )
            {
            var retrieveSpecial = await _employeeInformationC.RetrieveSpecial ( );
            return new JsonResult (retrieveSpecial);

            }
        [HttpGet]
        public async Task<IActionResult> RetrieveUniversity ( )
            {
            var retrieveUniversity = await _employeeInformationC.RetrieveUniversity ( );
            return new JsonResult (retrieveUniversity);

            }

        }
}
