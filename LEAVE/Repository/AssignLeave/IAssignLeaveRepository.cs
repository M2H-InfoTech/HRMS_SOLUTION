using LEAVE.Dto;

namespace LEAVE.Repository.AssignLeave
{
    public interface IAssignLeaveRepository
    {
        Task<int> GetconfirmBsInsert(GetconfirmBsInsert GetconfirmBsInsert);
        Task<List<BsemployeedataDto>> Bsemployeedata(int employeeId);
        Task<List<FillchildBSdetailsDto>> FillchildBSdetails(int employeeId);
        Task<object> Getallbasics(string linkid, int levelid, string transaction, int empid);
    }
}
