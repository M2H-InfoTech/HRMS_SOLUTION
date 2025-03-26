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
        [HttpPost]
        public async Task<IActionResult> SubmitAssetDetailsNew([FromBody] SubmitAssetNewDto submitAssetNewDto)
        {
            var result = await _employeeInformationB.SubmitAssetDetailsNewAsync(submitAssetNewDto);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAssetDetailsNew([FromBody] SubmitAssetNewDto submitAssetNewDto, int AssetRole)
        {
            var result = await _employeeInformationB.UpdateAssetDetailsNewAsync(submitAssetNewDto, AssetRole);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetParameter()
        {
            var getAssetParameter = await _employeeInformationB.GetAssetParameterAsync();

            if (getAssetParameter == null || !getAssetParameter.Any())
            {
                return NotFound("No asset parameters found."); // ✅ Return 404 if empty
            }

            return Ok(getAssetParameter);
        }

        [HttpGet]
        public async Task<IActionResult> GenrlCategoryFieldsReason(int Reason_Id)
        {
            var genrlCategoryFieldsReason = await _employeeInformationB.GenrlCategoryFieldsReasonAsync(Reason_Id);
            return new JsonResult(genrlCategoryFieldsReason);
        }
        [HttpGet]
        public async Task<IActionResult> FillAssetsubOnchange1(int ComFieldID, string AssignAssetStatus)
        {
            var fillAssetsubOnchange1 = await _employeeInformationB.FillAssetsubOnchange1Async(ComFieldID, AssignAssetStatus);
            return new JsonResult(fillAssetsubOnchange1);
        }


        [HttpPost]
        public async Task<IActionResult> SavefieldsReasons([FromBody] SaveReasonDto saveReasonDto)
        {
            var result = await _employeeInformationB.SavefieldsReasonsAsync(saveReasonDto);
            return Ok(result);
        }
        
        
    }
}
