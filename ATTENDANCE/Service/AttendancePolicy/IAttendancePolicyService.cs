using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;

namespace ATTENDANCE.Service.AttendancePolicy
{
    public interface IAttendancePolicyService
    {
        Task<List<OvertimeValueDto>> FillOverTime();
        Task<int> InsertAttendancePolicy(InsertAttendancePolicyDto request);
        Task<List<LeaveTypeDto>> GetLeaveType();
    }
}
