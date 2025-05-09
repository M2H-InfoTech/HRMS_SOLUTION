using LEAVE.Dto;

namespace LEAVE.Service.AssignLeave
{
    public interface IAssignLeaveService
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
