
using HRMS.EmployeeInformation.DTO.DTOs;
using LEAVE.Dto;
using LEAVE.Repository.LeaveMaster;

namespace LEAVE.Service.LeaveMaster
{
    public class LeaveMasterService : ILeaveMasterService
    {
        private readonly ILeaveMasterRepository _leaveMasterRepository;
        public LeaveMasterService(ILeaveMasterRepository leaveMasterRepository)
        {
            _leaveMasterRepository = leaveMasterRepository;
        }

        public Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId)
        {
            return _leaveMasterRepository.FillLeaveMasterAsync(secondEntityId, empId);
        }
        public Task<List<object>> FillvaluetypesAsync(string type)
        {
            return _leaveMasterRepository.FillvaluetypesAsync(type);
        }

        public async Task<int?> CreateMasterAsync(CreateMasterDto createMasterDto)
        {
            return await _leaveMasterRepository.CreateMasterAsync(createMasterDto);
        }
        public Task<List<object>> FillbasicsettingsAsync(int Masterid, int SecondEntityId, int EmpId)
        {
            return _leaveMasterRepository.FillbasicsettingsAsync(Masterid, SecondEntityId, EmpId);
        }
    }
}
