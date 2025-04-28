
using LEAVE.Dto;
using LEAVE.Repository.LeaveMaster;

namespace LEAVE.Service.LeaveMaster
{
    public class LeaveMasterService : ILeaveMasterService
    {
        private readonly ILeaveMasterRepository _leaveMasterRepository;
        public LeaveMasterService(ILeaveMasterRepository leaveMasterRepository)
        {
            _leaveMasterRepository = leaveMasterRepository;
        }

        public Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId)
        {
            return _leaveMasterRepository.FillLeaveMasterAsync(secondEntityId, empId);
        }
    }
}
