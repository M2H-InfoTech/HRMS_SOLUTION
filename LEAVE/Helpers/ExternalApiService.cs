﻿using System.Text.Json;
using HRMS.EmployeeInformation.DTO.DTOs;

namespace LEAVE.Helpers
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ExternalApiService(HttpClient httpClient, HttpClientSettings httpClientSettings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (httpClientSettings == null || string.IsNullOrWhiteSpace(httpClientSettings.baseUrl))
                throw new ArgumentException("Base URL configuration is invalid.", nameof(httpClientSettings));

            _baseUrl = httpClientSettings.baseUrl;
        }

        public async Task<int> GetTransactionIdByTransactionTypeAsync(string transactionType)
        {
            return await GetFromApiAsync<int>($"GetTransactionIdByTransactionType?transactionType={transactionType}");
        }

        public async Task<int> GetLinkLevelByRoleIdAsync(int roleId)
        {
            return await GetFromApiAsync<int>($"GetLinkLevelByRoleId?roleId={roleId}");
        }

        public async Task<bool> GetEntityAccessRightsAsync(int roleId, int linkLevel)
        {
            var content = await GetStringFromApiAsync($"GetEntityAccessRights?roleId={roleId}&linkSelect={linkLevel}");
            return !string.IsNullOrEmpty(content);
        }

        public async Task<AccessCheckResultDto> AccessLevelDetailsAndEmpList(int empId, string code, int roleId)
        {
            return await GetFromApiAsync<AccessCheckResultDto>($"AccessChecking?empId={empId}&code={code}&roleId={roleId}");
        }

        // --- Helper Methods ---

        private async Task<T> GetFromApiAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GET {endpoint} failed with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            if (typeof(T) == typeof(int))
            {
                if (!int.TryParse(content, out var intValue))
                    throw new InvalidOperationException($"Failed to parse int from response: {content}");
                return (T)(object)intValue;
            }

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private async Task<string> GetStringFromApiAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GET {endpoint} failed with status code {response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<int> EmployeeParameterSettings(int employeeId, string drpType, string parameterCode, string parameterType)
        {
            return await GetFromApiAsync<int>($"GetEmployeeParameterSettings?employeeId={employeeId}&drpType={drpType}&parameterCode={parameterCode}&parameterType={parameterType}");
            //GetEmployeeParameterSettings?employeeId=72&drpType=EmployeeReporting&parameterCode=Leavecalculation&parameterType=COM
        }
    }
}
