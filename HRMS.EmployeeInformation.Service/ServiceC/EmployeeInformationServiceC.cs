using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Repository.Common.RepositoryC;
using HRMS.EmployeeInformation.Service.InterfaceC;

namespace HRMS.EmployeeInformation.Service.ServiceC
{
    public class EmployeeInformationServiceC : IEmployeeInformationServiceC
    {
        private readonly IRepositoryC _repositoryC;
        public EmployeeInformationServiceC(IRepositoryC repositoryC)
        {
            _repositoryC = repositoryC;
        }
        public async Task<FillTravelTypeDto> FillTravelType()
        {
            return await _repositoryC.FillTravelType();
        }
        public async Task<List<FillEmployeesBasedOnRoleDto>> FillEmployeesBasedOnRole(int firstEntityId, int secondEntityId, string transactionType)
        {
            return await _repositoryC.FillEmployeesBasedOnRole(firstEntityId, secondEntityId, transactionType);
        }
        public async Task<GetDependentDetailsDto> GetDependentDetails(int employeeId)
        {
            return await _repositoryC.GetDependentDetails(employeeId);
        }
        public async Task<int> SaveDependentEmp(SaveDependentEmpDto SaveDependentEmp)
        {
            return await _repositoryC.SaveDependentEmp(SaveDependentEmp);
        }
        public async Task<object> RetrieveEducation()
        {
            return await _repositoryC.RetrieveEducation();
        }
        public async Task<object> RetrieveCourse()
        {
            return await _repositoryC.RetrieveCourse();
        }
        public async Task<object> RetrieveSpecial()
        {
            return await _repositoryC.RetrieveSpecial();
        }
        public async Task<object> RetrieveUniversity()
        {
            return await _repositoryC.RetrieveUniversity();
        }
        public async Task<List<EditDependentEmpDto>> EditDependentEmp (int Schemeid)
            {
            return await _repositoryC.EditDependentEmp (Schemeid);
            }
        public async Task<List<WorkFlowAvailabilityDto>> WorkFlowAvailability (int Emp_Id, string Transactiontype, int ParameterID)
            {
            return await _repositoryC.WorkFlowAvailability (Emp_Id, Transactiontype, ParameterID);
            }
        public async Task<string> InsertDepFields (List<TmpDocFileUpDto> InsertDepFields)
            {
            return await _repositoryC.InsertDepFields (InsertDepFields);
            }
        }
}
