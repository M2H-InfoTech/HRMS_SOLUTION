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
            return await _employeeRepository.CommunicationAsync(employeeId);
        }

        public async Task<List<CommunicationEmergencyDto>> CommunicationEmergency(int employeeId)
        {
            return await _employeeRepository.CommunicationEmergencyAsync(employeeId);
        }

        public async Task<List<CommunicationExtraDto>> CommunicationExtra(int employeeId)
        {
            return await _employeeRepository.CommunicationExtraAsync(employeeId);
        }

        public async Task<EmployeeStatusResultDto> EmployeeStatus(int employeeId, string parameterCode, string type)
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

        public async Task<List<string>> HobbiesData(int employeeId)
        {
            return await _employeeRepository.HobbiesDataAsync(employeeId);
        }

        public async Task<List<RewardAndRecognitionDto>> RewardAndRecognition(int employeeId)
        {
            return await _employeeRepository.RewardAndRecognitionsAsync(employeeId);
        }

        public async Task<List<QualificationDto>> Qualification(int employeeId)
        {
            return await _employeeRepository.QualificationAsync(employeeId);
        }

        public async Task<List<SkillSetDto>> SkillSets(int employeeId)
        {
            return await _employeeRepository.SkillSetsAsync(employeeId);
        }
        public async Task<List<AllDocumentsDto>> Documents(int employeeId, List<string> excludedDocTypes)
        {
            return await _employeeRepository.DocumentsAsync(employeeId, excludedDocTypes);
        }
        //public async Task<List<AllDocumentsDto>> BankDetails(int employeeId)
        //{
        //    return await _employeeRepository.BankDetails(employeeId);
        //}
        public async Task<List<DependentDto>> Dependent(int employeeId)
        {
            return await _employeeRepository.DependentAsync(employeeId);
        }

        public async Task<List<CertificationDto>> Certification(int employeeId)
        {
            return await _employeeRepository.CertificationAsync(employeeId);
        }
        public async Task<List<DisciplinaryActionsDto>> DisciplinaryActions(int employeeId)
        {
            return await _employeeRepository.DisciplinaryActionsAsync(employeeId);
        }

        public async Task<List<LetterDto>> Letter(int employeeId)
        {
            return await _employeeRepository.LetterAsync(employeeId);
        }
        public async Task<List<ReferenceDto>> Reference(int employeeId)
        {
            return await _employeeRepository.ReferenceAsync(employeeId);
        }
        public async Task<List<ProfessionalDto>> Professional(int employeeId)
        {
            return await _employeeRepository.ProfessionalAsync(employeeId);
        }
        public async Task<List<AssetDto>> Asset()
        {
            return await _employeeRepository.AsseAsynct();
        }
        public async Task<List<AssetDetailsDto>> AssetDetails(int employeeId)
        {
            return await _employeeRepository.AssetDetailsAsync(employeeId);
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

        public async Task<List<PersonalDetailsDto>> GetPersonalDetailsById(int employeeid)
        {
            return await _employeeRepository.GetPersonalDetailsByIdAsync(employeeid);
        }
        public async Task<List<TrainingDto>> Training(int employeeid)
        {
            return await _employeeRepository.TrainingAsync(employeeid);
        }

        public async Task<List<CareerHistoryDto>> CareerHistory(int employeeid)
        {
            return await _employeeRepository.CareerHistoryAsync(employeeid);
        }
        public async Task<List<object>> BiometricDetails(int employeeid)
        {
            return await _employeeRepository.BiometricDetailsAsync(employeeid);
        }
        public async Task<object> AccessDetails(int employeeid)
        {
            return await _employeeRepository.AccessDetailsAsync(employeeid);
        }
        //public async Task<List<object>> AccessDetails(int employeeid)
        //{
        //    return await _employeeRepository.AccessDetails(employeeid);
        //}
        public async Task<List<Fill_ModulesWorkFlowDto>> Fill_ModulesWorkFlow(int entityID, int linkId)
        {
            return await _employeeRepository.Fill_ModulesWorkFlowAsync(entityID, linkId);
        }
        public async Task<List<Fill_WorkFlowMasterDto>> Fill_WorkFlowMaster(int emp_Id, int roleId)
        {
            return await _employeeRepository.Fill_WorkFlowMasterAsync(emp_Id, roleId);
        }
        public async Task<List<BindWorkFlowMasterEmpDto>> BindWorkFlowMasterEmp(int linkId, int linkLevel)
        {
            return await _employeeRepository.BindWorkFlowMasterEmpAsync(linkId, linkLevel);
        }
        public async Task<List<GetRejoinReportDto>> GetRejoinReportAsync(int employeeId)
        {
            return await _employeeRepository.GetRejoinReportAsync(employeeId);
        }
        public async Task<List<GetEmpReportingReportDto>> GetEmpReportingReport(int employeeId)
        {
            return await _employeeRepository.GetEmpReportingReportAsync(employeeId);
        }


        public async Task<List<TransferAndPromotionDto>> TransferAndPromotion(int employeeid)
        {
            return await _employeeRepository.TransferAndPromotionAsync(employeeid);
        }
        public async Task<List<SalarySeriesDto>> SalarySeries(int employeeid, string status)
        {
            return await _employeeRepository.SalarySeriesAsync(employeeid, status);
        }
        public async Task<List<AuditInformationDto>> AuditInformation(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto)
        {
            return await _employeeRepository.AuditInformationAsync(employeeIDs, empId, roleId, infotype, infoDesc, datefrom, dateto);
        }


        public async Task<List<GetEmpWorkFlowRoleDetailstDto>> GetEmpWorkFlowRoleDetails(int linkId, int linkLevel)
        {
            return await _employeeRepository.GetEmpWorkFlowRoleDetailsAsync(linkId, linkLevel);
        }

        public async Task<List<FillEmpWorkFlowRoleDto>> FillEmpWorkFlowRole(int entityID)
        {
            return await _employeeRepository.FillEmpWorkFlowRoleAsync(entityID);
        }
        public async Task<List<EmployeeHraDto>> HraDetails(int employeeId)
        {
            return await _employeeRepository.HraDetailsAsync(employeeId);
        }

        public async Task<List<object>> EmployeeType(int employeeid)
        {
            return await _employeeRepository.EmployeeTypeAsync(employeeid);
        }

        public async Task<List<object>> GeoSpacingTypeAndCriteria(string type)
        {
            return await _employeeRepository.GeoSpacingTypeAndCriteriaAsync(type);
        }

        public async Task<List<GeoSpacingDto>> GetGeoSpacing(int employeeid)
        {
            return await _employeeRepository.GetGeoSpacingAsync(employeeid);
        }
        public async Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmployeesBasedOnwWorkflow(int firstEntityId, int secondEntityId)
        {
            return await _employeeRepository.FillEmployeesBasedOnwWorkflowAsync(firstEntityId, secondEntityId);
        }

    }
}
