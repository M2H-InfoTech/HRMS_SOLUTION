using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;

namespace ATTENDANCE.Repository.AttendancePolicy
{
    public interface IAttendancePolicyRepository
    {
        Task<List<OvertimeValueDto>> FillOverTime();
        Task<int> InsertAttendancePolicy(InsertAttendancePolicyDto request);
        Task<List<LeaveTypeDto>> GetLeaveType();
    }
}
