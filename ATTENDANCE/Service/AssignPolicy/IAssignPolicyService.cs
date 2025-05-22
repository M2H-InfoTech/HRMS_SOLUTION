using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;

namespace ATTENDANCE.Service.AssignPolicy
{
    public interface IAssignPolicyService
    {
        Task<int> DeleteEmployeeShiftpolicy(int shiftId);
        Task<List<ShiftPolicyDto>> GetAllShiftPolicy(int levelId, int empId, string linkId);
        Task<List<AttendancePolicyAccessDto>> ViewEmployeeShiftPolicyFiltered(int attendanceAccessId, string employeeIds);
        Task<int> BulkDeleteEmpPolicy(string ShiftIDs);
        Task<List<AttendancePolicyAccessDto>> ViewEmployeeShiftPolicy(ViewEmployeeShiftPolicyDto request);
        Task<List<OvertimeValueDto>> FillOverTime();
        Task<int> InsertAttendancePolicy(InsertAttendancePolicyDto request);
    }
}
