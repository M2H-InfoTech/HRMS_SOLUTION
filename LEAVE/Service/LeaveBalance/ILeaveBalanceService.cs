using LEAVE.Dto;

namespace LEAVE.Service.LeaveBalance
{
    public interface ILeaveBalanceService
    {
        Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID);
    }
}
