
using LEAVE.Dto;
using LEAVE.Repository.LeaveMaster;
using Microsoft.AspNetCore.Mvc;

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
        public Task<List<object>> FillbasicsettingsAsync(int Masterid, string TransactionType, int SecondEntityId, int EmpId)
        {
            return _leaveMasterRepository.FillbasicsettingsAsync(Masterid, TransactionType, SecondEntityId, EmpId);
        }

        public async Task<(string ApplicableLevelsNew, string ApplicableLevelsOne, string EmpIds, string CompanyIds)> GetEntityApplicableStringsAsync(string transactionType, long masterId)
        {
            return await _leaveMasterRepository.GetEntityApplicableStringsAsync(transactionType, masterId);
        }

        public async Task<string> ProcessEntityApplicableAsync(EntityApplicableApiDto entityApplicableApiDtos)
        {
            return await _leaveMasterRepository.ProcessEntityApplicableAsync(entityApplicableApiDtos);
        }
        public Task<List<object>> GetEditLeaveMastersAsync(int masterId)
        {
            return _leaveMasterRepository.GetEditLeaveMastersAsync(masterId);
        }
        public Task<int> GetDeleteLeaveMastersAsync(int masterId)
        {
            return _leaveMasterRepository.GetDeleteLeaveMastersAsync(masterId);
        }
    }
}
