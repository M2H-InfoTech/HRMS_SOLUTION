using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using HRMS.EmployeeInformation.Repository.Common;
using HRMS.EmployeeInformation.Service.Interface;
using Microsoft.AspNetCore.Http;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Service.Service
{
    public class EmployeeInformationService : IEmployeeInformationService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeInformationService(IEmployeeRepository employeeRepository)
        {

            _employeeRepository = employeeRepository;
        }

        public async Task<CommunicationResultDto> CommunicationAsync(int employeeId)
        {
            return await _employeeRepository.CommunicationAsync(employeeId);
        }

        public async Task<List<CommunicationEmergencyDto>> CommunicationEmergencyAsync(int employeeId)
        {
            return await _employeeRepository.CommunicationEmergencyAsync(employeeId);
        }

        public async Task<List<CommunicationExtraDto>> CommunicationExtraAsync(int employeeId)
        {
            return await _employeeRepository.CommunicationExtraAsync(employeeId);
        }

        public async Task<EmployeeStatusResultDto> EmployeeStatusAsync(int employeeId, string parameterCode, string type)
        {
            return await _employeeRepository.EmployeeStatus(employeeId, parameterCode, type);
        }

        public async Task<PaginatedResult<EmployeeResultDto>> GetEmpData(EmployeeInformationParameters employeeInformationParameters)
        {
            return await _employeeRepository.GetEmpData(employeeInformationParameters);
        }

        public async Task<List<LanguageSkillResultDto>> LanguageSkillAsync(int employeeId)
        {
            return await _employeeRepository.LanguageSkillAsync(employeeId);
        }

        public async Task<List<string>> HobbiesDataAsync(int employeeId)
        {
            return await _employeeRepository.HobbiesDataAsync(employeeId);
        }

        public async Task<List<RewardAndRecognitionDto>> RewardAndRecognitionAsync(int employeeId)
        {
            return await _employeeRepository.RewardAndRecognitionsAsync(employeeId);
        }

        public async Task<List<QualificationDto>> QualificationAsync(int employeeId)
        {
            return await _employeeRepository.QualificationAsync(employeeId);
        }

        public async Task<List<SkillSetDto>> SkillSetsAsync(int employeeId)
        {
            return await _employeeRepository.SkillSetsAsync(employeeId);
        }
        public async Task<List<AllDocumentsDto>> DocumentsAsync(int employeeId, List<string> excludedDocTypes)
        {
            return await _employeeRepository.DocumentsAsync(employeeId, excludedDocTypes);
        }
        //public async Task<List<AllDocumentsDto>> BankDetails(int employeeId)
        //{
        //    return await _employeeRepository.BankDetails(employeeId);
        //}
        public async Task<List<DependentDto>> DependentAsync(int employeeId)
        {
            return await _employeeRepository.DependentAsync(employeeId);
        }

        public async Task<List<CertificationDto>> CertificationAsync(int employeeId)
        {
            return await _employeeRepository.CertificationAsync(employeeId);
        }
        public async Task<List<DisciplinaryActionsDto>> DisciplinaryActionsAsync(int employeeId)
        {
            return await _employeeRepository.DisciplinaryActionsAsync(employeeId);
        }

        public async Task<List<LetterDto>> LetterAsync(int employeeId)
        {
            return await _employeeRepository.LetterAsync(employeeId);
        }
        public async Task<List<ReferenceDto>> ReferenceAsync(int employeeId)
        {
            return await _employeeRepository.ReferenceAsync(employeeId);
        }
        public async Task<List<ProfessionalDto>> ProfessionalAsync(int employeeId)
        {
            return await _employeeRepository.ProfessionalAsync(employeeId);
        }
        public async Task<List<AssetDto>> AssetAsync()
        {
            return await _employeeRepository.AsseAsynct();
        }
        public async Task<List<AssetDetailsDto>> AssetDetailsAsync(int employeeId)
        {
            return await _employeeRepository.AssetDetailsAsync(employeeId);
        }

        public async Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdownProfessionalAsync()
        {
            return await _employeeRepository.CurrencyDropdownProfessionalAsync();
        }
        //public async Task<HrEmpProfdtlsApprlDto> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto)
        //{
        //    return await _employeeRepository.InsertOrUpdateProfessionalData(profdtlsApprlDto);
        //}
        public async Task<string?> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto)
        {
            return await _employeeRepository.InsertOrUpdateProfessionalData(profdtlsApprlDto);
        }
        public async Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID)
        {
            return await _employeeRepository.GetProfessionalByIdAsync(updateType, detailID, empID);
        }
        public async Task<List<PersonalDetailsDto>> GetPersonalDetailsByIdAsync(int employeeid)
        {
            return await _employeeRepository.GetPersonalDetailsByIdAsync(employeeid);
        }
        public async Task<List<TrainingDto>> TrainingAsync(int employeeid)
        {
            return await _employeeRepository.TrainingAsync(employeeid);
        }

        public async Task<List<CareerHistoryDto>> CareerHistoryAsync(int employeeid)
        {
            return await _employeeRepository.CareerHistoryAsync(employeeid);
        }
        public async Task<List<object>> BiometricDetailsAsync(int employeeid)
        {
            return await _employeeRepository.BiometricDetailsAsync(employeeid);
        }
        public async Task<object> AccessDetailsAsync(int employeeid)
        {
            return await _employeeRepository.AccessDetailsAsync(employeeid);
        }
        public async Task<List<Fill_ModulesWorkFlowDto>> FillModulesWorkFlowAsync(int entityID, int linkId)
        {
            return await _employeeRepository.FillModulesWorkFlowAsync(entityID, linkId);
        }
        public async Task<List<Fill_WorkFlowMasterDto>> FillWorkFlowMasterAsync(int emp_Id, int roleId)
        {
            return await _employeeRepository.FillWorkFlowMasterAsync(emp_Id, roleId);
        }
        public async Task<List<BindWorkFlowMasterEmpDto>> BindWorkFlowMasterEmpAsync(int linkId, int linkLevel)
        {
            return await _employeeRepository.BindWorkFlowMasterEmpAsync(linkId, linkLevel);
        }
        public async Task<List<GetRejoinReportDto>> GetRejoinReportAsync(int employeeId)
        {
            return await _employeeRepository.GetRejoinReportAsync(employeeId);
        }
        public async Task<List<GetEmpReportingReportDto>> GetEmpReportingReportAsync(int employeeId)
        {
            return await _employeeRepository.GetEmpReportingReportAsync(employeeId);
        }


        public async Task<List<TransferAndPromotionDto>> TransferAndPromotionAsync(int employeeid)
        {
            return await _employeeRepository.TransferAndPromotionAsync(employeeid);
        }
        public async Task<List<SalarySeriesDto>> SalarySeriesAsync(int employeeid, string status)
        {
            return await _employeeRepository.SalarySeriesAsync(employeeid, status);
        }
        public async Task<List<AuditInformationDto>> AuditInformationAsync(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto)
        {
            return await _employeeRepository.AuditInformationAsync(employeeIDs, empId, roleId, infotype, infoDesc, datefrom, dateto);
        }


        public async Task<List<GetEmpWorkFlowRoleDetailstDto>> GetEmpWorkFlowRoleDetailsAsync(int linkId, int linkLevel)
        {
            return await _employeeRepository.GetEmpWorkFlowRoleDetailsAsync(linkId, linkLevel);
        }

        public async Task<List<FillEmpWorkFlowRoleDto>> FillEmpWorkFlowRoleAsync(int entityID)
        {
            return await _employeeRepository.FillEmpWorkFlowRoleAsync(entityID);
        }
        public async Task<List<EmployeeHraDto>> HraDetailsAsync(int employeeId)
        {
            return await _employeeRepository.HraDetailsAsync(employeeId);
        }

        public async Task<List<object>> EmployeeTypeAsync(int employeeid)
        {
            return await _employeeRepository.EmployeeTypeAsync(employeeid);
        }

        public async Task<List<object>> GeoSpacingTypeAndCriteriaAsync(string type)
        {
            return await _employeeRepository.GeoSpacingTypeAndCriteriaAsync(type);
        }

        public async Task<List<GeoSpacingDto>> GetGeoSpacingAsync(int employeeid)
        {
            return await _employeeRepository.GetGeoSpacingAsync(employeeid);
        }
        public async Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmployeesBasedOnwWorkflowAsync(int firstEntityId, int secondEntityId)
        {
            return await _employeeRepository.FillEmployeesBasedOnwWorkflowAsync(firstEntityId, secondEntityId);
        }
        public async Task<List<object>> GetCountry()
        {
            return await _employeeRepository.GetCountry();
        }
        public async Task<List<object>> GetNationalities()
        {
            return await _employeeRepository.GetNationalities();
        }
        public async Task<List<object>> GetBloodGroup()
        {
            return await _employeeRepository.GetBloodGroup();
        }
        public async Task<List<object>> FillReligion()
        {
            return await _employeeRepository.FillReligion();
        }

        public async Task<string> InsertOrUpdateLanguageSkills(LanguageSkillsSaveDto langSkills)
        {
            return await _employeeRepository.InsertOrUpdateLanguageSkills(langSkills);
        }
        public async Task<List<object>> FillLanguageTypes()
        {
            return await _employeeRepository.FillLanguageTypes();
        }
        public async Task<List<object>> FillConsultant()
        {
            return await _employeeRepository.FillConsultant();
        }

        public async Task<string> InsertOrUpdateReference(ReferenceSaveDto Reference)
        {
            return await _employeeRepository.InsertOrUpdateReference(Reference);
        }
        public async Task<List<object>> FillRewardType()
        {
            return await _employeeRepository.FillRewardType();
        }
        public async Task<string> InsertOrUpdateEmpRewards(EmpRewardsSaveDto EmpRewards)
        {
            return await _employeeRepository.InsertOrUpdateEmpRewards(EmpRewards);
        }
        public async Task<List<object>> FillBankDetails(int empID)
        {
            return await _employeeRepository.FillBankDetails(empID);
        }
        public async Task<List<object>> BankTypeEdit()
        {
            return await _employeeRepository.BankTypeEdit();
        }

        public async Task<EmployeeDetailsDto> GetHrEmpDetailsAsync(int employeeId, int roleId)
        {
            return await _employeeRepository.GetHrEmpDetailsAsync(employeeId, roleId);
        }
        public async Task<List<object>> CertificationsDropdown(string description)
        {
            return await _employeeRepository.CertificationsDropdown(description);
        }
        public async Task<string> InsertOrUpdateCertificates(CertificationSaveDto certificates)
        {
            return await _employeeRepository.InsertOrUpdateCertificates(certificates);
        }

        public async Task<string> UpdateEmployeeType(EmployeeTypeDto EmployeeType)
        {
            return await _employeeRepository.UpdateEmployeeType(EmployeeType);
        }
        public async Task<string> InsertOrUpdateSkill(SaveSkillSetDto skillset)
        {
            return await _employeeRepository.InsertOrUpdateSkill(skillset);
        }
        public async Task<List<object>> FillEmployeeDropdown(string activeStatus, string employeeStatus, string probationStatus)
        {
            return await _employeeRepository.FillEmployeeDropdown(activeStatus, employeeStatus, probationStatus);
        }
        public async Task<List<object>> AssetGroupDropdownEdit()
        {
            return await _employeeRepository.AssetGroupDropdownEdit();
        }

        public async Task<List<object>> GetAssetDropdownEdit(int varAssestTypeID)
        {
            return await _employeeRepository.GetAssetDropdownEdit(varAssestTypeID);
        }



        public async Task<List<object>> GetAssetDetailsEdit(string CommonName)
        {
            return await _employeeRepository.GetAssetDetailsEdit(CommonName);
        }
        public async Task<string> AssetEdit(AssetEditDto assetEdits)
        {
            return await _employeeRepository.AssetEdit(assetEdits);
        }
        public async Task<List<object>> GetAssetEditDatas(int varSelectedTypeID, int varAssestID)
        {
            return await _employeeRepository.GetAssetEditDatas(varSelectedTypeID, varAssestID);
        }


        public async Task<string> AssetDelete(int varEmpID, int varAssestID)
        {
            return await _employeeRepository.AssetDelete(varEmpID, varAssestID);
        }
        //public async Task<EmployeeDetailsUpdateDto> UpdateEmployeeDetails(EmployeeDetailsUpdateDto employeeDetailsDto, int lastEntity)
        //{
        //    return await _employeeRepository.UpdateEmployeeDetails(employeeDetailsDto, lastEntity);
        //}
        public async Task<EmployeeDetailsUpdateDto> UpdateEmployeeDetails(EmployeeParametersDto employeeDetailsDto)
        {
            return await _employeeRepository.UpdateEmployeeDetails(employeeDetailsDto);
        }
        public async Task<PersonalDetailsHistoryDto> UpdatePersonalDetails(PersonalDetailsUpdateDto personalDetailsDto)
        {
            return await _employeeRepository.UpdatePersonalDetails(personalDetailsDto);
        }
        public async Task<string> UploadEmployeeDocuments(List<IFormFile> files, QualificationAttachmentDto skillset)
        {
            return await _employeeRepository.UploadEmployeeDocuments(files, skillset);
        }

        public async Task<string> InsertQualification (QualificationTableDto Qualification, string FirstEntityID, int EmpEntityIds)
            {
            return await _employeeRepository.InsertQualification (Qualification, FirstEntityID, EmpEntityIds);
            }
        public async Task<object> FillCountry ( )
            {
            return await _employeeRepository.FillCountry ( );
            }
        public async Task<object> GetBankType (int employeeId)
            {
            return await _employeeRepository.GetBankType (employeeId);
            }
        public async Task<object> GetGeneralSubCategoryList (string remarks)
            {
            return await _employeeRepository.GetGeneralSubCategoryList (remarks);
            }
        public async Task<string> SetEmpDocumentDetails (SetEmpDocumentDetailsDto SetEmpDocumentDetails)
            {
            return await _employeeRepository.SetEmpDocumentDetails (SetEmpDocumentDetails);
            }
        }
    }
