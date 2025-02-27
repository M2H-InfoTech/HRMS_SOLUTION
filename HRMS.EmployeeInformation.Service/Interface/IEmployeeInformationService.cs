using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using HRMS.EmployeeInformation.Repository.Common;
using Microsoft.AspNetCore.Http;
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
        //Task<HrEmpProfdtlsApprlDto> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto);
        Task<string?> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto);
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
        Task<List<object>> GetCountryAsync();
        Task<List<object>> GetNationalitiesAsync();
        Task<List<object>> GetBloodGroupAsync();
        Task<List<object>> FillReligionAsync();
        Task<string> InsertOrUpdateLanguageSkillsAsync(LanguageSkillsSaveDto langSkills);
        Task<List<object>> FillLanguageTypesAsync();
        Task<List<object>> FillConsultantAsync();
        Task<string> InsertOrUpdateReferenceAsync(ReferenceSaveDto Reference);
        Task<List<object>> FillRewardTypeAsync();
        Task<string> InsertOrUpdateEmpRewardsAsync(EmpRewardsSaveDto EmpRewards);
        Task<List<object>> FillBankDetailsAsync(int empID);
        Task<List<object>> BankTypeEditAsync();

        Task<EmployeeDetailsDto> GetHrEmpDetailsAsync(int employeeId, int roleId);
        //sk<HrEmpMasterDto> SaveOrUpdateEmployeeDetails(EmployeeParametersDto employeeDetailsDto);
        Task<List<object>> CertificationsDropdownAsync(string description);
        Task<string> InsertOrUpdateCertificatesAsync(CertificationSaveDto certificates);
        Task<string> UpdateEmployeeTypeAsync(EmployeeTypeDto EmployeeType);
        Task<string> InsertOrUpdateSkillAsync(SaveSkillSetDto skillset);
        Task<string> UploadEmployeeDocumentsAsync(List<IFormFile> files, QualificationAttachmentDto skillset);

        Task<string> InsertQualificationAsync(QualificationTableDto Qualification, string FirstEntityID, int EmpEntityIds);
        Task<object> FillCountryAsync();

        //Task<EmployeeDetailsUpdateDto> UpdateEmployeeDetails(EmployeeDetailsUpdateDto employeeDetailsDto, int lastEntity);
        Task<string?> UpdateEmployeeDetailsAsync(EmployeeDetailsUpdateDto employeeDetailsDto);

        //Task<PersonalDetailsHistoryDto> UpdatePersonalDetailsAsync(PersonalDetailsUpdateDto personalDetailsDto);
        Task<string?> UpdatePersonalDetailsAsync(PersonalDetailsUpdateDto personalDetailsDto);

        Task<List<object>> FillEmployeeDropdownAsync(string activeStatus, string employeeStatus, string probationStatus);
        Task<List<object>> AssetGroupDropdownEditAsync();
        Task<List<object>> GetAssetDropdownEditAsync(int varAssestTypeID);

        Task<List<object>> GetAssetDetailsEditAsync(string CommonName);
        Task<string> AssetEditAsync(AssetEditDto assetEdits);

        Task<List<object>> GetAssetEditDatasAsync(int varSelectedTypeID, int varAssestID);
        Task<string> AssetDeleteAsync(int varEmpID, int varAssestID);
        Task<object> GetBankTypeAsync(int employeeId);
        Task<object> GetGeneralSubCategoryListAsync(string remarks);
        Task<string> SetEmpDocumentDetailsAsync(SetEmpDocumentDetailsDto SetEmpDocumentDetails);
        Task<List<FillDocumentTypeDto>> FillDocumentTypeAsync(int EmpID);
        Task<List<DocumentFieldDto>> DocumentFieldAsync(int DocumentID);
        Task<List<DocumentGetGeneralSubCategoryListDto>> DocumentGetGeneralSubCategoryListAsync(string Remarks);
        Task<string> InsertDocumentsFieldDetailsAsync(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy);
        Task<string> SetEmpDocumentsAsync(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy);

    }

}
