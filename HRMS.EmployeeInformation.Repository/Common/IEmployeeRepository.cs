using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Repository.Common
{
    public interface IEmployeeRepository
    {
        //Task<PaginatedResult<EmployeeResultDto>> GetEmpData(EmployeeInformationParameters employeeInformationParameters);

        Task<PaginatedResult<EmployeeResultDto>> GetEmpData(EmployeeInformationParameters employeeInformationParameters);

        Task<EmployeeStatusResultDto> EmployeeStatus(int employeeId, string parameterCode, string type);

        Task<List<LanguageSkillResultDto>> LanguageSkill(int employeeId);

        Task<CommunicationResultDto> Communication(int employeeId);

        Task<CommunicationExtraDto> CommunicationExtra(int employeeId);

        Task<List<CommunicationEmergencyDto>> CommunicationEmergency(int employeeId);

        Task<List<string>> HobbiesData(int employeeId);

        Task<List<RewardAndRecognitionDto>> RewardAndRecognitions(int employeeId);
        Task<List<QualificationDto>> Qualification(int employeeId);

        Task<List<SkillSetDto>> SkillSets(int employeeId);

        Task<List<AllDocumentsDto>> Documents(int employeeId, List<string> excludedDocTypes);

        //Task<List<AllDocumentsDto>> BankDetails(int employeeId);

        Task<List<DependentDto>> Dependent(int employeeId);
        Task<List<CertificationDto>> Certification(int employeeId);
        Task<List<DisciplinaryActionsDto>> DisciplinaryActions(int employeeId);
        Task<List<LetterDto>> Letter(int employeeId);
        Task<List<ReferenceDto>> Reference(int employeeId);
        Task<List<ProfessionalDto>> Professional(int employeeId);
        Task<List<AssetDto>> Asset();
        Task<List<AssetDetailsDto>> AssetDetails(int employeeId);
        Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdown_Professional();
        Task<HrEmpProfdtlsApprlDto> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto);
        Task<PersonalDetailsHistoryDto> InsertOrUpdatePersonalData(PersonalDetailsHistoryDto persnldtlsApprlDto);

        Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID);
        Task<List<PersonalDetailsDto>> GetPersonalDetailsById(int employeeid);
        Task<List<TrainingDto>> Training(int employeeid);
        Task<List<CareerHistoryDto>> CareerHistory(int employeeid);
        Task<List<object>> BiometricDetails(int employeeId);
        Task<List<object>> AccessDetails(int employeeid);
        Task<List<TransferAndPromotionDto>> TransferAndPromotion(int employeeId);
        Task<List<Fill_ModulesWorkFlowDto>> Fill_ModulesWorkFlow(int entityID, int linkId);
        Task<List<Fill_WorkFlowMasterDto>> Fill_WorkFlowMaster(int emp_Id, int roleId);
        Task<List<BindWorkFlowMasterEmpDto>> BindWorkFlowMasterEmp(int linkId, int linkLevel);
        Task<List<SalarySeriesDto>> SalarySeries(int employeeid, string status);
        Task<List<GetRejoinReportDto>> GetRejoinReport(int employeeId);
        Task<List<GetEmpReportingReportDto>> GetEmpReportingReport(int employeeId);
        Task<List<GetEmpWorkFlowRoleDetailstDto>> GetEmpWorkFlowRoleDetails(int linkId, int linkLevel);
        Task<List<FillEmpWorkFlowRoleDto>> FillEmpWorkFlowRole(int entityID);

        Task<List<AuditInformationDto>> AuditInformation(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto);

        Task<List<object>> EmployeeType(int employeeid);
        Task<List<object>> GeoSpacingTypeAndCriteria(string type);
        Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmployeesBasedOnwWorkflow (int firstEntityId, int secondEntityId); 

        Task<List<GeoSpacingDto>> GetGeoSpacing(int employeeid);
        Task<List<EmployeeHraDto>> HraDetails(int employeeId);
        Task<List<FillCountryDto>> FillCountry ( );
        Task<List<object>> GetBloodGroup ( );
        Task<List<object>> FillReligion ( );


        }
}
