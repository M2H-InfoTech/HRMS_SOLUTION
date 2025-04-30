using HRMS.EmployeeInformation.DTO.DTOs;
using Newtonsoft.Json;

namespace LEAVE.Helpers
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;
        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> GetTransactionIdByTransactionTypeAsync(string transactionType)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5194/gateway/Employee/GetTransactionIdByTransactionType?transactionType={transactionType}");
            var content = await response.Content.ReadAsStringAsync();

            if (!int.TryParse(content, out int transactionId))
            {
                throw new InvalidOperationException("Failed to parse transaction ID.");
            }

            return transactionId;
        }

        public async Task<int> GetLinkLevelByRoleIdAsync(int roleId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5194/gateway/Employee/GetLinkLevelByRoleId?roleId={roleId}");
            var content = await response.Content.ReadAsStringAsync();

            if (!int.TryParse(content, out int linkLevel))
            {
                throw new InvalidOperationException("Failed to parse link level.");
            }

            return linkLevel;
        }

        public async Task<bool> GetEntityAccessRightsAsync(int roleId, int linkLevel)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5194/gateway/Employee/GetEntityAccessRights?roleId={roleId}&linkSelect={linkLevel}");
            var content = await response.Content.ReadAsStringAsync();
            return !string.IsNullOrEmpty(content) && content.Any();
        }
        public async Task<AccessCheckResultDto> AccessLevelDetailsAndEmpList(int empId, string code, int roleId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5194/gateway/Employee/AccessChecking?empId={roleId}&code={code}&roleId={roleId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AccessCheckResultDto>(content);
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving access level", ex);
            }
        }
    }
}
