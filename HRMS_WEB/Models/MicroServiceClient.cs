namespace HRMS_WEB.Models
{
    public class MicroServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MicroServiceClient(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetDataFromMicroservice()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7160/Employee");  // Replace with your microservice URL
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return data;
            ///Employee
        }
    }
}
