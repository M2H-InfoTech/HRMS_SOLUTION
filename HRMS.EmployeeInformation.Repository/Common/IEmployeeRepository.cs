using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using HRMS.EmployeeInformation.Models;
using Microsoft.AspNetCore.Http;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common
{
    public interface IEmployeeRepository
    {
        Task<PaginatedResult<EmployeeResultDto>> GetEmpData(EmployeeInformationParameters employeeInformationParameters);
        Task<EmployeeStatusResultDto> EmployeeStatus(int employeeId, string parameterCode, string type);
        Task<List<LanguageSkillResultDto>> LanguageSkillAsync(int employeeId);
        Task<CommunicationResultDto> CommunicationAsync(int employeeId);
        Task<List<CommunicationExtraDto>> CommunicationExtraAsync(int employeeId);
        Task<List<CommunicationEmergencyDto>> CommunicationEmergencyAsync(int employeeId);
        Task<List<string>> HobbiesDataAsync(int employeeId);
        Task<List<RewardAndRecognitionDto>> RewardAndRecognitionsAsync(int employeeId);
        Task<List<QualificationDto>> QualificationAsync(int employeeId);
        Task<List<SkillSetDto>> SkillSetsAsync(int employeeId);
        Task<List<AllDocumentsDto>> DocumentsAsync(int employeeId, List<string> excludedDocTypes);
        Task<List<DependentDto>> DependentAsync(int employeeId);
        Task<List<CertificationDto>> CertificationAsync(int employeeId);
        Task<List<DisciplinaryActionsDto>> DisciplinaryActionsAsync(int employeeId);
        Task<List<LetterDto>> LetterAsync(int employeeId);
        Task<List<ReferenceDto>> ReferenceAsync(int employeeId);
        Task<List<ProfessionalDto>> ProfessionalAsync(int employeeId);
        Task<List<AssetDto>> AsseAsync();
        Task<List<AssetDetailsDto>> AssetDetailsAsync(int employeeId);
        Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdownProfessionalAsync();
        Task<string?> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto);
        Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID);
        Task<List<PersonalDetailsDto>> GetPersonalDetailsByIdAsync(int employeeid);
        Task<List<TrainingDto>> TrainingAsync(int employeeid);
        Task<CareerHistoryResultDto> CareerHistoryAsync(int employeeid);
        Task<List<object>> BiometricDetailsAsync(int employeeId);
        Task<object> AccessDetailsAsync(int employeeid);
        Task<List<TransferAndPromotionDto>> TransferAndPromotionAsync(int employeeId);
        Task<List<Fill_ModulesWorkFlowDto>> FillModulesWorkFlowAsync(int entityID, int linkId);
        Task<List<Fill_WorkFlowMasterDto>> FillWorkFlowMasterAsync(int emp_Id, int roleId);
        Task<List<BindWorkFlowMasterEmpDto>> BindWorkFlowMasterEmpAsync(int linkId, int linkLevel);
        //Task<List<SalarySeriesDto>> SalarySeriesAsync(int employeeid, string status);
        Task<List<GetRejoinReportDto>> GetRejoinReportAsync(int employeeId);
        Task<List<GetEmpReportingReportDto>> GetEmpReportingReportAsync(int employeeId);
        Task<List<GetEmpWorkFlowRoleDetailstDto>> GetEmpWorkFlowRoleDetailsAsync(int linkId, int linkLevel);
        Task<List<FillEmpWorkFlowRoleDto>> FillEmpWorkFlowRoleAsync(int entityID);
        Task<List<AuditInformationDto>> AuditInformationAsync(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto);
        Task<List<object>> EmployeeTypeAsync(int employeeid);
        Task<List<object>> GeoSpacingTypeAndCriteriaAsync(string type);
        Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmployeesBasedOnwWorkflowAsync(int firstEntityId, int secondEntityId);
        Task<List<GeoSpacingDto>> GetGeoSpacingAsync(int employeeid);
        Task<List<EmployeeHraDto>> HraDetailsAsync(int employeeId);
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
        Task<string?> UpdateEmployeeDetails(EmployeeDetailsUpdateDto employeeDetailsDto);
        Task<string?> UpdatePersonalDetails(PersonalDetailsUpdateDto personalDetailsDto);
        Task<List<object>> CertificationsDropdown(string description);
        Task<string> InsertOrUpdateCertificates(CertificationSaveDto certificates);
        Task<string> UpdateEmployeeType(EmployeeTypeDto EmployeeType);
        Task<string> InsertOrUpdateSkill(SaveSkillSetDto skillset);
        Task<string> InsertQualification(QualificationTableDto Qualification, string updateType, string FirstEntityID, int EmpEntityIds);
        Task<object> FillCountry();
        Task<List<object>> FillEmployeeDropdown(string activeStatus, string employeeStatus, string probationStatus);
        Task<List<object>> AssetGroupDropdownEdit();
        Task<List<object>> GetAssetDropdownEdit(int varAssestTypeID);
        Task<List<object>> GetAssetDetailsEdit(string CommonName);
        Task<string> AssetEdit(AssetEditDto assetEdits);
        Task<List<object>> GetAssetEditDatas(int varSelectedTypeID, int varAssestID);
        Task<string> AssetDelete(int varEmpID, int varAssestID);
        Task<object> GetBankType(int employeeId);
        Task<object> GetGeneralSubCategoryList(string remarks);
        Task<string> SetEmpDocumentDetails(SetEmpDocumentDetailsDto SetEmpDocumentDetails);
        Task<List<FillDocumentTypeDto>> FillDocumentType(int EmpID);
        Task<List<DocumentFieldDto>> DocumentField(int DocumentID);
        Task<string> InsertDocumentsFieldDetails(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy);
        Task<string> SetEmpDocuments(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy);
        Task<bool> IsWorkflowNeeded();
        Task<string> GenerateRequestId(int empId);
        Task<string?> GetLastSequence(string codeId);
        Task UpdateCodeGeneration(string codeId);
        Task<string?> InsertLetterTypeRequest(LetterInsertUpdateDto LetterInsertUpdateDtos);
        Task<object> EditEmployeeCommonInformation(string? empIds, int? employeeid);
        Task<string?> EditInformationAsync(List<TmpEmpInformation> inputs);
        Task<object> GetInformationDescriptionAsync(int infoId);
        Task<object> GetLetterTypeAsync();
        Task<object> LetterSignatureAuthorityAsync();
        Task<LoadCompanyDetailsResultDto> LoadCompanyDetailsAsync(LoadCompanyDetailsRequestDto loadCompanyDetailsRequestDto);
        Task<object> GetLevelAsync(int level);
        Task<string> DirectUploadLetter(List<IFormFile> files, string filePath, int masterID);
        Task<string> UploadOrUpdateEmployeeDocuments(List<IFormFile> files, string filePath, QualificationAttachmentDto attachmentDto);
        Task<string?> CheckLetterTypeRequest(int? LetterTypeId, int? LetterSubType, int? MasterId);
        Task<string> DeleteDesciplinaryLetter(string? masterId);
        Task<object> GetAllLetterType();
        Task<LetterMaster01Dto> GetLetterSubTypeByIdAsync(int LetterSubTypeID);
        Task<int?> GetLastEntity(int? empId);
        Task<object> GetUserRoles(int? firstEntityId, int? secondEntityId);
        Task<(int ErrorID, string ErrorMessage)> UpdateProfessionalDetailsAsync(HrEmpProfdtlsApprlDto dto);
        Task<(string EmployeeStatuses, string SystemStatuses)> GetEmployeeAndSystemStatusesAsync(int empId);
        Task<object> GetReligionsAsync();
        Task<object> GetEmployeeMasterHeaderDataAsync();
        Task<object> GetCategoryMasterDetailsAsync(int roleId);
        Task<object> GetEmployeeMasterHeaderEditDataAsync();
        Task<object> GetFieldsToHideAsync();
        Task<object> EmployeeCreationFilterAsync();
        Task<IEnumerable<DependentDto1>> GetDependentsByEmpId(int empId);
        Task<List<DailyRatePolicyDto>> GetDailyRatePoliciesAsync();
        Task<object> GetWageTypesWithRatesAsync();
        Task<int> IsEnableWeddingDate(int empId);
        Task<object> GetEmployeePersonalDetails(int empId);
        Task<object> FillEmpProject(int empId);
        Task<string> DeleteEmployeeDetails(string empIds, int entryBy);
        Task<object> GetProbationEffective(string linkId);
        //Task<(int errorID, string errorMessage)> DeleteSavedEmployeeAsync(int empId, string status, int entryBy);
        Task<int> UpdateEditEmployeeDetailsAsync(UpdateEmployeeRequestDto request);

        //Task<(int, string)> UpdateEditEmployeeDetailsAsync(UpdateEmployeeRequestDto request);
        Task<object> GetGeoDetails(string mode, int? geoSpacingType, int? geoCriteria);
        Task<string?> EmployeeHraDtoAsync(EmployeeHraDto EmployeeHraDtos);

        Task<object> GetEmployeeCertifications(int employeeid);
        Task<string> DeleteCertificate(int certificateid);

        Task<string?> AddEmpModuleDetailsAsync(BiometricDto BiometricDto);
        List<ParamWorkFlowViewDto> GetWorkFlowData(int linkLevel, int valueId);
        Task<UpdateResult> UpdateWorkFlowELAsync(ParamWorkFlow01s2sDto dto);
        Task<List<Dictionary<string, object>>> SalarySeriesAsync1(int employeeId, string status);
        Task<int> GetAgeLimitValue(int empId);
        Task<ProfessionalDto> GetUpdateProfessional(int empId, string updateType, int Detailid);
        Task<QualificationTableDto> GetUpdateQualification(int empId, string updateType, int Detailid);
        Task<RewardAndRecognitionDto> GetEmployeeRewardsDetails(int empId);
        Task<SkillSetDto> GetUpdateTechnical(int empId, string updateType, int Detailid);
        Task<CommunicationTableDto> GetUpdateCommunication(int empId, string updateType, int Detailid);
        Task<CommunicationTableDto> GetUpdateCommunicationExtra(int empId, string updateType, int Detailid);
        Task<CommunicationTableDto> GetUpdateEmergencyExtra(int empId, int Detailid);
        Task<ReferenceDto> GetUpdateReference(int Detailid);
        Task<List<EmployeeLanguageSkill>> RetrieveEmployeeLanguage(int empId, int Detailid);
        Task<object> EmployeeCreationFilterAsync(int? firstEntityId);
        Task<List<ParamRoleViewDto>> EditRoleELAsync (int linkLevel, int valueId);
        Task<UpdateResult> UpdateRoleEL (ParamRole01AND02Dto dto);
        Task<CompanyParameterDto> EnableGeoCriteria ( );
        Task<string> GetGeoCoordinateNameStatus (int EmployeeId);
        Task<string> GetGeotaggingMasterStatus (int EmployeeId);
        Task<List<EmployeeDocumentListDto>> DownloadIndividualEmpDocuments (int EmployeeId);
        Task<List<DocumentDetailDto>> GetDocumentDetailsAsync (string status, int detailId);
        Task<int> GetSlabEnabledAsync (int enteredBy);
        Task<int> EnableNewQualif(int empId);
       

        Task<int> GetDefaultAttendancePolicyAsync(int empId);
        string GetControlValue(dynamic param, string controlType, bool isMultiple);
       // Task<(int, string)> UpdateEditEmployeeDetailsAsync(UpdateEmployeeRequestDto request);
        Task AssignHolidayAccessAsync(AssignEmployeeAccessRequestDto request);
        Task AssignAttendancePolicyAccessAsync(AssignEmployeeAccessRequestDto request);
        //helper
        
        //
        Task AssignShiftAccessAsync(AssignEmployeeAccessRequestDto request);
        //helper for LEAVE POLICY ACCESS
        Task<int?> GetDefaultLeavePolicyAsync(int employeeId);
        //-- LEAVE POLICY ACCESS
        Task AssignLeavePolicyIfNotExistsAsync(AssignEmployeeAccessRequestDto request);
        //-- EMPLOYEE LEAVE ACCESS
        Task AssignEmployeeLeaveAccessAsync(AssignEmployeeAccessRequestDto request);
        // basic leave settings
        Task AssignLeaveBasicSettingsAccessAsync(AssignEmployeeAccessRequestDto request);
        //SaveWorkFlowMasterEMp   Mode : InsertWorkFlowEL  SP : WORKFLOW_ASSIGN
        Task<int> SaveParamWorkflow(SaveParamWorkflowDto request);
        //SaveWorkFlowEmp  Mode : InsertRoleEL
        Task<int> InsertRoleAsync(RoleInsertDTO roleInsertDto);
        //Mode : RoleRetrieveEL
        Task<List<RoleDetailsDTO>> GetRoleDetailsAsync(int linkId, int linkLevel);

        Task AssignEmployeeAccessAsync(AssignEmployeeAccessRequestDto request);
        //Task<List<object>> GetGeoCoordinatesAsync(int geoSpacingType, int geoCriteria);

        Task<List<object>> GetGeoSpacingCriteria();
        Task<List<object>> GetGeoCoordinatesTabAsync(int geoSpacingType, int geoCriteria);

        Task<string> SaveOrUpdateGeoLocationAsync(SaveGeoLocationRequestDTO dto);
        Task<IEnumerable<AssetCategoryCodeDto>> GetFilteredAssetCategoriesAsync(int varAssetTypeID);

        Task<IEnumerable<AssetCategoryCodeDto>> GetAssignedOrPendingAssetCategoriesAsync(int varAssetTypeID, string varAssignAssetStatus);
        Task<IEnumerable<ReasonDto>> GetGeneralSubCategoryAsync(string code);
        //SaveEmployeeShift  Mode : InsertShiftEmpCreation  SP : ShiftAssignSettings
        Task<string> SaveShiftMasterAccessAsync(ShiftMasterAccessInputDto dto);
        //FillLanguageTypes  Mode : FillLanguageTypes  SP : EmployeeCreation
        Task<List<object>> GetLanguagesAsync();

    }
}
