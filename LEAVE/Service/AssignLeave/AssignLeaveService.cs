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
        public Task<List<BsemployeedataDto>> Bsemployeedata(int employeeId)
        {
            return _assignLeaveRepository.Bsemployeedata(employeeId);
        }
        public Task<List<FillchildBSdetailsDto>> FillchildBSdetails(int employeeId)
        {
            return _assignLeaveRepository.FillchildBSdetails(employeeId);
        }
        public Task<object> Getallbasics(string linkid, int levelid, string transaction, int empid)
        {
            return _assignLeaveRepository.Getallbasics(linkid, levelid, transaction, empid);
        }
    }
}
