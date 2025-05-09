using LEAVE.Dto;

namespace LEAVE.Repository.AssignLeave
{
    public interface IAssignLeaveRepository
    {
        Task<int> GetconfirmBsInsert(GetconfirmBsInsert GetconfirmBsInsert);
        Task<List<BsemployeedataDto>> Bsemployeedata(int employeeId);
        Task<List<FillchildBSdetailsDto>> FillchildBSdetails(int employeeId);
        Task<object> Getallbasics(string linkid, int levelid, string transaction, int empid);
        Task<Object> GetBasicAssignmentAsync (int roleId, int entryBy);
        Task<bool> DeleteSingleEmpBasicSettingAsync (int leavemasters, int empid);
        Task<int> AssignBasicsAsync (LeaveAssignSaveDto Dto);
    }
}
