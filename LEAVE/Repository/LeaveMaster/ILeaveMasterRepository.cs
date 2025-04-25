namespace LEAVE.Repository.LeaveMaster
{
    public interface ILeaveMasterRepository
    {
        Task<int?> FillLeaveMasterAsync(int SecondEntityId, int EmpId);

    }
}
