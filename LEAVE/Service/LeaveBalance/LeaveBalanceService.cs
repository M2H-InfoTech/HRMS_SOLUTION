using LEAVE.Dto;
using LEAVE.Repository.LeaveBalance;

namespace LEAVE.Service.LeaveBalance
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        public LeaveBalanceService(ILeaveBalanceRepository leaveBalanceRepository)
        {

            _leaveBalanceRepository = leaveBalanceRepository;
        }
        public Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID)
        {
            return _leaveBalanceRepository.RetrieveBranchDetails(instID, branchID);
        }
        public Task<List<LeaveApplicationDto>> GetLeaveApplicationsAsync(int employeeId, int leaveId, string approvalStatus, string? flowStatus, DateTime? leaveFrom, DateTime? leaveTo)
        {
            return _leaveBalanceRepository.GetLeaveApplicationsAsync(employeeId, leaveId, approvalStatus, flowStatus, leaveFrom, leaveTo);
        }

    }

}
