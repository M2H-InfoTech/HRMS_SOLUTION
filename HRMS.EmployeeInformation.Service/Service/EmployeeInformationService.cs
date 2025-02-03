using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.DTO.DTOs.Documents;
using HRMS.EmployeeInformation.Repository.Common;
using HRMS.EmployeeInformation.Service.Interface;
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

        public async Task<CommunicationResultDto> Communication(int employeeId)
        {
            return await _employeeRepository.Communication(employeeId);
        }

        public async Task<List<CommunicationEmergencyDto>> CommunicationEmergency(int employeeId)
        {
            return await _employeeRepository.CommunicationEmergency(employeeId);
        }

        public async Task<CommunicationExtraDto> CommunicationExtra(int employeeId)
        {
            return await _employeeRepository.CommunicationExtra(employeeId);
        }

        public async Task<EmployeeStatusResultDto> EmployeeStatus(int employeeId, string parameterCode, string type)
        {
            return await _employeeRepository.EmployeeStatus(employeeId, parameterCode, type);
        }

        public async Task<PaginatedResult<EmployeeResultDto>> GetEmpData(EmployeeInformationParameters employeeInformationParameters)
        {
            return await _employeeRepository.GetEmpData(employeeInformationParameters);
        }

        public async Task<List<LanguageSkillResultDto>> LanguageSkill(int employeeId)
        {
            return await _employeeRepository.LanguageSkill(employeeId);
        }

        public async Task<List<string>> HobbiesData(int employeeId)
        {
            return await _employeeRepository.HobbiesData(employeeId);
        }

        public async Task<List<RewardAndRecognitionDto>> RewardAndRecognition(int employeeId)
        {
            return await _employeeRepository.RewardAndRecognitions(employeeId);
        }

        public async Task<List<QualificationDto>> Qualification(int employeeId)
        {
            return await _employeeRepository.Qualification(employeeId);
        }

        public async Task<List<SkillSetDto>> SkillSets(int employeeId)
        {
            return await _employeeRepository.SkillSets(employeeId);
        }
        public async Task<List<AllDocumentsDto>> Documents(int employeeId, List<string> excludedDocTypes)
        {
            return await _employeeRepository.Documents(employeeId, excludedDocTypes);
        }
        //public async Task<List<AllDocumentsDto>> BankDetails(int employeeId)
        //{
        //    return await _employeeRepository.BankDetails(employeeId);
        //}
        public async Task<List<DependentDto>> Dependent(int employeeId)
        {
            return await _employeeRepository.Dependent(employeeId);
        }

        public async Task<List<CertificationDto>> Certification(int employeeId)
        {
            return await _employeeRepository.Certification(employeeId);
        }
        public async Task<List<DisciplinaryActionsDto>> DisciplinaryActions(int employeeId)
        {
            return await _employeeRepository.DisciplinaryActions(employeeId);
        }

        public async Task<List<LetterDto>> Letter(int employeeId)
        {
            return await _employeeRepository.Letter(employeeId);
        }
        public async Task<List<ReferenceDto>> Reference(int employeeId)
        {
            return await _employeeRepository.Reference(employeeId);
        }
        public async Task<List<ProfessionalDto>> Professional(int employeeId)
        {
            return await _employeeRepository.Professional(employeeId);
        }
        public async Task<List<AssetDto>> Asset()
        {
            return await _employeeRepository.Asset();
        }
        public async Task<List<AssetDetailsDto>> AssetDetails(int employeeId)
        {
            return await _employeeRepository.AssetDetails(employeeId);
        }

        public async Task<List<CurrencyDropdown_ProfessionalDto>> CurrencyDropdown_Professional()
        {
            return await _employeeRepository.CurrencyDropdown_Professional();
        }
        public async Task<HrEmpProfdtlsApprlDto> InsertOrUpdateProfessionalData(HrEmpProfdtlsApprlDto profdtlsApprlDto)
        {
            return await _employeeRepository.InsertOrUpdateProfessionalData(profdtlsApprlDto);
        }
        public async Task<PersonalDetailsHistoryDto> InsertOrUpdatePersonalData(PersonalDetailsHistoryDto persnldtlsApprlDto)
        {
            return await _employeeRepository.InsertOrUpdatePersonalData(persnldtlsApprlDto);
        }
        public async Task<List<HrEmpProfdtlsApprlDto>> GetProfessionalByIdAsync(string updateType, int detailID, int empID)
        {
            return await _employeeRepository.GetProfessionalByIdAsync(updateType, detailID, empID);
        }

        public async Task <List<PersonalDetailsDto>> GetPersonalDetailsById(int employeeid)
        {
            return await _employeeRepository.GetPersonalDetailsById(employeeid);
        }
        public async Task<List<TrainingDto>> Training(int employeeid)
        {
            return await _employeeRepository.Training(employeeid);
        }

        public async Task<List<CareerHistoryDto>> CareerHistory(int employeeid)
        {
            return await _employeeRepository.CareerHistory(employeeid);
        }
        public async Task<List<object>> BiometricDetails(int employeeid)
        {
            return await _employeeRepository.BiometricDetails(employeeid);
        }


    }

}
