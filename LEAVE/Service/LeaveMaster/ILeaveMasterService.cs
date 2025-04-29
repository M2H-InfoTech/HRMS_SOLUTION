using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Dto;

namespace LEAVE.Service.LeaveMaster
{
    public interface ILeaveMasterService
    {
        Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId);
        Task<List<object>> FillvaluetypesAsync(string type);

        Task<int?> CreateMasterAsync(CreateMasterDto createMasterDto);
        Task<List<object>> FillbasicsettingsAsync(int Masterid, int SecondEntityId, int EmpId);
    }
}
