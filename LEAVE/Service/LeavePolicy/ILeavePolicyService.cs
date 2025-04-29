using HRMS.EmployeeInformation.DTO.DTOs;

namespace LEAVE.Service.LeavePolicy
{
    public interface ILeavePolicyService
    {
        Task<List<object>> FillLeavepolicyAsync(int SecondEntityId, int EmpId);
        Task<int?> CreatepolicyAsync(CreatePolicyDto createPolicyDto);
        Task<List<object>> FillleaveAsync(int SecondEntityId, int EmpId);
        Task<List<object>> FillInstatntLimitAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID);
    }
}
