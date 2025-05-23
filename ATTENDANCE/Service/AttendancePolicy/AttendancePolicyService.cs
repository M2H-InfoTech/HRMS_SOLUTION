using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;
using ATTENDANCE.Repository.AttendancePolicy;
using HRMS.EmployeeInformation.Repository.Common;

namespace ATTENDANCE.Service.AttendancePolicy
{
    public class AttendancePolicyService(IAttendancePolicyRepository repository):IAttendancePolicyService
    {
        public async Task<List<OvertimeValueDto>> FillOverTime()
        {
            return await repository.FillOverTime();
        }

        public async Task<List<LeaveTypeDto>> GetLeaveType()
        {
           return await repository.GetLeaveType();
        }

        public async Task<int> InsertAttendancePolicy(InsertAttendancePolicyDto request)
        {
            return await repository.InsertAttendancePolicy(request);
        }
    }
}
