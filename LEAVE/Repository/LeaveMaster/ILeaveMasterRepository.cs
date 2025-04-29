using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Dto;

namespace LEAVE.Repository.LeaveMaster
{
    public interface ILeaveMasterRepository
    {
        Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId);
        Task<List<object>> FillvaluetypesAsync(string type);
        Task<int?> CreateMasterAsync(CreateMasterDto createMasterdto);
        Task<List<object>> FillbasicsettingsAsync(int Masterid, int SecondEntityId, int EmpId);
    }
}
