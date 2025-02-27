using HRMS.EmployeeInformation.DTO.DTOs;

namespace HRMS.EmployeeInformation.Service.InterfaceC
{
    public interface IEmployeeInformationServiceC
    {
        Task<FillTravelTypeDto> FillTravelType();
        Task<List<FillEmployeesBasedOnRoleDto>> FillEmployeesBasedOnRole(int firstEntityId, int secondEntityId, string transactionType);
        Task<GetDependentDetailsDto> GetDependentDetails(int employeeId);
        Task<int> SaveDependentEmp(SaveDependentEmpDto SaveDependentEmp);
        Task<object> RetrieveEducation();
        Task<object> RetrieveCourse();
        Task<object> RetrieveSpecial();
        Task<object> RetrieveUniversity();
        Task<List<EditDependentEmpDto>> EditDependentEmp (int Schemeid);
        Task<List<WorkFlowAvailabilityDto>> WorkFlowAvailability (int Emp_Id, string Transactiontype, int ParameterID);
        Task<string> InsertDepFields (List<TmpDocFileUpDto> InsertDepFields);

        }
    }
