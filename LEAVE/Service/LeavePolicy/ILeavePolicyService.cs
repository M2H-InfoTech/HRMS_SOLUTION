using LEAVE.Dto;

namespace LEAVE.Service.LeavePolicy
{
    public interface ILeavePolicyService
    {
        Task<List<object>> FillLeavepolicyAsync(int SecondEntityId, int EmpId);
        Task<int?> CreatepolicyAsync(CreatePolicyDto createPolicyDto);
        Task<List<object>> FillleaveAsync(int SecondEntityId, int EmpId);
        Task<List<object>> FillInstatntLimitAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID);
        Task<List<object>> EditFillInstatntLimitLeaveAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID);
        Task<List<object>> fillweekendinclude(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID);
        Task<List<object>> FillHolidayincludeAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID);

        Task<string> InsertInstanceLeaveLimitAsync(LeavePolicyInstanceLimitDto leavePolicyInstanceLimitDto, string compLeaveIDs, int empId);

        Task<string> DeleteInstanceLimit(int LeavePolicyInstanceLimitID);
        Task<object> LeavePolicySettingsAttachment(int leavePolicyMasterId, int leavePolicyInstanceLimitId);

    }
}
