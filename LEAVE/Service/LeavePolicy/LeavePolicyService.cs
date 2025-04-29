using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Repository.LeavePolicy;

namespace LEAVE.Service.LeavePolicy
{
    public class LeavePolicyService : ILeavePolicyService
    {

        private readonly ILeavePolicyRepository _leavePolicyRepository  ;
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

    }
}
