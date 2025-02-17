

using EMPLOYEE_INFORMATION.Models.EnumFolder;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using HRMS.EmployeeInformation.Repository.Common;
using Microsoft.AspNetCore.Mvc;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Service.Interface
{
    public interface IEmployeeInformationService
    {
        Task<PaginatedResult<EmployeeResultDto>> GetEmpData(EmployeeInformationParameters employeeInformationParameters);
        Task<EmployeeStatusResultDto> EmployeeStatusAsync(int employeeId, string parameterCode, string type);
        Task<List<LanguageSkillResultDto>> LanguageSkillAsync(int employeeId);
        Task<CommunicationResultDto> CommunicationAsync(int employeeId);
        //Task<CommunicationExtraDto> CommunicationExtra(int employeeId);

        Task<List<CommunicationExtraDto>> CommunicationExtraAsync(int employeeId);
        Task<List<CommunicationEmergencyDto>> CommunicationEmergencyAsync(int employeeId);
        Task<List<string>> HobbiesDataAsync(int employeeId);
        Task<List<RewardAndRecognitionDto>> RewardAndRecognitionAsync(int employeeId);
        Task<List<QualificationDto>> QualificationAsync(int employeeId);
        Task<List<SkillSetDto>> SkillSetsAsync(int employeeId);
        Task<List<AllDocumentsDto>> DocumentsAsync(int employeeId, List<string> excludedDocTypes);


        Task<List<DependentDto>> DependentAsync(int employeeId);
        Task<List<CertificationDto>> CertificationAsync(int employeeId);
        Task<List<DisciplinaryActionsDto>> DisciplinaryActionsAsync(int employeeId);
        Task<List<LetterDto>> LetterAsync(int employeeId);
        Task<List<ReferenceDto>> ReferenceAsync(int employeeId);
        Task<List<ProfessionalDto>> ProfessionalAsync(int employeeId);
        Task<List<AssetDto>> AssetAsync();
        Task<List<AssetDetailsDto>> AssetDetailsAsync(int employeeId);
        Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdownProfessionalAsync();
        Task<HrEmpProfdtlsApprlDto> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto);
        Task<PersonalDetailsHistoryDto> InsertOrUpdatePersonalData(PersonalDetailsHistoryDto persnldtlsApprlDto);

        Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID);
        Task<List<PersonalDetailsDto>> GetPersonalDetailsByIdAsync(int employeeid);
        Task<List<TrainingDto>> TrainingAsync(int employeeid);
        Task<List<CareerHistoryDto>> CareerHistoryAsync(int employeeid);
        Task<List<object>> BiometricDetailsAsync(int employeeId);
        //Task<List<object>> AccessDetails(int employeeId);
        Task<object> AccessDetailsAsync(int employeeId);
        Task<List<TransferAndPromotionDto>> TransferAndPromotionAsync(int employeeId);

        Task<List<Fill_ModulesWorkFlowDto>> FillModulesWorkFlowAsync(int entityID, int linkId);
        Task<List<Fill_WorkFlowMasterDto>> FillWorkFlowMasterAsync(int emp_Id, int roleId);
        Task<List<BindWorkFlowMasterEmpDto>> BindWorkFlowMasterEmpAsync(int linkId, int linkLevel);
        Task<List<SalarySeriesDto>> SalarySeriesAsync(int employeeId, string status);

        // Task<List<GetRejoinReportDto>> GetRejoinReport(int employeeId);

        Task<List<GetRejoinReportDto>> GetRejoinReportAsync(int employeeId);

        Task<List<GetEmpReportingReportDto>> GetEmpReportingReportAsync(int employeeId);
        Task<List<GetEmpWorkFlowRoleDetailstDto>> GetEmpWorkFlowRoleDetailsAsync(int linkId, int linkLevel);
        Task<List<FillEmpWorkFlowRoleDto>> FillEmpWorkFlowRoleAsync(int entityID);

        Task<List<AuditInformationDto>> AuditInformationAsync(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto);

        Task<List<object>> EmployeeTypeAsync(int employeeid);

        Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmployeesBasedOnwWorkflowAsync(int firstEntityId, int secondEntityId);



        Task<List<object>> GeoSpacingTypeAndCriteriaAsync(string type);
        Task<List<EmployeeHraDto>> HraDetailsAsync(int employeeId);
        Task<List<GeoSpacingDto>> GetGeoSpacingAsync(int employeeid);
        Task<List<object>> GetCountry();
        Task<List<object>> GetNationalities();
        Task<List<object>> GetBloodGroup();
        Task<List<object>> FillReligion();
        Task<string> InsertOrUpdateLanguageSkills(LanguageSkillsSaveDto langSkills);
        Task<List<object>> FillLanguageTypes();
        Task<List<object>> FillConsultant();
        Task<string> InsertOrUpdateReference(ReferenceSaveDto Reference);
        Task<List<object>> FillRewardType();
        Task<string> InsertOrUpdateEmpRewards(EmpRewardsSaveDto EmpRewards);
        Task<List<object>> FillBankDetails(int empID);
        Task<List<object>> BankTypeEdit();

        Task<EmployeeDetailsDto> GetHrEmpDetailsAsync(int employeeId, int roleId);
        Task<HrEmpMasterDto> SaveOrUpdateEmployeeDetails(EmployeeParametersDto employeeDetailsDto);
        Task<List<object>> CertificationsDropdown(string description);
        Task<string> InsertOrUpdateCertificates(CertificationSaveDto certificates);
        Task<string> UpdateEmployeeType(EmployeeTypeDto EmployeeType);
        Task<string> InsertOrUpdateSkill(SaveSkillSetDto skillset);
        Task<List<object>> FillEmployeeDropdown(string activeStatus, string employeeStatus, string probationStatus);
        Task<List<object>> AssetGroupDropdownEdit();
        Task<List<object>> GetAssetDropdownEdit(int varAssestTypeID);

        Task<List<object>> GetAssetDetailsEdit(string CommonName);
        Task<string> AssetEdit(AssetEditDto assetEdits);

        Task<List<object>> GetAssetEditDatas(int varSelectedTypeID, int varAssestID);
        Task<string> AssetDelete(int varEmpID, int varAssestID);
       
    }
       
}
