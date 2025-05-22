using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;
using ATTENDANCE.Repository.AssignPolicy;

namespace ATTENDANCE.Service.AssignPolicy
{
    public class AssignPolicyService(IAssignPolicyRepository repository) : IAssignPolicyService
    {
        public async Task<int> BulkDeleteEmpPolicy(string ShiftIDs)
        {
           return await repository.BulkDeleteEmpPolicy(ShiftIDs);
        }

        public async Task<int> DeleteEmployeeShiftpolicy(int shiftId)
        {
           return await repository.DeleteEmployeeShiftpolicy(shiftId);
        }

        public async Task<List<OvertimeValueDto>> FillOverTime()
        {
           return await repository.FillOverTime(); 
        }

        public async Task<List<ShiftPolicyDto>> GetAllShiftPolicy(int levelId, int empId, string linkId)
        {
            return await repository.GetAllShiftPolicy(levelId, empId, linkId);
        }

        public async Task<List<AttendancePolicyAccessDto>> ViewEmployeeShiftPolicy(ViewEmployeeShiftPolicyDto request)
        {
            return await repository.ViewEmployeeShiftPolicy(request); 
        }

        public async Task<List<AttendancePolicyAccessDto>> ViewEmployeeShiftPolicyFiltered(int attendanceAccessId, string employeeIds)
        {
            return await repository.ViewEmployeeShiftPolicyFiltered(attendanceAccessId, employeeIds);
        }
        public async Task<int> InsertAttendancePolicy(InsertAttendancePolicyDto request)
        {
            return await repository.InsertAttendancePolicy(request);
        }
    }
}
