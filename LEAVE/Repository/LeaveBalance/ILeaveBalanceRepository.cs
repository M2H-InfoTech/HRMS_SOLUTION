using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Dto;

namespace LEAVE.Repository.LeaveBalance
{
    public interface ILeaveBalanceRepository
    {
        Task<List<RetrieveBranchDetailsDto>> RetrieveBranchDetails(int instID, int branchID);
        Task<List<LeaveApplicationDto>> GetLeaveApplicationsAsync(int employeeId, int leaveId, string approvalStatus, string? flowStatus, DateTime? leaveFrom, DateTime? leaveTo);
        Task<List<EmployeeDto>> GetLeaveAssignmentEligibleEmployeesAsync(int entryByUserId, int? roleId);


    }
}
