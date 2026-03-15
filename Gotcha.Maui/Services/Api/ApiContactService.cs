using System.Net.Http.Json;

namespace Gotcha.Maui.Services.Api
{
    public class ApiContactService : IContactService
    {
        private readonly HttpClient _httpClient;

        public ApiContactService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GotchaApi");
        }

        public async Task<bool> SubmitAsync(string reason, string message)
        {
            try
            {
                var dto = new
                {
                    LogType = "Clean",
                    LogSubType = "Other",
                    Message = $"[Contact Form] {reason}",
                    ExtraInfo = message
                };

                var response = await _httpClient.PostAsJsonAsync("api/logs", dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiContactService.SubmitAsync failed: {ex.Message}");
                return false;
            }
        }
    }
}
