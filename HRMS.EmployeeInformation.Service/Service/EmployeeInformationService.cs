using EMPLOYEE_INFORMATION.Models.Entity;
using HRMS.EmployeeInformation.DTO;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using HRMS.EmployeeInformation.DTO.DTOs.PayScale;
using HRMS.EmployeeInformation.Models;
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
            return await _employeeRepository.AsseAsync();
        }
        public async Task<List<AssetDetailsDto>> AssetDetailsAsync(int employeeId)
        {
            return await _employeeRepository.AssetDetailsAsync(employeeId);
        }

        public async Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdownProfessionalAsync()
        {
            return await _employeeRepository.CurrencyDropdownProfessionalAsync();
        }
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

        public async Task<CareerHistoryResultDto> CareerHistoryAsync(int employeeid)
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

        //public async Task<List<Dictionary<string, object>>> SalarySeriesAsync1(int employeeId, string status)
        //{
        //    return await _employeeRepository.SalarySeriesAsync1(employeeId, status);
        //}
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
        public async Task<List<object>> GetCountryAsync()
        {
            return await _employeeRepository.GetCountry();
        }
        public async Task<List<object>> GetNationalitiesAsync()
        {
            return await _employeeRepository.GetNationalities();
        }
        public async Task<List<object>> GetBloodGroupAsync()
        {
            return await _employeeRepository.GetBloodGroup();
        }
        public async Task<List<object>> FillReligionAsync()
        {
            return await _employeeRepository.FillReligion();
        }

        public async Task<string> InsertOrUpdateLanguageSkillsAsync(LanguageSkillsSaveDto langSkills)
        {
            return await _employeeRepository.InsertOrUpdateLanguageSkills(langSkills);
        }
        public async Task<List<object>> FillLanguageTypesAsync()
        {
            return await _employeeRepository.FillLanguageTypes();
        }
        public async Task<List<object>> FillConsultantAsync()
        {
            return await _employeeRepository.FillConsultant();
        }

        public async Task<string> InsertOrUpdateReferenceAsync(ReferenceSaveDto Reference)
        {
            return await _employeeRepository.InsertOrUpdateReference(Reference);
        }
        public async Task<List<object>> FillRewardTypeAsync()
        {
            return await _employeeRepository.FillRewardType();
        }
        public async Task<string> InsertOrUpdateEmpRewardsAsync(EmpRewardsSaveDto EmpRewards)
        {
            return await _employeeRepository.InsertOrUpdateEmpRewards(EmpRewards);
        }
        public async Task<List<object>> FillBankDetailsAsync(int empID)
        {
            return await _employeeRepository.FillBankDetails(empID);
        }
        public async Task<List<object>> BankTypeEditAsync()
        {
            return await _employeeRepository.BankTypeEdit();
        }

        public async Task<EmployeeDetailsDto> GetHrEmpDetailsAsync(int employeeId, int roleId)
        {
            return await _employeeRepository.GetHrEmpDetailsAsync(employeeId, roleId);
        }
        public async Task<List<object>> CertificationsDropdownAsync(string description)
        {
            return await _employeeRepository.CertificationsDropdown(description);
        }
        public async Task<string> InsertOrUpdateCertificatesAsync(CertificationSaveDto certificates)
        {
            return await _employeeRepository.InsertOrUpdateCertificates(certificates);
        }

        public async Task<string> UpdateEmployeeTypeAsync(EmployeeTypeDto EmployeeType)
        {
            return await _employeeRepository.UpdateEmployeeType(EmployeeType);
        }
        public async Task<string> InsertOrUpdateSkillAsync(SaveSkillSetDto skillset)
        {
            return await _employeeRepository.InsertOrUpdateSkill(skillset);
        }
        public async Task<List<object>> FillEmployeeDropdownAsync(string activeStatus, string employeeStatus, string probationStatus)
        {
            return await _employeeRepository.FillEmployeeDropdown(activeStatus, employeeStatus, probationStatus);
        }
        public async Task<List<object>> AssetGroupDropdownEditAsync()
        {
            return await _employeeRepository.AssetGroupDropdownEdit();
        }

        public async Task<List<object>> GetAssetDropdownEditAsync(int varAssestTypeID)
        {
            return await _employeeRepository.GetAssetDropdownEdit(varAssestTypeID);
        }



        public async Task<List<object>> GetAssetDetailsEditAsync(string CommonName)
        {
            return await _employeeRepository.GetAssetDetailsEdit(CommonName);
        }
        public async Task<string> AssetEditAsync(AssetEditDto assetEdits)
        {
            return await _employeeRepository.AssetEdit(assetEdits);
        }
        public async Task<List<object>> GetAssetEditDatasAsync(int varSelectedTypeID, int varAssestID)
        {
            return await _employeeRepository.GetAssetEditDatas(varSelectedTypeID, varAssestID);
        }


        public async Task<string> AssetDeleteAsync(int varEmpID, int varAssestID)
        {
            return await _employeeRepository.AssetDelete(varEmpID, varAssestID);
        }
        //public async Task<EmployeeDetailsUpdateDto> UpdateEmployeeDetails(EmployeeDetailsUpdateDto employeeDetailsDto, int lastEntity)
        //{
        //    return await _employeeRepository.UpdateEmployeeDetails(employeeDetailsDto, lastEntity);
        //}
        public async Task<string?> UpdateEmployeeDetailsAsync(EmployeeDetailsUpdateDto employeeDetailsDto)
        {
            return await _employeeRepository.UpdateEmployeeDetails(employeeDetailsDto);
        }
        //public async Task<PersonalDetailsHistoryDto> UpdatePersonalDetailsAsync(PersonalDetailsUpdateDto personalDetailsDto)
        //{
        //    return await _employeeRepository.UpdatePersonalDetails(personalDetailsDto);
        //}
        public async Task<string?> UpdatePersonalDetailsAsync(PersonalDetailsUpdateDto personalDetailsDto)
        {
            return await _employeeRepository.UpdatePersonalDetails(personalDetailsDto);
        }
        //public async Task<string> UploadEmployeeDocumentsAsync(List<IFormFile> files, QualificationAttachmentDto skillset)
        //{
        //    return await _employeeRepository.UploadEmployeeDocuments(files, skillset);
        //}

        public async Task<string> InsertQualificationAsync(QualificationTableDto Qualification, string updateType, string FirstEntityID, int EmpEntityIds)
        {
            return await _employeeRepository.InsertQualification(Qualification, updateType, FirstEntityID, EmpEntityIds);
        }
        public async Task<object> FillCountryAsync()
        {
            return await _employeeRepository.FillCountry();
        }
        public async Task<object> GetBankTypeAsync(int employeeId)
        {
            return await _employeeRepository.GetBankType(employeeId);
        }
        public async Task<object> GetGeneralSubCategoryListAsync(string remarks)
        {
            return await _employeeRepository.GetGeneralSubCategoryList(remarks);
        }
        public async Task<string> SetEmpDocumentDetailsAsync(SetEmpDocumentDetailsDto SetEmpDocumentDetails)
        {
            return await _employeeRepository.SetEmpDocumentDetails(SetEmpDocumentDetails);
        }
        public async Task<List<FillDocumentTypeDto>> FillDocumentTypeAsync(int EmpID)
        {
            return await _employeeRepository.FillDocumentType(EmpID);
        }
        public async Task<List<DocumentFieldDto>> DocumentFieldAsync(int DocumentID)
        {
            return await _employeeRepository.DocumentField(DocumentID);
        }
        public async Task<string> InsertDocumentsFieldDetailsAsync(List<TmpDocFileUpDto> DocumentBankField, int DocumentID, int In_EntryBy)
        {
            return await _employeeRepository.InsertDocumentsFieldDetails(DocumentBankField, DocumentID, In_EntryBy);
        }
        public async Task<string> SetEmpDocumentsAsync(TmpFileUpDto DocumentBankField, int DetailID, string Status, int In_EntryBy)
        {
            return await _employeeRepository.SetEmpDocuments(DocumentBankField, DetailID, Status, In_EntryBy);
        }

        //public async Task<string?> InsertLetterTypeRequestAsync(List<IFormFile> files, LetterInsertUpdateDto LetterInsertUpdateDtos)
        //{
        //    return await _employeeRepository.InsertLetterTypeRequest(files, LetterInsertUpdateDtos);
        //}
        public async Task<string?> InsertLetterTypeRequestAsync(LetterInsertUpdateDto LetterInsertUpdateDtos)
        {
            return await _employeeRepository.InsertLetterTypeRequest(LetterInsertUpdateDtos);
        }
        public async Task<object> EditEmployeeCommonInformationAsync(string? empIds, int? employeeid)
        {
            return await _employeeRepository.EditEmployeeCommonInformation(empIds, employeeid);
        }
        public async Task<string?> EditInformationAsync(List<TmpEmpInformation> inputs)
        {
            return await _employeeRepository.EditInformationAsync(inputs);
        }

        public async Task<object> GetInformationDescriptionAsync(int infoId)
        {
            return await _employeeRepository.GetInformationDescriptionAsync(infoId);
        }

        public async Task<object> GetLetterTypeAsync()
        {
            return await _employeeRepository.GetLetterTypeAsync();
        }

        public async Task<object> LetterSignatureAuthorityAsync()
        {
            return await _employeeRepository.LetterSignatureAuthorityAsync();
        }

        public async Task<LoadCompanyDetailsResultDto> LoadCompanyDetailsAsync(LoadCompanyDetailsRequestDto loadCompanyDetailsRequestDto)
        {
            return await _employeeRepository.LoadCompanyDetailsAsync(loadCompanyDetailsRequestDto);
        }

        public async Task<object> GetLevelAsync(int level)
        {
            return await _employeeRepository.GetLevelAsync(level);
        }
        public async Task<string> DirectUploadLetter(List<IFormFile> files, string filePath, int masterID)
        {
            return await _employeeRepository.DirectUploadLetter(files, filePath, masterID);
        }


        public async Task<string> UploadOrUpdateEmployeeDocuments(List<IFormFile> files, string filePath, QualificationAttachmentDto attachmentDto)
        {
            return await _employeeRepository.UploadOrUpdateEmployeeDocuments(files, filePath, attachmentDto);
        }

        public async Task<string?> CheckLetterTypeRequest(int? LetterTypeId, int? LetterSubType, int? MasterId)
        {
            return await _employeeRepository.CheckLetterTypeRequest(LetterTypeId, LetterSubType, MasterId);
        }

        public async Task<string> DeleteDesciplinaryLetter(string? masterId)
        {
            return await _employeeRepository.DeleteDesciplinaryLetter(masterId);
        }

        public async Task<object> GetAllLetterType()
        {
            return await _employeeRepository.GetAllLetterType();
        }

        public Task<LetterMaster01Dto> GetLetterSubTypeByIdAsync(int LetterSubTypeID)
        {
            return _employeeRepository.GetLetterSubTypeByIdAsync(LetterSubTypeID);
        }

        public Task<int?> GetLastEntityByEmployeeId(int? empId)
        {
            return _employeeRepository.GetLastEntity(empId);
        }

        public Task<object> GetUserRoles(int? firstEntityId, int? secondEntityId)
        {
            return _employeeRepository.GetUserRoles(firstEntityId, secondEntityId);
        }

        public Task<(int ErrorID, string ErrorMessage)> UpdateProfessionalDetailsAsync(HrEmpProfdtlsApprlDto dto)
        {
            return _employeeRepository.UpdateProfessionalDetailsAsync(dto);
        }

        public Task<(string EmployeeStatuses, string SystemStatuses)> GetEmployeeAndSystemStatusesAsync(int empId)
        {
            return _employeeRepository.GetEmployeeAndSystemStatusesAsync(empId);
        }

        public Task<object> GetReligionsAsync()
        {
            return _employeeRepository.GetReligionsAsync();
        }

        public Task<object> GetEmployeeMasterHeaderDataAsync()
        {
            return _employeeRepository.GetEmployeeMasterHeaderDataAsync();
        }

        public Task<object> GetCategoryMasterDetailsAsync(int roleId)
        {
            return _employeeRepository.GetCategoryMasterDetailsAsync(roleId);
        }

        public Task<object> GetEmployeeMasterHeaderEditDataAsync()
        {
            return _employeeRepository.GetEmployeeMasterHeaderEditDataAsync();
        }

        public Task<object> GetFieldsToHideAsync()
        {
            return _employeeRepository.GetFieldsToHideAsync();
        }

        public Task<object> EmployeeCreationFilterAsync()
        {
            return _employeeRepository.EmployeeCreationFilterAsync();
        }

        public async Task<IEnumerable<DependentDto1>> GetDependentsByEmpId(int empId)
        {
            return await _employeeRepository.GetDependentsByEmpId(empId);
        }

        public async Task<List<DailyRatePolicyDto>> GetDailyRatePoliciesAsync()
        {
            return await _employeeRepository.GetDailyRatePoliciesAsync();
        }

        public Task<object> GetWageTypesWithRatesAsync()
        {
            return _employeeRepository.GetWageTypesWithRatesAsync();
        }

        public async Task<int> IsEnableWeddingDate(int empId)
        {
            return await _employeeRepository.IsEnableWeddingDate(empId);
        }

        public async Task<object> GetEmployeePersonalDetails(int empId)
        {
            return await _employeeRepository.GetEmployeePersonalDetails(empId);
        }

        public async Task<object> FillEmpProject(int empId)
        {
            return await _employeeRepository.FillEmpProject(empId);
        }

        public async Task<string> DeleteEmployeeDetails(string empIds, int entryBy)
        {
            return await _employeeRepository.DeleteEmployeeDetails(empIds, entryBy);
        }

        public async Task<object> GetProbationEffective(string linkId)
        {
            return await _employeeRepository.GetProbationEffective(linkId);
        }

        //public async Task<(int, string)> UpdateEditEmployeeDetails(UpdateEmployeeRequestDto request)
        //{
        //    return await _employeeRepository.UpdateEditEmployeeDetailsAsync(request);
        //}
        public async Task<int> UpdateEditEmployeeDetails(UpdateEmployeeRequestDto request)
        {
            return await _employeeRepository.UpdateEditEmployeeDetailsAsync(request);
        }

        public async Task<object> GetGeoDetails(string mode, int? geoSpacingType, int? geoCriteria)
        {
            return await _employeeRepository.GetGeoDetails(mode, geoSpacingType, geoCriteria);
        }

        public async Task<List<HighLevelTableDto>> GetAccessLevel()
        {
            return await _employeeRepository.GetAccessLevel();
        }

        public async Task<object> AddEmployeeAsync(AddEmployeeDto inserEmployeeDto)
        {
            return await _employeeRepository.AddEmployeeAsync(inserEmployeeDto);
        }

        //public Task<(int errorID, string errorMessage)> DeleteSavedEmployeeAsync(int empId, string status, int entryBy)
        //{
        //    return _employeeRepository.DeleteSavedEmployee(empId, status, entryBy);
        //}

        public async Task<string?> EmployeeHraDtoAsync(EmployeeHraDto EmployeeHraDtos)
        {
            return await _employeeRepository.EmployeeHraDtoAsync(EmployeeHraDtos);
        }
        public async Task<object> GetEmployeeCertifications(int employeeid)
        {
            return await _employeeRepository.GetEmployeeCertifications(employeeid);
        }
        public async Task<string> DeleteCertificate(int certificateid)
        {
            return await _employeeRepository.DeleteCertificate(certificateid);
        }


        public async Task<string?> AddEmpModuleDetailsAsync(BiometricDto BiometricDto)
        {
            return await _employeeRepository.AddEmpModuleDetailsAsync(BiometricDto);
        }
        public List<ParamWorkFlowViewDto> GetWorkFlowData(int linkLevel, int valueId)
        {
            return _employeeRepository.GetWorkFlowData(linkLevel, valueId);
        }
        public async Task<UpdateResult> UpdateWorkFlowELAsync(ParamWorkFlow01s2sDto dto)
        {
            return await _employeeRepository.UpdateWorkFlowELAsync(dto);
        }

        public async Task<List<Dictionary<string, object>>> SalarySeriesAsync1(int employeeId, string status)
        {
            return await _employeeRepository.SalarySeriesAsync1(employeeId, status);
        }
        public async Task<int> GetAgeLimitValue(int empId)
        {
            return await _employeeRepository.GetAgeLimitValue(empId);
        }

        public async Task<ProfessionalDto> GetUpdateProfessional(int empId, string updateType, int Detailid)
        {
            return await _employeeRepository.GetUpdateProfessional(empId, updateType, Detailid);
        }
        public async Task<QualificationTableDto> GetUpdateQualification(int empId, string updateType, int Detailid)
        {
            return await _employeeRepository.GetUpdateQualification(empId, updateType, Detailid);
        }
        public async Task<RewardAndRecognitionDto> GetEmployeeRewardsDetails(int empId)
        {
            return await _employeeRepository.GetEmployeeRewardsDetails(empId);
        }
        public async Task<SkillSetDto> GetUpdateTechnical(int empId, string updateType, int Detailid)
        {
            return await _employeeRepository.GetUpdateTechnical(empId, updateType, Detailid);
        }
        public async Task<CommunicationTableDto> GetUpdateCommunication(int empId, string updateType, int Detailid)
        {
            return await _employeeRepository.GetUpdateCommunication(empId, updateType, Detailid);
        }
        public async Task<CommunicationTableDto> GetUpdateCommunicationExtra(int empId, string updateType, int Detailid)
        {
            return await _employeeRepository.GetUpdateCommunicationExtra(empId, updateType, Detailid);
        }
        public async Task<CommunicationTableDto> GetUpdateEmergencyExtra(int empId, int Detailid)
        {
            return await _employeeRepository.GetUpdateEmergencyExtra(empId, Detailid);
        }
        public async Task<ReferenceDto> GetUpdateReference(int Detailid)
        {
            return await _employeeRepository.GetUpdateReference(Detailid);
        }
        public async Task<List<EmployeeLanguageSkill>> RetrieveEmployeeLanguage(int empId, int Detailid)
        {
            return await _employeeRepository.RetrieveEmployeeLanguage(empId, Detailid);
        }

        public async Task<object> GetAccessLevelByRoleId(int? firstEntityId)
        {
            return await _employeeRepository.EmployeeCreationFilterAsync(firstEntityId);
        }
        public async Task<List<ParamRoleViewDto>> EditRoleELAsync(int linkLevel, int valueId)
        {
            return await _employeeRepository.EditRoleELAsync(linkLevel, valueId);
        }
        public async Task<UpdateResult> UpdateRoleEL(ParamRole01AND02Dto dto)
        {
            return await _employeeRepository.UpdateRoleEL(dto);
        }
        public async Task<CompanyParameterDto> EnableGeoCriteria()
        {
            return await _employeeRepository.EnableGeoCriteria();
        }
        public async Task<string> GetGeoCoordinateNameStatus(int EmployeeId)
        {
            return await _employeeRepository.GetGeoCoordinateNameStatus(EmployeeId);
        }
        public async Task<string> GetGeotaggingMasterStatus(int EmployeeId)
        {
            return await _employeeRepository.GetGeotaggingMasterStatus(EmployeeId);
        }
        public async Task<List<EmployeeDocumentListDto>> DownloadIndividualEmpDocuments(int EmployeeId)
        {
            return await _employeeRepository.DownloadIndividualEmpDocuments(EmployeeId);
        }
        public async Task<List<DocumentDetailDto>> GetDocumentDetailsAsync(string status, int detailId)
        {
            return await _employeeRepository.GetDocumentDetailsAsync(status, detailId);
        }
        public async Task<int> GetSlabEnabledAsync(int enteredBy)
        {
            return await _employeeRepository.GetSlabEnabledAsync(enteredBy);
        }
        public async Task<int> EnableNewQualif(int empId)
        {
            return await _employeeRepository.EnableNewQualif(empId);
        }


        public async Task AssignEmployeeAccessService(AssignEmployeeAccessRequestDto request)
        {
            await _employeeRepository.AssignEmployeeAccessAsync(request);
        }

        public async Task InsertWorkFlow(SaveParamWorkflowDto request)
        {
            await _employeeRepository.SaveParamWorkflow(request);
        }
        public async Task<int> InsertRoleAsync(RoleInsertDTO roleInsertDto)
        {
            return await _employeeRepository.InsertRoleAsync(roleInsertDto);
        }
        public async Task<List<RoleDetailsDTO>> GetRoleDetailsAsync(int linkId, int linkLevel)
        {
            return await _employeeRepository.GetRoleDetailsAsync(linkId, linkLevel);

        }
        //public async Task<List<object>> GetGeoCoordinatesAsync(int geoSpacingType, int geoCriteria)
        //{
        //    return await _employeeRepository.GetGeoCoordinatesAsync(geoSpacingType, geoCriteria);
        //}
        public async Task<List<object>> GetGeoSpacingCriteriaAsync()
        {
            return await _employeeRepository.GetGeoSpacingCriteria();
        }
        public async Task<List<object>> GetGeoCoordinatesTabAsync(int geoSpacingType, int geoCriteria)
        {
            return await _employeeRepository.GetGeoCoordinatesTabAsync(geoSpacingType, geoCriteria);
        }
        public async Task<string> SaveGeoLocationAsync(SaveGeoLocationRequestDTO dto)
        {
            return await _employeeRepository.SaveOrUpdateGeoLocationAsync(dto);
        }
        public async Task<IEnumerable<AssetCategoryCodeDto>> GetFilteredAssetCategoriesAsync(int varAssetTypeID)
        {
            return await _employeeRepository.GetFilteredAssetCategoriesAsync(varAssetTypeID);
        }
        public async Task<IEnumerable<AssetCategoryCodeDto>> GetAssignedOrPendingAssetCategoriesAsync(int varAssetTypeID, string varAssignAssetStatus)
        {
            return await _employeeRepository.GetAssignedOrPendingAssetCategoriesAsync(varAssetTypeID, varAssignAssetStatus);
        }
        public async Task<IEnumerable<ReasonDto>> GetGeneralSubCategoryAsync(string code)
        {
            return await _employeeRepository.GetGeneralSubCategoryAsync(code);
        }
        public async Task<string> SaveShiftMasterAccessAsync(ShiftMasterAccessInputDto request)
        {
            return await _employeeRepository.SaveShiftMasterAccessAsync(request);
        }
        public async Task<List<object>> GetLanguagesAsync()
        {
            return await _employeeRepository.GetLanguagesAsync();
        }
        public async Task<List<object>> RetrieveShiftEmpCreationAsync()
        {
            return await _employeeRepository.RetrieveShiftEmpCreationAsync();
        }
        public async Task<List<object>> FillWeekEndShiftEmpCreationAsync()
        {
            return await _employeeRepository.FillWeekEndShiftEmpCreationAsync();
        }
        public async Task<List<object>> FillbatchslabsEmpAsync(int batchid)
        {
            return await _employeeRepository.FillbatchslabsEmpAsync(batchid);
        }


        public async Task<PayscaleComponentsResponseDto> PayscaleComponentsListManual(int batchId, int employeeIds, int type)
        {
            return await _employeeRepository.PayscaleComponentsListManual(batchId, employeeIds, type);
        }
        public async Task<int> EnableBatchOptionEmpwiseAsync(int empid)
        {
            return await _employeeRepository.EnableBatchOptionEmpwiseAsync(empid);
        }
        public async Task<List<object>> GetParameterShiftInEmpAsync()
        {
            return await _employeeRepository.GetParameterShiftInEmpAsync();
        }
        public async Task<List<object>> RetrieveEmpparametersAsync(int empid)
        {
            return await _employeeRepository.RetrieveEmpparametersAsync(empid);
        }
        public async Task<List<object>> ShowEntityLinkCheckBoxAsync(int roleid)
        {
            return await _employeeRepository.ShowEntityLinkCheckBoxAsync(roleid);
        }
        public async Task<List<object>> EnableDocEditAsync()
        {
            return await _employeeRepository.EnableDocEditAsync();
        }
        public async Task<List<GeoLocationDto>> GetAccessibleGeoLocationsAsync(int roleId, int empId)
        {
            return await _employeeRepository.GetAccessibleGeoLocationsAsync(roleId, empId);
        }
        public async Task<int> CheckLiabilityPending(int empid)
        {
            return await _employeeRepository.CheckLiabilityPending(empid);
        }

        public async Task<List<DocumentsDownoaldDto>> DownloadSingleDocumentsAsync(int DetailID, string status)
        {
            return await _employeeRepository.DownloadSingleDocumentsAsync(DetailID, status);
        }
        public async Task<List<DocumentsDownoaldDto>> DownloadEmpDocumentsAsync(int DetailID, string status)
        {
            return await _employeeRepository.DownloadEmpDocumentsAsync(DetailID, status);
        }

        public async Task<List<CoordinateDto>> FillcordinateAsync(int value)
        {
            return await _employeeRepository.FillcordinateAsync(value);
        }
        public async Task<List<GeocoordinatesDto>> GetcordinatesAsync(int GeoMasterID, int GeoSpaceType)
        {
            return await _employeeRepository.GetcordinatesAsync(GeoMasterID, GeoSpaceType);
        }
        public async Task<string> UpdateEmpStatusAsync(UpdateEmployeeStatusDto employeeModuleSetupDto)
        {
            return await _employeeRepository.UpdateEmpStatusAsync(employeeModuleSetupDto);
        }


        public async Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmpRoleReporteesAsync(int SecondEntityId, int FirstEntityId, string Prefix)
        {
            return await _employeeRepository.FillEmpRoleReporteesAsync(SecondEntityId, FirstEntityId, Prefix);
        }
        public async Task<List<HrmsDocumentField00>> GetDependentFieldsAsync()
        {
            return await _employeeRepository.GetDependentFieldsAsync();
        }
        public async Task<PayscaleResultDto> GetLatestPayscaleAsync(int employeeId, int? type)
        {
            return await _employeeRepository.GetLatestPayscaleAsync(employeeId, type);
        }

        public async Task<int> GetlastEntityByRoleId(int roleId, int EntityLimit)
        {
            return await _employeeRepository.GetlastEntityByRoleId(roleId, EntityLimit);
        }
    }
}
