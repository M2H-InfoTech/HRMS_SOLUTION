using EMPLOYEE_INFORMATION.Helpers;
using EMPLOYEE_INFORMATION.Models.EnumFolder;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Repository.Common;
using HRMS.EmployeeInformation.Service.Interface;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetActiveStatus(int employeeId, string parameterCode, string type)
        {
            var activeStatus = await _employeeInformation.EmployeeStatusAsync(employeeId, parameterCode, type);
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
            var languagedata = await _employeeInformation.LanguageSkillAsync(employeeId);
            return new JsonResult(languagedata);
        }

        [HttpGet]
        public async Task<IActionResult> Communication(int employeeId)
        {
            var Communicationdata = await _employeeInformation.CommunicationAsync(employeeId);
            return new JsonResult(Communicationdata);
        }
        [HttpGet]
        public async Task<IActionResult> CommunicationExtra(int employeeId)
        {
            var CommunicationExtra = await _employeeInformation.CommunicationExtraAsync(employeeId);
            return new JsonResult(CommunicationExtra);
        }

        [HttpGet]

        public async Task<IActionResult> CommunicationEmergency(int employeeId)
        {
            var CommunicationEmergency = await _employeeInformation.CommunicationEmergencyAsync(employeeId);
            return new JsonResult(CommunicationEmergency);
        }

        [HttpGet]
        public async Task<IActionResult> HobbiesData(int employeeId)
        {
            var VisaDetailsReport = await _employeeInformation.HobbiesDataAsync(employeeId);
            return new JsonResult(VisaDetailsReport);
        }

        [HttpGet]
        public async Task<IActionResult> RewardAndRecognition(int employeeId)
        {
            var RewardAndRecognitionData = await _employeeInformation.RewardAndRecognitionAsync(employeeId);
            return new JsonResult(RewardAndRecognitionData);
        }

        [HttpGet]
        public async Task<IActionResult> Qualification(int employeeId)
        {
            var Qualification = await _employeeInformation.QualificationAsync(employeeId);
            return new JsonResult(Qualification);
        }

        [HttpGet]
        public async Task<IActionResult> SkillSet(int employeeId)
        {
            var SkillSetData = await _employeeInformation.SkillSetsAsync(employeeId);
            return new JsonResult(SkillSetData);
        }

        [HttpGet]
        public async Task<IActionResult> Documents(int employeeId)
        {
            List<string> excludedDocTypes = new List<string> { "Statutory", "BANK DETAILS", "VISA" };
            var DocumentsData = await _employeeInformation.DocumentsAsync(employeeId, excludedDocTypes);
            return new JsonResult(DocumentsData);
        }
        [HttpGet]

        public async Task<IActionResult> BankDetails(int employeeId)
        {
            List<string> excludedDocTypes = new List<string> { "Passport", "VISA", "Normal", "Statutory" };
            var DocumentsData = await _employeeInformation.DocumentsAsync(employeeId, excludedDocTypes);
            return new JsonResult(DocumentsData);
        }
        [HttpGet]
        public async Task<IActionResult> Dependent(int employeeId)
        {
            var DependentData = await _employeeInformation.DependentAsync(employeeId);
            return new JsonResult(DependentData);
        }
        [HttpGet]
        public async Task<IActionResult> Certification(int employeeId)
        {
            var CertificationData = await _employeeInformation.CertificationAsync(employeeId);
            return new JsonResult(CertificationData);

        }
        [HttpGet]
        public async Task<IActionResult> DisciplinaryActions(int employeeId)
        {
            var DisciplinaryActionsData = await _employeeInformation.DisciplinaryActionsAsync(employeeId);
            return new JsonResult(DisciplinaryActionsData);

        }

        [HttpGet]
        public async Task<IActionResult> Letter(int employeeId)
        {
            var LetterData = await _employeeInformation.LetterAsync(employeeId);
            return new JsonResult(LetterData);

        }

        [HttpGet]
        public async Task<IActionResult> Reference(int employeeId)
        {
            var ReferenceData = await _employeeInformation.ReferenceAsync(employeeId);
            return new JsonResult(ReferenceData);

        }

        [HttpGet]
        public async Task<IActionResult> Professional(int employeeId)
        {
            var ProfessionalData = await _employeeInformation.ProfessionalAsync(employeeId);
            return new JsonResult(ProfessionalData);

        }

        [HttpGet]
        public async Task<IActionResult> Asset()
        {
            var AssetData = await _employeeInformation.AssetAsync();
            return new JsonResult(AssetData);

        }

        [HttpGet]
        public async Task<IActionResult> AssetDetails(int employeeId)
        {
            var AssetDetailslData = await _employeeInformation.AssetDetailsAsync(employeeId);
            return new JsonResult(AssetDetailslData);

        }
        [HttpGet]
        public async Task<IActionResult> CurrencyDropdown_Professional()
        {
            var CurrencyDropdown_Professional = await _employeeInformation.CurrencyDropdownProfessionalAsync();
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
        [HttpGet]
        public async Task<IActionResult> GetPersonalDetailsById(int employeeId)
        {
            var personalDetails = await _employeeInformation.GetPersonalDetailsByIdAsync(employeeId);
            return new JsonResult(personalDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Training(int employeeId)
        {
            var Training = await _employeeInformation.TrainingAsync(employeeId);
            return new JsonResult(Training);
        }
        [HttpGet]
        public async Task<IActionResult> CareerHistory(int employeeId)
        {
            var CareerDetails = await _employeeInformation.CareerHistoryAsync(employeeId);
            return new JsonResult(CareerDetails);
        }
        [HttpGet]
        public async Task<IActionResult> BiometricDetails(int employeeId)
        {
            var BioDetails = await _employeeInformation.BiometricDetailsAsync(employeeId);
            return new JsonResult(BioDetails);
        }
        [HttpGet]
        public async Task<IActionResult> AuditInformation(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto)
        {
            var BioDetails = await _employeeInformation.AuditInformationAsync(employeeIDs, empId, roleId, infotype, infoDesc, datefrom, dateto);
            return new JsonResult(BioDetails);
        }

        [HttpGet]
        public async Task<IActionResult> AccessDetails(int employeeId)
        {
            var AccessDetails = await _employeeInformation.AccessDetailsAsync(employeeId);
            return new JsonResult(AccessDetails);
        }
        [HttpGet]
        public async Task<IActionResult> Fill_ModulesWorkFlow(int entityID, int linkId)
        {
            var fill_ModulesWorkFlow = await _employeeInformation.FillModulesWorkFlowAsync(entityID, linkId);
            return new JsonResult(fill_ModulesWorkFlow);
        }
        [HttpGet]
        public async Task<IActionResult> Fill_WorkFlowMaster(int emp_Id, int roleId)
        {
            var fill_WorkFlowMaster = await _employeeInformation.FillWorkFlowMasterAsync(emp_Id, roleId);
            return new JsonResult(fill_WorkFlowMaster);
        }
        [HttpGet]
        public async Task<IActionResult> BindWorkFlowMasterEmp(int linkId, int linkLevel)
        {
            var bindWorkFlowMasterEmp = await _employeeInformation.BindWorkFlowMasterEmpAsync(linkId, linkLevel);
            return new JsonResult(bindWorkFlowMasterEmp);
        }

        [HttpGet]
        public async Task<IActionResult> TransferAndPromotion(int employeeId)
        {
            var TransferAndPromotion = await _employeeInformation.TransferAndPromotionAsync(employeeId);
            return new JsonResult(TransferAndPromotion);
        }
        [HttpGet]
        public async Task<IActionResult> SalarySeries(int employeeId, string status)
        {
            var SalarySeries = await _employeeInformation.SalarySeriesAsync(employeeId, status);
            return new JsonResult(SalarySeries);
        }



        [HttpGet]
        public async Task<IActionResult> GetRejoinReport(int employeeId)
        {
            var getRejoinReport = await _employeeInformation.GetRejoinReportAsync(employeeId);
            return new JsonResult(getRejoinReport);
        }
        [HttpGet]
        public async Task<IActionResult> GetEmpReportingReport(int employeeId)
        {
            var getEmpReportingReport = await _employeeInformation.GetEmpReportingReportAsync(employeeId);
            return new JsonResult(getEmpReportingReport);
        }
        [HttpGet]
        public async Task<IActionResult> GetEmpWorkFlowRoleDetails(int linkId, int linkLevel)
        {
            var getEmpWorkFlowRoleDetails = await _employeeInformation.GetEmpWorkFlowRoleDetailsAsync(linkId, linkLevel);
            return new JsonResult(getEmpWorkFlowRoleDetails);
        }

        [HttpGet]
        public async Task<IActionResult> FillEmpWorkFlowRole(int entityID)
        {
            var fillEmpWorkFlowRole = await _employeeInformation.FillEmpWorkFlowRoleAsync(entityID);
            return new JsonResult(fillEmpWorkFlowRole);
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeType(int employeeId)
        {
            var employeeType = await _employeeInformation.EmployeeTypeAsync(employeeId);
            return new JsonResult(employeeType);
        }
        [HttpGet]
        public async Task<IActionResult> GeoSpacingTypeAndCriteria(string type)
        {
            var GeoSpacingType = await _employeeInformation.GeoSpacingTypeAndCriteriaAsync(type);
            return new JsonResult(GeoSpacingType);
        }
        [HttpGet]
        public async Task<IActionResult> GetGeoSpacing(int employeeid)
        {
            var GetGeoSpacing = await _employeeInformation.GetGeoSpacingAsync(employeeid);
            return new JsonResult(GetGeoSpacing);
        }
        //  [HttpGet]
        //public async Task<IActionResult> FillEmployeesBasedOnwWorkflow (int firstEntityId)
        //    {
        //    var fillEmployeesBasedOnwWorkflow = await _employeeInformation.FillEmployeesBasedOnwWorkflow (firstEntityId);
        //    return new JsonResult (fillEmployeesBasedOnwWorkflow);
        //    }
        [HttpGet]
        public async Task<IActionResult> HraDetails(int employeeId)
        {
            var HraDetails = await _employeeInformation.HraDetailsAsync(employeeId);
            return new JsonResult(HraDetails);
        }
        [HttpGet]
        public async Task<IActionResult> FillEmployeesBasedOnwWorkflow(int firstEntityId, int secondEntityId)
        {
            var fillEmployeesBasedOnwWorkflow = await _employeeInformation.FillEmployeesBasedOnwWorkflowAsync(firstEntityId, secondEntityId);
            return new JsonResult(fillEmployeesBasedOnwWorkflow);
        }
        [HttpGet]
        public async Task<IActionResult> GetCountry ( )
            {
            var fillCountry = await _employeeInformation.GetCountry ( );
            return new JsonResult (fillCountry);

            }
        [HttpGet]
        public async Task<IActionResult> GetNationalities ( )
            {
            var fillNatinalities = await _employeeInformation.GetNationalities ( );
            return new JsonResult (fillNatinalities);

            }
        [HttpGet]
        public async Task<IActionResult> GetBloodGroup ( )
            {
            var getBloodGroup = await _employeeInformation.GetBloodGroup ( );
            return new JsonResult (getBloodGroup);

            }
        [HttpGet]
        public async Task<IActionResult> FillReligion ( )
            {
            var fillReligion = await _employeeInformation.FillReligion ( );
            return new JsonResult (fillReligion);

            }

        [HttpPost]

        public async Task<IActionResult> InsertOrUpdateLanguageSkills([FromBody] LanguageSkillsSaveDto langSkills)
        {
            var languageSkills = await _employeeInformation.InsertOrUpdateLanguageSkills(langSkills);

            if (languageSkills == null || !languageSkills.Any()) // Checking for null or empty list
            {
                return NotFound();
            }

            return Ok(languageSkills);
        }

    }
}
