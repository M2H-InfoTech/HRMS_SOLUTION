using LEAVE.Dto;
using static LEAVE.Repository.LeaveMaster.LeaveMasterRepository;

namespace LEAVE.Repository.LeaveMaster
{
    public interface ILeaveMasterRepository
    {
        Task<List<LeaveDetailModelDto>> FillLeaveMasterAsync(int secondEntityId, int empId);
        Task<List<object>> FillvaluetypesAsync(string type);
        Task<int?> CreateMasterAsync(CreateMasterDto createMasterdto);
        Task<List<object>> FillbasicsettingsAsync(int Masterid, string TransactionType, int SecondEntityId, int EmpId);
        Task<(string ApplicableLevelsNew, string ApplicableLevelsOne, string EmpIds, string CompanyIds)> GetEntityApplicableStringsAsync(string transactionType, long masterId);
        Task<string> ProcessEntityApplicableAsync(EntityApplicableApiDto entityApplicableApiDtos);
        Task<List<object>> GetEditLeaveMastersAsync(int masterId);
        Task<int> GetDeleteLeaveMastersAsync(int masterId);

        Task<List<LeaveBalanceBaseDto>> GetLeaveBalanceDetails(string employeeIds, string submode, int leaveBalanceFormat);// = "balancedetails",leaveBalanceFormat=1




    }
}
