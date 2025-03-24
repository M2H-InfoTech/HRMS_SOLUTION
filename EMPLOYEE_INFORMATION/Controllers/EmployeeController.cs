using EMPLOYEE_INFORMATION.Helpers;
using EMPLOYEE_INFORMATION.Models.EnumFolder;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Repository.Common;
using HRMS.EmployeeInformation.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace EMPLOYEE_INFORMATION.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class EmployeeController : Controller
    {
        private readonly IEmployeeInformationService _employeeInformation;
        private readonly EmployeeSettings _employeeSettings;

        private readonly TokenService _tokenService;
        public EmployeeController(IEmployeeInformationService employeeInformation, TokenService tokenService, IOptions<EmployeeSettings> employeeSettings)
        {
            _employeeInformation = employeeInformation;
            _tokenService = tokenService;
            _employeeSettings = employeeSettings.Value;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var token = _tokenService.GenerateToken("2311427", "Admin");
            return Ok(token);
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
            return Ok(activeStatus);
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
            return Ok(languagedata);
        }

        [HttpGet]
        public async Task<IActionResult> Communication(int employeeId)
        {
            var Communicationdata = await _employeeInformation.CommunicationAsync(employeeId);
            return Ok(Communicationdata);
        }
        [HttpGet]
        public async Task<IActionResult> CommunicationExtra(int employeeId)
        {
            var CommunicationExtra = await _employeeInformation.CommunicationExtraAsync(employeeId);
            return Ok(CommunicationExtra);
        }

        [HttpGet]

        public async Task<IActionResult> CommunicationEmergency(int employeeId)
        {
            var CommunicationEmergency = await _employeeInformation.CommunicationEmergencyAsync(employeeId);
            return Ok(CommunicationEmergency);
        }

        [HttpGet]
        public async Task<IActionResult> HobbiesData(int employeeId)
        {
            var VisaDetailsReport = await _employeeInformation.HobbiesDataAsync(employeeId);
            return Ok(VisaDetailsReport);
        }

        [HttpGet]
        public async Task<IActionResult> RewardAndRecognition(int employeeId)
        {
            var RewardAndRecognitionData = await _employeeInformation.RewardAndRecognitionAsync(employeeId);
            return Ok(RewardAndRecognitionData);
        }

        [HttpGet]
        public async Task<IActionResult> Qualification(int employeeId)
        {
            var Qualification = await _employeeInformation.QualificationAsync(employeeId);
            return Ok(Qualification);
        }

        [HttpGet]
        public async Task<IActionResult> SkillSet(int employeeId)
        {
            var SkillSetData = await _employeeInformation.SkillSetsAsync(employeeId);
            return Ok(SkillSetData);
        }

        [HttpGet]
        public async Task<IActionResult> Documents(int employeeId)
        {
            List<string> excludedDocTypes = new List<string> { _employeeSettings.Documents01, _employeeSettings.Documents02, _employeeSettings.Documents03 };
            var DocumentsData = await _employeeInformation.DocumentsAsync(employeeId, excludedDocTypes);
            return Ok(DocumentsData);
        }
        [HttpGet]

        public async Task<IActionResult> BankDetails(int employeeId)
        {
            List<string> excludedDocTypes = new List<string> { _employeeSettings.Documents04, _employeeSettings.Documents03, _employeeSettings.Documents05, _employeeSettings.Documents01 };
            var DocumentsData = await _employeeInformation.DocumentsAsync(employeeId, excludedDocTypes);
            return Ok(DocumentsData);
        }
        [HttpGet]
        public async Task<IActionResult> Dependent(int employeeId)
        {
            var DependentData = await _employeeInformation.DependentAsync(employeeId);
            return Ok(DependentData);
        }
        [HttpGet]
        public async Task<IActionResult> Certification(int employeeId)
        {
            var CertificationData = await _employeeInformation.CertificationAsync(employeeId);
            return Ok(CertificationData);

        }
        [HttpGet]
        public async Task<IActionResult> DisciplinaryActions(int employeeId)
        {
            var DisciplinaryActionsData = await _employeeInformation.DisciplinaryActionsAsync(employeeId);
            return Ok(DisciplinaryActionsData);

        }

        [HttpGet]
        public async Task<IActionResult> Letter(int employeeId)
        {
            var LetterData = await _employeeInformation.LetterAsync(employeeId);
            return Ok(LetterData);

        }

        [HttpGet]
        public async Task<IActionResult> Reference(int employeeId)
        {
            var ReferenceData = await _employeeInformation.ReferenceAsync(employeeId);
            return Ok(ReferenceData);

        }

        [HttpGet]
        public async Task<IActionResult> Professional(int employeeId)
        {
            var ProfessionalData = await _employeeInformation.ProfessionalAsync(employeeId);
            return Ok(ProfessionalData);

        }

        [HttpGet]
        public async Task<IActionResult> Asset()
        {
            var AssetData = await _employeeInformation.AssetAsync();
            return Ok(AssetData);

        }

        [HttpGet]
        public async Task<IActionResult> AssetDetails(int employeeId)
        {
            var AssetDetailslData = await _employeeInformation.AssetDetailsAsync(employeeId);
            return Ok(AssetDetailslData);

        }
        [HttpGet]
        public async Task<IActionResult> CurrencyDropdown_Professional()
        {
            var CurrencyDropdown_Professional = await _employeeInformation.CurrencyDropdownProfessionalAsync();
            return Ok(CurrencyDropdown_Professional);

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
        [HttpGet]
        public async Task<IActionResult> GetProfessionalDataById(string updateType, int detailID, int empID)
        {
            var ProfessionalData = await _employeeInformation.GetProfessionalByIdAsync(updateType, detailID, empID);
            return Ok(ProfessionalData);
        }
        [HttpGet]
        public async Task<IActionResult> GetPersonalDetailsById(int employeeId)
        {
            var personalDetails = await _employeeInformation.GetPersonalDetailsByIdAsync(employeeId);
            return Ok(personalDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Training(int employeeId)
        {
            var Training = await _employeeInformation.TrainingAsync(employeeId);
            return Ok(Training);
        }
        [HttpGet]
        public async Task<IActionResult> CareerHistory(int employeeId)
        {
            var CareerDetails = await _employeeInformation.CareerHistoryAsync(employeeId);
            return Ok(CareerDetails);
        }
        [HttpGet]
        public async Task<IActionResult> BiometricDetails(int employeeId)
        {
            var BioDetails = await _employeeInformation.BiometricDetailsAsync(employeeId);
            return Ok(BioDetails);
        }
        [HttpGet]
        public async Task<IActionResult> AuditInformation(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto)
        {
            var BioDetails = await _employeeInformation.AuditInformationAsync(employeeIDs, empId, roleId, infotype, infoDesc, datefrom, dateto);
            return Ok(BioDetails);
        }

        [HttpGet]
        public async Task<IActionResult> AccessDetails(int employeeId)
        {
            var AccessDetails = await _employeeInformation.AccessDetailsAsync(employeeId);
            return Ok(AccessDetails);
        }
        [HttpGet]
        public async Task<IActionResult> Fill_ModulesWorkFlow(int entityID, int linkId)
        {
            var fill_ModulesWorkFlow = await _employeeInformation.FillModulesWorkFlowAsync(entityID, linkId);
            return Ok(fill_ModulesWorkFlow);
        }
        [HttpGet]
        public async Task<IActionResult> Fill_WorkFlowMaster(int emp_Id, int roleId)
        {
            var fill_WorkFlowMaster = await _employeeInformation.FillWorkFlowMasterAsync(emp_Id, roleId);
            return Ok(fill_WorkFlowMaster);
        }
        [HttpGet]
        public async Task<IActionResult> BindWorkFlowMasterEmp(int linkId, int linkLevel)
        {
            var bindWorkFlowMasterEmp = await _employeeInformation.BindWorkFlowMasterEmpAsync(linkId, linkLevel);
            return Ok(bindWorkFlowMasterEmp);
        }

        [HttpGet]
        public async Task<IActionResult> TransferAndPromotion(int employeeId)
        {
            var TransferAndPromotion = await _employeeInformation.TransferAndPromotionAsync(employeeId);
            return Ok(TransferAndPromotion);
        }
        [HttpGet]
        public async Task<IActionResult> SalarySeries(int employeeId, string status)
        {
            var SalarySeries = await _employeeInformation.SalarySeriesAsync(employeeId, status);
            return Ok(SalarySeries);
        }



        [HttpGet]
        public async Task<IActionResult> GetRejoinReport(int employeeId)
        {
            var getRejoinReport = await _employeeInformation.GetRejoinReportAsync(employeeId);
            return Ok(getRejoinReport);
        }
        [HttpGet]
        public async Task<IActionResult> GetEmpReportingReport(int employeeId)
        {
            var getEmpReportingReport = await _employeeInformation.GetEmpReportingReportAsync(employeeId);
            return Ok(getEmpReportingReport);
        }
        [HttpGet]
        public async Task<IActionResult> GetEmpWorkFlowRoleDetails(int linkId, int linkLevel)
        {
            var getEmpWorkFlowRoleDetails = await _employeeInformation.GetEmpWorkFlowRoleDetailsAsync(linkId, linkLevel);
            return Ok(getEmpWorkFlowRoleDetails);
        }

        [HttpGet]
        public async Task<IActionResult> FillEmpWorkFlowRole(int entityID)
        {
            var fillEmpWorkFlowRole = await _employeeInformation.FillEmpWorkFlowRoleAsync(entityID);
            return Ok(fillEmpWorkFlowRole);
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeType(int employeeId)
        {
            var employeeType = await _employeeInformation.EmployeeTypeAsync(employeeId);
            return Ok(employeeType);
        }
        [HttpGet]
        public async Task<IActionResult> GeoSpacingTypeAndCriteria(string type)
        {
            var GeoSpacingType = await _employeeInformation.GeoSpacingTypeAndCriteriaAsync(type);
            return Ok(GeoSpacingType);
        }
        [HttpGet]
        public async Task<IActionResult> GetGeoSpacing(int employeeid)
        {
            var GetGeoSpacing = await _employeeInformation.GetGeoSpacingAsync(employeeid);
            return Ok(GetGeoSpacing);
        }
        [HttpGet]
        public async Task<IActionResult> HraDetails(int employeeId)
        {
            var HraDetails = await _employeeInformation.HraDetailsAsync(employeeId);
            return Ok(HraDetails);
        }
        [HttpGet]
        public async Task<IActionResult> FillEmployeesBasedOnwWorkflow(int firstEntityId, int secondEntityId)
        {
            var fillEmployeesBasedOnwWorkflow = await _employeeInformation.FillEmployeesBasedOnwWorkflowAsync(firstEntityId, secondEntityId);
            return Ok(fillEmployeesBasedOnwWorkflow);
        }
        [HttpGet]
        public async Task<IActionResult> GetCountry()
        {
            var fillCountry = await _employeeInformation.GetCountryAsync();
            return Ok(fillCountry);

        }
        [HttpGet]
        public async Task<IActionResult> GetNationalities()
        {
            var fillNatinalities = await _employeeInformation.GetNationalitiesAsync();
            return Ok(fillNatinalities);

        }
        [HttpGet]
        public async Task<IActionResult> GetBloodGroup()
        {
            var getBloodGroup = await _employeeInformation.GetBloodGroupAsync();
            return Ok(getBloodGroup);

        }
        [HttpGet]
        public async Task<IActionResult> FillReligion()
        {
            var fillReligion = await _employeeInformation.FillReligionAsync();
            return Ok(fillReligion);

        }
        [HttpGet]
        public async Task<IActionResult> GetHrEmpDetailsAsync(int employeeId, int roleId)
        {
            var employeeDetails = await _employeeInformation.GetHrEmpDetailsAsync(employeeId, roleId);
            return Ok(employeeDetails);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmployeeDetails([FromBody] EmployeeDetailsUpdateDto employeeDetailsDto)
        {
            var employeeDetails = await _employeeInformation.UpdateEmployeeDetailsAsync(employeeDetailsDto);
            return Ok(employeeDetails);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLanguageSkills([FromBody] LanguageSkillsSaveDto langSkills)
        {
            var languageSkills = await _employeeInformation.InsertOrUpdateLanguageSkillsAsync(langSkills);

            if (languageSkills == null || !languageSkills.Any()) // Checking for null or empty list
            {
                return NotFound();
            }

            return Ok(languageSkills);
        }
        [HttpGet]
        public async Task<IActionResult> FillLanguageTypes()
        {
            var fillLanguageTypes = await _employeeInformation.FillLanguageTypesAsync();
            return Ok(fillLanguageTypes);

        }
        [HttpGet]
        public async Task<IActionResult> FillConsultant()
        {
            var fillConsultant = await _employeeInformation.FillConsultantAsync();
            return Ok(fillConsultant);

        }

        [HttpPost]

        public async Task<IActionResult> InsertOrUpdateReference([FromBody] ReferenceSaveDto Reference)
        {
            var References = await _employeeInformation.InsertOrUpdateReferenceAsync(Reference);

            if (References == null || !References.Any()) // Checking for null or empty list
            {
                return NotFound();
            }

            return Ok(References);
        }

        [HttpGet]
        public async Task<IActionResult> FillRewardType()
        {
            var fillRewardType = await _employeeInformation.FillRewardTypeAsync();
            return Ok(fillRewardType);

        }

        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateEmpRewards([FromBody] EmpRewardsSaveDto EmpRewards)
        {
            var EmpReward = await _employeeInformation.InsertOrUpdateEmpRewardsAsync(EmpRewards);

            if (EmpReward == null || !EmpReward.Any()) // Checking for null or empty list
            {
                return NotFound();
            }

            return Ok(EmpReward);
        }
        [HttpGet]
        public async Task<IActionResult> FillBankDetails(int empID)
        {
            var bankDetails = await _employeeInformation.FillBankDetailsAsync(empID);
            return Ok(bankDetails);

        }

        [HttpGet]
        public async Task<IActionResult> BankTypeEdit()
        {
            var BankType = await _employeeInformation.BankTypeEditAsync();
            return Ok(BankType);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmployeeType([FromBody] EmployeeTypeDto EmployeeType)
        {
            var employeeTypes = await _employeeInformation.UpdateEmployeeTypeAsync(EmployeeType);

            if (employeeTypes == null || !employeeTypes.Any()) // Checking for null or empty list
            {
                return NotFound();
            }

            return Ok(employeeTypes);
        }

        [HttpGet]
        public async Task<IActionResult> CertificationsDropdown(string description)
        {
            var certificationDropdown = await _employeeInformation.CertificationsDropdownAsync(description);
            return Ok(certificationDropdown);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateCertificates([FromBody] CertificationSaveDto certificates)
        {
            var empCertificate = await _employeeInformation.InsertOrUpdateCertificatesAsync(certificates);

            if (empCertificate == null || !empCertificate.Any())
            {
                return NotFound();
            }

            return Ok(empCertificate);
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateSkill([FromBody] SaveSkillSetDto skillset)
        {
            var empskilll = await _employeeInformation.InsertOrUpdateSkillAsync(skillset);

            if (empskilll == null || !empskilll.Any())
            {
                return NotFound();
            }

            return Ok(empskilll);
        }

        //[HttpPost("uploadDocument")]
        //public async Task<IActionResult> UploadQualificationDocument(List<IFormFile> files, [FromForm] QualificationAttachmentDto skillset)
        //{
        //    if (files == null || files.Count == 0)
        //        return BadRequest("No files uploaded.");

        //    var result = await _employeeInformation.UploadEmployeeDocumentsAsync(files, skillset);

        //    if (string.IsNullOrEmpty(result))
        //        return StatusCode(500, "Error while uploading files.");

        //    return Ok(new { message = "Files uploaded successfully", result });
        //}

        [HttpPost]
        public async Task<IActionResult> InsertQualification([FromBody] QualificationTableDto Qualification, string FirstEntityID, int EmpEntityIds)
        {
            var empqualification = await _employeeInformation.InsertQualificationAsync(Qualification, FirstEntityID, EmpEntityIds);



            return Ok(empqualification);
        }

        [HttpGet]
        public async Task<IActionResult> FillCountry()
        {
            var fillCountry = await _employeeInformation.FillCountryAsync();
            return Ok(fillCountry);

        }



        [HttpGet]
        public async Task<IActionResult> FillEmployeeDropdown(string activeStatus, string employeeStatus, string probationStatus)
        {
            var fillEmp = await _employeeInformation.FillEmployeeDropdownAsync(activeStatus, employeeStatus, probationStatus);
            return Ok(fillEmp);
        }
        [HttpGet]
        public async Task<IActionResult> AssetGroupDropdownEdit()
        {
            var assetgroup = await _employeeInformation.AssetGroupDropdownEditAsync();
            return Ok(assetgroup);
        }
        [HttpGet]
        public async Task<IActionResult> GetAssetDropdownEdit(int varAssestTypeID)
        {
            var assetdetails = await _employeeInformation.GetAssetDropdownEditAsync(varAssestTypeID);
            return Ok(assetdetails);
        }
        [HttpGet]
        public async Task<IActionResult> GetAssetDetailsEdit(string CommonName)
        {
            var assetdetailsno = await _employeeInformation.GetAssetDetailsEditAsync(CommonName);
            return Ok(assetdetailsno);
        }

        [HttpPost]
        public async Task<IActionResult> AssetEdit([FromBody] AssetEditDto assetEdits)
        {
            var assetedit = await _employeeInformation.AssetEditAsync(assetEdits);

            if (assetedit == null || !assetedit.Any())
            {
                return NotFound();
            }

            return Ok(assetedit);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePersonalDetails([FromBody] PersonalDetailsUpdateDto personalDetailsDto)
        {
            var personalDetails = await _employeeInformation.UpdatePersonalDetailsAsync(personalDetailsDto);
            return Ok(personalDetails);
        }
        [HttpGet]
        public async Task<IActionResult> GetAssetEditDatas(int varSelectedTypeID, int varAssestID)
        {
            var getassetedit = await _employeeInformation.GetAssetEditDatasAsync(varSelectedTypeID, varAssestID);
            return Ok(getassetedit);
        }

        [HttpDelete]
        public async Task<IActionResult> AssetDelete(int varEmpID, int varAssestID)
        {
            var getassetdelete = await _employeeInformation.AssetDeleteAsync(varEmpID, varAssestID);
            return Ok(getassetdelete);
        }
        [HttpGet]
        public async Task<IActionResult> GetBankType(int employeeId)
        {
            var getBankType = await _employeeInformation.GetBankTypeAsync(employeeId);
            return Ok(getBankType);
        }
        [HttpGet]
        public async Task<IActionResult> GetGeneralSubCategoryList(string remarks)
        {
            var getGeneralSubCategoryList = await _employeeInformation.GetGeneralSubCategoryListAsync(remarks);
            return Ok(getGeneralSubCategoryList);
        }
        [HttpPost]
        public async Task<IActionResult> SetEmpDocumentDetails([FromBody] SetEmpDocumentDetailsDto SetEmpDocumentDetails)   // For Document and Bank Insertion
        {
            var setEmpDocumentDetails = await _employeeInformation.SetEmpDocumentDetailsAsync(SetEmpDocumentDetails);
            return Ok(setEmpDocumentDetails);
        }
        [HttpGet]
        public async Task<IActionResult> FillDocumentType(int EmpID)    //dropdown in document add button
        {
            var FillDocumentType = await _employeeInformation.FillDocumentTypeAsync(EmpID);
            return Ok(FillDocumentType);
        }
        [HttpGet]
        public async Task<IActionResult> DocumentField(int DocumentID)   //textbox field name inside document add button
        {
            var DocumentField = await _employeeInformation.DocumentFieldAsync(DocumentID);
            return Ok(DocumentField);
        }


        [HttpPost]
        public async Task<IActionResult> InsertDocumentsFieldDetails([FromBody] List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy)   //InsertOrUpdate document & bank
        {
            var FieldDetails = await _employeeInformation.InsertDocumentsFieldDetailsAsync(DocumentBankField, DocumentID, In_EntryBy);

            if (FieldDetails == null || !FieldDetails.Any())
            {
                return NotFound();
            }

            return Ok(FieldDetails);
        }

        [HttpPost]
        public async Task<IActionResult> SetEmpDocuments([FromBody] TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy)   //InsertOrUpdate document & bank upload file
        {
            var SetEmpDocuments = await _employeeInformation.SetEmpDocumentsAsync(DocumentBankField, DetailID, Status, In_EntryBy);

            if (SetEmpDocuments == null || !SetEmpDocuments.Any())
            {
                return NotFound();
            }

            return Ok(SetEmpDocuments);
        }
        [HttpPost]
        public async Task<IActionResult> InsertLetterTypeRequest([FromBody] LetterInsertUpdateDto LetterInsertUpdateDtos)
        {
            var InsertLetterTypeRequest = await _employeeInformation.InsertLetterTypeRequestAsync(LetterInsertUpdateDtos);
            return Ok(InsertLetterTypeRequest);
        }
        [HttpGet]
        public async Task<IActionResult> EditEmployeeCommonInformationAsync(string? empIds, int? employeeid)
        {
            var employeeType = await _employeeInformation.EditEmployeeCommonInformationAsync(empIds, employeeid);
            return Ok(employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployeeInfo(List<TmpEmpInformation> inputs)
        {
            var employeeType = await _employeeInformation.EditInformationAsync(inputs);
            return Ok(employeeType);
        }
        [HttpGet]
        public async Task<IActionResult> GetInformationDescriptionAsync(int infoId)
        {
            var employeeType = await _employeeInformation.GetInformationDescriptionAsync(infoId);
            return Ok(employeeType);
        }
        [HttpGet]
        public async Task<IActionResult> GetLetterTypes()
        {
            var employeeType = await _employeeInformation.GetLetterTypeAsync();
            return Ok(employeeType);
        }
        [HttpGet]
        public async Task<IActionResult> LetterSignatureAuthority()
        {
            var employeeType = await _employeeInformation.LetterSignatureAuthorityAsync();
            return Ok(employeeType);
        }
        [HttpPost]
        public async Task<IActionResult> LoadCompanyDetailsAsync(LoadCompanyDetailsRequestDto loadCompanyDetailsRequestDto)
        {
            var employeeType = await _employeeInformation.LoadCompanyDetailsAsync(loadCompanyDetailsRequestDto);
            return Ok(employeeType);
        }
        [HttpGet]
        public async Task<IActionResult> GetLevelAsync(int level)
        {
            var employeeType = await _employeeInformation.GetLevelAsync(level);
            return Ok(employeeType);
        }
        [HttpPost]
        public async Task<IActionResult> DirectUploadLetter([FromForm] List<IFormFile> files, string filepath, int masterID)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");
            var result = await _employeeInformation.DirectUploadLetter(files, filepath, masterID);
            if (result == "")
                return StatusCode(500, "Error while uploading files.");
            return Ok(new { message = "Files uploaded successfully", result });
        }
        [HttpPost]
        public async Task<IActionResult> UploadOrUpdateEmployeeDocuments([FromForm] List<IFormFile> files, string filePath, [FromForm] QualificationAttachmentDto attachmentDto)
        {
            var result = await _employeeInformation.UploadOrUpdateEmployeeDocuments(files, filePath, attachmentDto);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> CheckLetterTypeRequest(int? LetterTypeId, int? LetterSubType, int? MasterId)
        {
            var LetterTypeRequest = await _employeeInformation.CheckLetterTypeRequest(LetterTypeId, LetterSubType, MasterId);
            return Ok(LetterTypeRequest);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDesciplinaryLetter(string? masterId)
        {
            var LetterTypeRequest = await _employeeInformation.DeleteDesciplinaryLetter(masterId);
            return Ok(LetterTypeRequest);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLetterType()
        {
            var allLetterTypes = await _employeeInformation.GetAllLetterType();
            return Ok(allLetterTypes);
        }
        [HttpGet]
        public async Task<IActionResult> GetLetterSubTypeByIdAsync(int LetterSubTypeID)
        {
            var letterType = await _employeeInformation.GetLetterSubTypeByIdAsync(LetterSubTypeID);
            return Ok(letterType);
        }
        [HttpGet]
        public async Task<IActionResult> GetLastEntityByEmployeeId(int empId)
        {
            var letterType = await _employeeInformation.GetLastEntityByEmployeeId(empId);
            return Ok(letterType);
        }
    }
}
