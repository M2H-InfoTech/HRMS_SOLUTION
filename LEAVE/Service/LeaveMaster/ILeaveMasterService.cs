using LEAVE.Dto;
using LEAVE.Repository.LeaveMaster;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Service.LeaveMaster
{
    public interface ILeaveMasterService
    {
        Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId);
        Task<List<object>> FillvaluetypesAsync(string type);

        Task<int?> CreateMasterAsync(CreateMasterDto createMasterDto);
        Task<List<object>> FillbasicsettingsAsync(int Masterid, string TransactionType, int SecondEntityId, int EmpId);
        Task<(string ApplicableLevelsNew, string ApplicableLevelsOne, string EmpIds, string CompanyIds)> GetEntityApplicableStringsAsync(string transactionType, long masterId);
        Task<string> ProcessEntityApplicableAsync(EntityApplicableApiDto entityApplicableApiDtos);
        Task<List<object>> GetEditLeaveMastersAsync(int masterId);
        Task<int> GetDeleteLeaveMastersAsync(int masterId);

    }
}
