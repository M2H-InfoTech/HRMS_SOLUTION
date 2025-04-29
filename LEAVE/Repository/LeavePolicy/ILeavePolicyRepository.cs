using HRMS.EmployeeInformation.DTO.DTOs;

namespace LEAVE.Repository.LeavePolicy
{
    public interface ILeavePolicyRepository
    {


        Task<List<object>> FillLeavepolicyAsync(int SecondEntityId, int EmpId);
        Task<int?> CreatepolicyAsync(CreatePolicyDto createPolicyDto);
        Task<List<object>> FillleaveAsync(int SecondEntityId, int EmpId);
        Task<List<object>> FillInstatntLimitAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID);
    }
}
