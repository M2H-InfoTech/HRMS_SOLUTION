using LEAVE.Dto;

namespace LEAVE.Repository.LeaveBalance
{
    public interface ILeaveBalanceRepository
    {
        Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID);
        Task<List<LeaveApplicationDto>> GetLeaveApplicationsAsync(int employeeId, int leaveId, string approvalStatus, string? flowStatus, DateTime? leaveFrom, DateTime? leaveTo);

    }
}
