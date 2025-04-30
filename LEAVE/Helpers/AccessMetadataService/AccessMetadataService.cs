using LEAVE.Dto;

namespace LEAVE.Helpers.AccessMetadataService
{
    public class AccessMetadataService : IAccessMetadataService
    {
        private readonly ExternalApiService _externalApiService;
        public AccessMetadataService(ExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }
        public async Task<AccessMetadataDto> GetAccessMetadataAsync(string transactionType, int roleId, int empId)
        {
            var transactionId = await _externalApiService.GetTransactionIdByTransactionTypeAsync(transactionType);
            var linkLevel = await _externalApiService.GetLinkLevelByRoleIdAsync(roleId);
            var hasAccess = await _externalApiService.GetEntityAccessRightsAsync(roleId, linkLevel);
            var accessDetails = await _externalApiService.AccessLevelDetailsAndEmpList(empId, transactionType, roleId);
            return new AccessMetadataDto
            {
                TransactionId = transactionId,
                LinkLevel = linkLevel,
                HasAccessRights = hasAccess,
                accessCheckResultDto = accessDetails
            };
        }
    }
}
