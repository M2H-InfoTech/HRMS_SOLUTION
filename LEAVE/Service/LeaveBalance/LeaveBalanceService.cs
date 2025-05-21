using HRMS.EmployeeInformation.DTO.DTOs;
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
        public async Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID)
        {
            return await _leaveBalanceRepository.RetrieveBranchDetails(instID, branchID);
        }
        public async Task<List<LeaveApplicationDto>> GetLeaveApplicationsAsync(int employeeId, int leaveId, string approvalStatus, string? flowStatus, DateTime? leaveFrom, DateTime? leaveTo)
        {
            return await _leaveBalanceRepository.GetLeaveApplicationsAsync(employeeId, leaveId, approvalStatus, flowStatus, leaveFrom, leaveTo);
        }

        public async Task<List<EmployeeDto>> GetLeaveAssignmentEligibleEmployeesAsync(int entryByUserId, int? roleId)
        {
            return await _leaveBalanceRepository.GetLeaveAssignmentEligibleEmployeesAsync(entryByUserId, roleId);
        }
        public async Task<WorkFlowMainDto> EmployeeDetailsWorkFlow (EmployeeDetailWFDto dto)
        {
            return await _leaveBalanceRepository.EmployeeDetailsWorkFlow (dto);
        }
    }

}
