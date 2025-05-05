using LEAVE.Dto;

namespace LEAVE.Repository.LeaveBalance
{
    public interface ILeaveBalanceRepository
    {
        Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID);
    }
}
