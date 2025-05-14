using LEAVE.Dto;
using LEAVE.Repository.LeavePolicy;
using static LEAVE.Repository.LeavePolicy.LeavePolicyRepository;

namespace LEAVE.Service.LeavePolicy
{
    public class LeavePolicyService : ILeavePolicyService
    {
        private readonly ILeavePolicyRepository _leavePolicyRepository;
        public LeavePolicyService(ILeavePolicyRepository leavePolicyRepository)
        {
            _leavePolicyRepository = leavePolicyRepository;
        }
        public Task<List<object>> FillLeavepolicyAsync(int SecondEntityId, int EmpId)
        {
            return _leavePolicyRepository.FillLeavepolicyAsync(SecondEntityId, EmpId);
        }
        public async Task<int?> CreatepolicyAsync(CreatePolicyDto createPolicyDto)
        {
            return await _leavePolicyRepository.CreatepolicyAsync(createPolicyDto);
        }
        public Task<List<object>> FillleaveAsync(int SecondEntityId, int EmpId)
        {
            return _leavePolicyRepository.FillleaveAsync(SecondEntityId, EmpId);
        }
        public Task<List<object>> FillInstatntLimitAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {
            return _leavePolicyRepository.FillInstatntLimitAsync(LeavePolicyMasterID, LeavePolicyInstanceLimitID);
        }
        public Task<List<object>> EditFillInstatntLimitLeaveAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {
            return _leavePolicyRepository.EditFillInstatntLimitLeaveAsync(LeavePolicyMasterID, LeavePolicyInstanceLimitID);
        }

        public Task<List<object>> fillweekendinclude(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {
            return _leavePolicyRepository.fillweekendinclude(LeavePolicyMasterID, LeavePolicyInstanceLimitID);
        }
        public Task<List<object>> FillHolidayincludeAsync(int LeavePolicyMasterID, int LeavePolicyInstanceLimitID)
        {
            return _leavePolicyRepository.FillHolidayincludeAsync(LeavePolicyMasterID, LeavePolicyInstanceLimitID);
        }

        public async Task<string> InsertInstanceLeaveLimitAsync(LeavePolicyInstanceLimitDto leavePolicyInstanceLimitDto, string compLeaveIDs, int empId)
        {
            return await _leavePolicyRepository.InsertInstanceLeaveLimitAsync(leavePolicyInstanceLimitDto, compLeaveIDs, empId);
        }
        public async Task<string> DeleteInstanceLimit(int LeavePolicyInstanceLimitID)
        {
            return await _leavePolicyRepository.DeleteInstanceLimit(LeavePolicyInstanceLimitID);
        }

        public async Task<object> LeavePolicySettingsAttachment(int leavePolicyMasterId, int leavePolicyInstanceLimitId)
        {
            return await _leavePolicyRepository.LeavePolicySettingsAttachment(leavePolicyMasterId, leavePolicyInstanceLimitId);
        }

        public async Task<List<LeaveOptionDto>> LeavePolicySettingsUnpaidLeaves(int secondEntityId, int leaveId)
        {
            return await _leavePolicyRepository.LeavePolicySettingsUnpaidLeaves(secondEntityId, leaveId);
        }
    }
}
