using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.DTO;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using HRMS.EmployeeInformation.DTO.DTOs.PayScale;
using HRMS.EmployeeInformation.Models;
using HRMS.EmployeeInformation.Repository.Common;
using Microsoft.AspNetCore.Http;
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
        Task<string?> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto);
        Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID);
        Task<List<PersonalDetailsDto>> GetPersonalDetailsByIdAsync(int employeeid);
        Task<List<TrainingDto>> TrainingAsync(int employeeid);
        Task<CareerHistoryResultDto> CareerHistoryAsync(int employeeid);
        Task<List<object>> BiometricDetailsAsync(int employeeId);
        Task<object> AccessDetailsAsync(int employeeId);
        Task<List<TransferAndPromotionDto>> TransferAndPromotionAsync(int employeeId);
        Task<List<Fill_ModulesWorkFlowDto>> FillModulesWorkFlowAsync(int entityID, int linkId);
        Task<List<Fill_WorkFlowMasterDto>> FillWorkFlowMasterAsync(int emp_Id, int roleId);
        Task<List<BindWorkFlowMasterEmpDto>> BindWorkFlowMasterEmpAsync(int linkId, int linkLevel);
        //Task<List<SalarySeriesDto>> SalarySeriesAsync(int employeeId, string status);
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
        Task<List<object>> CertificationsDropdownAsync(string description);
        Task<string> InsertOrUpdateCertificatesAsync(CertificationSaveDto certificates);
        Task<string> UpdateEmployeeTypeAsync(EmployeeTypeDto EmployeeType);
        Task<string> InsertOrUpdateSkillAsync(SaveSkillSetDto skillset);
        Task<string> InsertQualificationAsync(QualificationTableDto Qualification, string updateType, string FirstEntityID, int EmpEntityIds);
        Task<object> FillCountryAsync();
        Task<string?> UpdateEmployeeDetailsAsync(EmployeeDetailsUpdateDto employeeDetailsDto);
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
        Task<string> InsertDocumentsFieldDetailsAsync(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy);
        Task<string> SetEmpDocumentsAsync(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy);
        Task<string?> InsertLetterTypeRequestAsync(LetterInsertUpdateDto LetterInsertUpdateDtos);
        Task<object> EditEmployeeCommonInformationAsync(string? empIds, int? employeeid);
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
        Task<int?> GetLastEntityByEmployeeId(int? empId);
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
        Task<int> UpdateEditEmployeeDetails(UpdateEmployeeRequestDto request);
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
        Task<object> GetAccessLevelByRoleId(int? firstEntityId);
        Task<List<ParamRoleViewDto>> EditRoleELAsync(int linkLevel, int valueId);
        Task<UpdateResult> UpdateRoleEL(ParamRole01AND02Dto dto);
        Task<CompanyParameterDto> EnableGeoCriteria();
        Task<string> GetGeoCoordinateNameStatus(int EmployeeId);
        Task<string> GetGeotaggingMasterStatus(int EmployeeId);
        Task<List<EmployeeDocumentListDto>> DownloadIndividualEmpDocuments(int EmployeeId); //DOWNLOAD ALL OPTION IN DOCUMENTS
        Task<List<DocumentDetailDto>> GetDocumentDetailsAsync(string status, int detailId);
        Task<int> GetSlabEnabledAsync(int enteredBy);
        Task<int> EnableNewQualif(int empId);
        //Reassign
        Task AssignEmployeeAccessService(AssignEmployeeAccessRequestDto request);
        //InsertWorkFlowEL
        Task InsertWorkFlow(SaveParamWorkflowDto request);
        //SaveWorkFlowEmp  Mode : InsertRoleEL
        Task<int> InsertRoleAsync(RoleInsertDTO roleInsertDto);

        Task<List<RoleDetailsDTO>> GetRoleDetailsAsync(int linkId, int linkLevel);
        Task<List<object>> GetGeoSpacingCriteriaAsync();
        Task<List<object>> GetGeoCoordinatesTabAsync(int geoSpacingType, int geoCriteria);

        Task<string> SaveGeoLocationAsync(SaveGeoLocationRequestDTO dto);
        Task<IEnumerable<AssetCategoryCodeDto>> GetFilteredAssetCategoriesAsync(int varAssetTypeID);
        Task<IEnumerable<AssetCategoryCodeDto>> GetAssignedOrPendingAssetCategoriesAsync(int varAssetTypeID, string varAssignAssetStatus);
        Task<IEnumerable<ReasonDto>> GetGeneralSubCategoryAsync(string code);
        Task<string> SaveShiftMasterAccessAsync(ShiftMasterAccessInputDto request);
        Task<List<object>> GetLanguagesAsync();
        Task<PayscaleComponentsResponseDto> PayscaleComponentsListManual(int batchId, int employeeIds, int type);
        Task<List<HighLevelTableDto>> GetAccessLevel();
        Task<int> AddEmployeeAsync(AddEmployeeDto inserEmployeeDto);
        Task<List<object>> RetrieveShiftEmpCreationAsync();

        Task<List<object>> FillWeekEndShiftEmpCreationAsync();
        Task<List<object>> FillbatchslabsEmpAsync(int batchid);
        Task<int> EnableBatchOptionEmpwiseAsync(int empid);
        Task<List<object>> GetParameterShiftInEmpAsync();
        Task<List<object>> RetrieveEmpparametersAsync(int empid);
        Task<List<object>> ShowEntityLinkCheckBoxAsync(int roleid);
        Task<List<object>> EnableDocEditAsync();
        Task<int> CheckLiabilityPending(int empid);

        Task<List<GeoLocationDto>> GetAccessibleGeoLocationsAsync(int roleId, int empId);// Created By Shan Lal k

        Task<List<DocumentsDownoaldDto>> DownloadSingleDocumentsAsync(int DetailID, string status);

        Task<List<DocumentsDownoaldDto>> DownloadEmpDocumentsAsync(int DetailID, string status);

        Task<List<CoordinateDto>> FillcordinateAsync(int value);

        Task<List<GeocoordinatesDto>> GetcordinatesAsync(int GeoMasterID, int GeoSpaceType);
        Task<string> UpdateEmpStatusAsync(UpdateEmployeeStatusDto employeeModuleSetupDto);

        Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmpRoleReporteesAsync(int SecondEntityId, int FirstEntityId, string Prefix);
        Task<List<HrmsDocumentField00>> GetDependentFieldsAsync();
        Task<PayscaleResultDto> GetLatestPayscaleAsync(int employeeId, int? type);
        Task<int> SaveManualEmpPayscaleOldFormat(SaveManualEmpPayscaleOldFormatDto dto); //Created By Shan Lal
        Task<List<string>> DdlIsprobationAsync(int FirstEntityID, string LinkID);
        Task<int> GetlastEntityByRoleId([FromBody] EntityRoleRequestDto customEntityList);

    }

}
