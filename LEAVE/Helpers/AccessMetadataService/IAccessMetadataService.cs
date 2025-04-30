using LEAVE.Dto;

namespace LEAVE.Helpers.AccessMetadataService
{
    public interface IAccessMetadataService
    {
        Task<AccessMetadataDto> GetAccessMetadataAsync(string transactionType, int roleId, int empId);
    }
}
