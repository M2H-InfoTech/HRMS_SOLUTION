using LEAVE.Dto;

namespace LEAVE.Service.LeaveMaster
{
    public interface ILeaveMasterService
    {
        Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId);
    }
}
