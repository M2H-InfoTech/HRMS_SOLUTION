

using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Repository.Common;
using MPLOYEE_INFORMATION.DTO.DTOs;

namespace HRMS.EmployeeInformation.Service.Interface
{
    public interface IEmployeeInformationService
    {
        Task<PaginatedResult<EmployeeResultDto>> GetEmpData(EmployeeInformationParameters employeeInformationParameters);
        Task<EmployeeStatusResultDto> EmployeeStatus(int employeeId, string parameterCode, string type);
        Task<List<LanguageSkillResultDto>> LanguageSkill(int employeeId);
        Task<CommunicationResultDto> Communication(int employeeId);
        Task<CommunicationExtraDto> CommunicationExtra(int employeeId); 
        Task<List<CommunicationEmergencyDto>> CommunicationEmergency(int employeeId);
        Task<List<string>> HobbiesData(int employeeId);
        Task<List<RewardAndRecognitionDto>> RewardAndRecognition(int employeeId);
        Task<List<QualificationDto>> Qualification(int employeeId);
        Task<List<SkillSetDto>> SkillSets(int employeeId);
        Task<List<DocumentsDto>> Documents(int employeeId);
        Task<List<DependentDto>> Dependent(int employeeId);
        Task<List<CertificationDto>> Certification(int employeeId);
        Task<List<DisciplinaryActionsDto>> DisciplinaryActions(int employeeId);
        Task<List<LetterDto>> Letter(int employeeId);
        Task<List<ReferenceDto>> Reference(int employeeId);
        Task<List<ProfessionalDto>> Professional(int employeeId);
        Task<List<AssetDto>> Asset();
        Task<List<AssetDetailsDto>> AssetDetails(int employeeId);
        Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdown_Professional();


        //Task<List<CareerHistoryDto>> CareerHistory(int employeeId);
        Task<HrEmpProfdtlsApprlDto> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto);
        Task<PersonalDetailsHistoryDto> InsertOrUpdatePersonalData(PersonalDetailsHistoryDto persnldtlsApprlDto);

        Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID);
        Task<List<PersonalDetailsDto>> GetPersonalDetailsById(int employeeid);
        Task<List<TrainingDto>> Training(int employeeid);
    }

}
