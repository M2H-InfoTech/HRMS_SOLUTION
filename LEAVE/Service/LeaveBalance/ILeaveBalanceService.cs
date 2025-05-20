using LEAVE.Dto;

namespace LEAVE.Service.LeaveBalance
{
    public interface ILeaveBalanceService
    {
        Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID);
        Task<List<LeaveApplicationDto>> GetLeaveApplicationsAsync(int employeeId, int leaveId, string approvalStatus, string? flowStatus, DateTime? leaveFrom, DateTime? leaveTo);


    }
}
