using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Repository.Common;
using HRMS.EmployeeInformation.Service.InterfaceB;
using Microsoft.AspNetCore.Mvc;

namespace EMPLOYEE_INFORMATION.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeBController : Controller
    {
        private readonly IEmployeeInformationServiceB _employeeInformationB;
        private readonly IEmployeeRepository _employeerepositoryB;
        public EmployeeBController(IEmployeeInformationServiceB employeeInformationB, IEmployeeRepository employeerepositoryB)
        {
            _employeeInformationB = employeeInformationB;
            _employeerepositoryB =employeerepositoryB;

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

        //CommunicationExtra
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateCommunication([FromBody] SaveCommunicationSDto communications)
        {
            var empcommunication = await _employeeInformationB.InsertOrUpdateCommunication(communications);

            if (empcommunication == null || !empcommunication.Any())
            {
                return NotFound();
            }

            return Ok(empcommunication);
        }
        //emergency extra
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateCommunicationEmergency([FromBody] SaveCommunicationSDto communications)
        {
            var empcommunication = await _employeeInformationB.InsertOrUpdateCommunicationEmergency(communications);

            if (empcommunication == null || !empcommunication.Any())
            {
                return NotFound();
            }

            return Ok(empcommunication);
        }

        //CommunicationUpdate
        [HttpPost]
        public async Task<IActionResult> UpdateCommunication([FromBody] SaveCommunicationSDto communications)
        {
            var empcommunication = await _employeeInformationB.UpdateCommunication(communications);

            if (empcommunication == null || !empcommunication.Any())
            {
                return NotFound();
            }

            return Ok(empcommunication);
        }
    }
}
