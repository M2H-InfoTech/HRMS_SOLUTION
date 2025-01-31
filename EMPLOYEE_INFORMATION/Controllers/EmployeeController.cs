using EMPLOYEE_INFORMATION.Helpers;
using EMPLOYEE_INFORMATION.Models.DTOs;
using EMPLOYEE_INFORMATION.Models.EnumFolder;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Repository.Common;
using HRMS.EmployeeInformation.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace EMPLOYEE_INFORMATION.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeInformationService _employeeInformation;
        private readonly TokenService _tokenService;
        public EmployeeController(IEmployeeInformationService employeeInformation, TokenService tokenService)
        {
            _employeeInformation = employeeInformation;
            _tokenService = tokenService;

        }
        [HttpGet]
        public string Index()
        {
            var token = _tokenService.GenerateToken("2311427", "Admin");

            return token;
        }
        [HttpPost]
        public async Task<IActionResult> GetEmployeeById(EmployeeInformationParameters employeeInformationParameters)
        {
            var result = await _employeeInformation.GetEmpData(employeeInformationParameters);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetActiveStatus(int employeeId , string parameterCode , string type)
        {
            var activeStatus = await _employeeInformation.EmployeeStatus(employeeId, parameterCode, type);
            return new JsonResult(activeStatus);
        }
        [HttpGet]
        public async Task<IActionResult> GetProbationStatus()
        {
            var options = Enum.GetValues(typeof(ProbationStatus)).Cast<ProbationStatus>().Select(e => new { Id = (int)e, Name = e.ToString() });

            return await Task.FromResult(Ok(options));
        }

        [HttpGet]
        public async Task<IActionResult> LanguageSkill(int employeeId)
        {
            var languagedata = await _employeeInformation.LanguageSkill(employeeId);
            return new JsonResult(languagedata);
        }

        [HttpGet]
        public async Task<IActionResult> Communication(int employeeId)
        {
            var Communicationdata = await _employeeInformation.Communication(employeeId);
            return new JsonResult(Communicationdata);
        }
        [HttpGet]
        public async Task<IActionResult>CommunicationExtra(int employeeId)
        {
            var CommunicationExtra = await _employeeInformation.CommunicationExtra(employeeId);
            return new JsonResult(CommunicationExtra);
        }

        [HttpGet]

        public async Task<IActionResult> CommunicationEmergency(int employeeId)
        {
            var CommunicationEmergency = await _employeeInformation.CommunicationEmergency(employeeId);
            return new JsonResult(CommunicationEmergency);
        }

        [HttpGet]
        public async Task<IActionResult> HobbiesData(int employeeId)
        {
            var VisaDetailsReport=await _employeeInformation.HobbiesData(employeeId);
            return new JsonResult(VisaDetailsReport);
        }

        [HttpGet]
        public async Task<IActionResult> RewardAndRecognition(int employeeId)
        {
            var RewardAndRecognitionData = await _employeeInformation.RewardAndRecognition(employeeId);
            return new JsonResult(RewardAndRecognitionData);
        }

        [HttpGet]
        public async Task<IActionResult> Qualification(int employeeId)
        {
            var Qualification = await _employeeInformation.Qualification(employeeId);
            return new JsonResult(Qualification);
        }

        [HttpGet]
        public async Task<IActionResult> SkillSet(int employeeId)
        {
            var SkillSetData = await _employeeInformation.SkillSets(employeeId);
            return new JsonResult(SkillSetData);
        }

        [HttpGet]
        public async Task<IActionResult> Documents(int employeeId)
        {
            var DocumentsData = await _employeeInformation.Documents(employeeId);
            return new JsonResult(DocumentsData);
        }
        [HttpGet]
        public async Task<IActionResult> Dependent(int employeeId)
        {
            var DependentData = await _employeeInformation.Dependent(employeeId);
            return new JsonResult(DependentData);
        }
        [HttpGet]
        public async Task<IActionResult> Certification(int employeeId)
        {
            var CertificationData = await _employeeInformation.Certification(employeeId);
            return new JsonResult(CertificationData);

        }
        [HttpGet]
        public async Task<IActionResult> DisciplinaryActions(int employeeId)
        {
            var DisciplinaryActionsData = await _employeeInformation.DisciplinaryActions(employeeId);
            return new JsonResult(DisciplinaryActionsData);

        }

        [HttpGet]
        public async Task<IActionResult> Letter(int employeeId)
        {
            var LetterData = await _employeeInformation.Letter(employeeId);
            return new JsonResult(LetterData);

        }

        [HttpGet]
        public async Task<IActionResult> Reference(int employeeId)
        {
            var ReferenceData = await _employeeInformation.Reference(employeeId);
            return new JsonResult(ReferenceData);

        }

        [HttpGet]
        public async Task<IActionResult> Professional(int employeeId)
        {
            var ProfessionalData = await _employeeInformation.Professional(employeeId);
            return new JsonResult(ProfessionalData);

        }

        [HttpGet]
        public async Task<IActionResult> Asset()
        {
            var AssetData = await _employeeInformation.Asset();
            return new JsonResult(AssetData);

        }

        [HttpGet]
        public async Task<IActionResult> AssetDetails(int employeeId)
        {
            var AssetDetailslData = await _employeeInformation.AssetDetails(employeeId);
            return new JsonResult(AssetDetailslData);

        }
        [HttpGet]
        public async Task<IActionResult> CurrencyDropdown_Professional()
        {
            var CurrencyDropdown_Professional = await _employeeInformation.CurrencyDropdown_Professional();
            return new JsonResult(CurrencyDropdown_Professional);
                                    
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateProfessionalData([FromBody] HrEmpProfdtlsApprlDto apprlDto)
        {
            var professionalData = await _employeeInformation.InsertOrUpdateProfessionalData(apprlDto);

            if (professionalData == null)
            {
                return NotFound();
            }

            return Ok(professionalData);

        }

        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePersonalData([FromBody] PersonalDetailsHistoryDto apprlDto)
        {
            var PersonalData = await _employeeInformation.InsertOrUpdatePersonalData(apprlDto);

            if (PersonalData == null)
            {
                return NotFound();
            }

            return Ok(PersonalData);

        }
        [HttpGet]
        public async Task<IActionResult> GetProfessionalDataById(string updateType, int detailID, int empID)
        {
            var ProfessionalData = await _employeeInformation.GetProfessionalByIdAsync(updateType, detailID, empID);
            return new JsonResult(ProfessionalData);
        }


    }
}
