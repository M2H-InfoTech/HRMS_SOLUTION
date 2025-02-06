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

        public async Task<List<PersonalDetailsDto>> GetPersonalDetailsById(int employeeid)
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

        public async Task<List<object>> AccessDetails(int employeeid)
        {
            return await _employeeRepository.AccessDetails(employeeid);
        }
        public async Task<List<Fill_ModulesWorkFlowDto>> Fill_ModulesWorkFlow(int entityID, int linkId)
        {
            return await _employeeRepository.Fill_ModulesWorkFlow(entityID, linkId);
        }
        public async Task<List<Fill_WorkFlowMasterDto>> Fill_WorkFlowMaster(int emp_Id, int roleId)
        {
            return await _employeeRepository.Fill_WorkFlowMaster(emp_Id, roleId);
        }
        public async Task<List<BindWorkFlowMasterEmpDto>> BindWorkFlowMasterEmp(int linkId, int linkLevel)
        {
            return await _employeeRepository.BindWorkFlowMasterEmp(linkId, linkLevel);
        }
        public async Task<List<GetRejoinReportDto>> GetRejoinReport(int employeeId)
        {
            return await _employeeRepository.GetRejoinReport(employeeId);
        }
        public async Task<List<GetEmpReportingReportDto>> GetEmpReportingReport(int employeeId)
        {
            return await _employeeRepository.GetEmpReportingReport(employeeId);
        }


        public async Task<List<TransferAndPromotionDto>> TransferAndPromotion(int employeeid)
        {
            return await _employeeRepository.TransferAndPromotion(employeeid);
        }
        public async Task<List<SalarySeriesDto>> SalarySeries(int employeeid, string status)
        {
            return await _employeeRepository.SalarySeries(employeeid, status);
        }
        public async Task<List<AuditInformationDto>> AuditInformation(string employeeIDs, int empId, int roleId, string? infotype, string? infoDesc, string? datefrom, string? dateto)
        {
            return await _employeeRepository.AuditInformation(employeeIDs, empId, roleId, infotype, infoDesc, datefrom, dateto);
        }


        public async Task<List<GetEmpWorkFlowRoleDetailstDto>> GetEmpWorkFlowRoleDetails(int linkId, int linkLevel)
        {
            return await _employeeRepository.GetEmpWorkFlowRoleDetails(linkId, linkLevel);
        }

        public async Task<List<FillEmpWorkFlowRoleDto>> FillEmpWorkFlowRole(int entityID)
        {
            return await _employeeRepository.FillEmpWorkFlowRole(entityID);
        }
        public async Task<List<EmployeeHraDto>> HraDetails(int employeeId)
        {
            return await _employeeRepository.HraDetails(employeeId);
        }

        public async Task<List<object>> EmployeeType(int employeeid)
        {
            return await _employeeRepository.EmployeeType(employeeid);
        }

        public async Task<List<object>> GeoSpacingTypeAndCriteria(string type)
        {
            return await _employeeRepository.GeoSpacingTypeAndCriteria(type);
        }

        public async Task<List<GeoSpacingDto>> GetGeoSpacing(int employeeid)
        {
            return await _employeeRepository.GetGeoSpacing(employeeid);
        }
        public async Task<List<FillEmployeesBasedOnwWorkflowDto>> FillEmployeesBasedOnwWorkflow (int firstEntityId, int secondEntityId)
            {
            return await _employeeRepository.FillEmployeesBasedOnwWorkflow (firstEntityId, secondEntityId);
            }
        public async Task<List<FillCountryDto>> FillCountry ( )
            {
            return await _employeeRepository.FillCountry ( );
            }
        public async Task<List<object>> GetBloodGroup ()
            {
            return await _employeeRepository.GetBloodGroup ( );
            }
        public async Task<List<object>> FillReligion ( )
            {
            return await _employeeRepository.FillReligion ( );
            }

        }
    }
