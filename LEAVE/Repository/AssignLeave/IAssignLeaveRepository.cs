using LEAVE.Dto;

namespace LEAVE.Repository.AssignLeave
{
    public interface IAssignLeaveRepository
    {
        Task<int> GetconfirmBsInsert(GetconfirmBsInsert GetconfirmBsInsert);
    }
}
