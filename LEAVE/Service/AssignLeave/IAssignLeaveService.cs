using LEAVE.Dto;

namespace LEAVE.Service.AssignLeave
{
    public interface IAssignLeaveService
    {
        Task<int> GetconfirmBsInsert(GetconfirmBsInsert GetconfirmBsInsert);
      
    }
}
