namespace LEAVE.Service.LeaveMaster
{
    public interface ILeaveMasterService
    {
        Task<int?> FillLeaveMasterAsync(int SecondEntityId, int EmpId);
    }
}
