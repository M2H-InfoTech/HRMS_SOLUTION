using LEAVE.Dto;
using LEAVE.Repository.AssignLeave;

namespace LEAVE.Service.AssignLeave
{
    public class AssignLeaveService : IAssignLeaveService
    {
        private readonly IAssignLeaveRepository _assignLeaveRepository;
        public AssignLeaveService(IAssignLeaveRepository assignLeaveRepository)
        {
            _assignLeaveRepository = assignLeaveRepository;
        }
        public async Task<int> GetconfirmBsInsert(GetconfirmBsInsert GetconfirmBsInsert)
        {
            return await _assignLeaveRepository.GetconfirmBsInsert(GetconfirmBsInsert);
        }
    }
}
