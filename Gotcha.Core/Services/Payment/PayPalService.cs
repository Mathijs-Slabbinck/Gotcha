using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Gotcha.Core.Enums;
using Microsoft.Extensions.Options;

namespace Gotcha.Core.Services.Payment
{
    public class PayPalService : IPayPalService
    {
        private readonly HttpClient _httpClient;
        private readonly PayPalSettings _settings;

        private string? _cachedToken;
        private DateTime _tokenExpiry = DateTime.MinValue;

        public PayPalService(HttpClient httpClient, IOptions<PayPalSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string?> CreateOrder(StoreItem item)
        {
            string? accessToken = await GetAccessToken();
            if (accessToken == null)
            {
                return null;
            }

            decimal price = StorePricing.GetPrice(item);
            string description = StorePricing.GetDescription(item);

            var orderRequest = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        description = description,
                        amount = new
                        {
                            currency_code = "USD",
                            value = price.ToString("F2")
                        }
                    }
                }
            };

            string json = JsonSerializer.Serialize(orderRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v2/checkout/orders");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = content;

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseBody);
            string? orderId = doc.RootElement.GetProperty("id").GetString();

            return orderId;
        }

        public async Task<bool> CaptureOrder(string orderId)
        {
            string? accessToken = await GetAccessToken();
            if (accessToken == null)
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v2/checkout/orders/{orderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseBody);
            string? status = doc.RootElement.GetProperty("status").GetString();

            return status == "COMPLETED";
        }

        private async Task<string?> GetAccessToken()
        {
            // Return cached token if still valid (with 60s buffer)
            if (_cachedToken != null && DateTime.UtcNow < _tokenExpiry)
            {
                return _cachedToken;
            }

            string credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.Secret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseBody);
            string? accessToken = doc.RootElement.GetProperty("access_token").GetString();

            // PayPal tokens typically expire in 3600 seconds; cache with 60s safety margin
            int expiresIn = 3600;
            if (doc.RootElement.TryGetProperty("expires_in", out JsonElement expiresElement))
            {
                expiresIn = expiresElement.GetInt32();
            }

            _cachedToken = accessToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(expiresIn - 60);

            return accessToken;
        }
    }
}
