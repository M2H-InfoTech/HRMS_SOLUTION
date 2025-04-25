
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
        public Task<int?> FillLeaveMasterAsync(int SecondEntityId, int EmpId)
        {
            return _leaveMasterRepository.FillLeaveMasterAsync(SecondEntityId, EmpId);
        }
    }
}
