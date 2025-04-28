using LEAVE.Dto;

namespace LEAVE.Repository.LeaveMaster
{
    public interface ILeaveMasterRepository
    {
        Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId);

    }
}
