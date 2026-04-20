using System.Net.Http.Json;
using Gotcha.Maui.Models.PageData;
using Gotcha.Shared.Constants;
using Gotcha.Shared.Enums;

namespace Gotcha.Maui.Services.Api
{
    public class ApiStoreService : IStoreService
    {
        private readonly HttpClient _httpClient;

        public ApiStoreService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GotchaApi");
        }

        public async Task<StoreState> GetStoreStateAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<VipSettingsResponse>(
                    $"api/vipsettings/{DevConstants.TestUserId}");

                if (response == null)
                {
                    return new StoreState();
                }

                return new StoreState
                {
                    AssassinUnlocked = response.AssassinModeUnlocked,
                    ChaosUnlocked = response.ChaosModeUnlocked,
                    TimedKillsUnlocked = response.TimedKillsUnlocked,
                    CustomKillMethodsUnlocked = response.CustomKillMethodsUnlocked,
                    Lobby100Owned = response.MaxLobbySize is "Medium" or "MediumLarge" or "Large" or "Max",
                    Lobby150Owned = response.MaxLobbySize is "MediumLarge" or "Large" or "Max",
                    Lobby500Owned = response.MaxLobbySize is "Large" or "Max",
                    Lobby10000Owned = response.MaxLobbySize == "Max",
                    CurrentPlan = Enum.TryParse<Plan>(response.UserPlan, out var plan)
                        ? plan
                        : Plan.Standard
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiStoreService.GetStoreStateAsync failed: {ex.Message}");
                return new StoreState();
            }
        }

        public async Task<bool> BuyFeatureAsync(string featureName)
        {
            try
            {
                var dto = BuildFeatureUpdateDto(featureName);
                var response = await _httpClient.PatchAsJsonAsync(
                    $"api/vipsettings/{DevConstants.TestUserId}", dto);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiStoreService.BuyFeatureAsync failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SubscribeAsync(string planName)
        {
            try
            {
                var dto = new { UserPlan = planName };
                var response = await _httpClient.PatchAsJsonAsync(
                    $"api/vipsettings/{DevConstants.TestUserId}", dto);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiStoreService.SubscribeAsync failed: {ex.Message}");
                return false;
            }
        }

        private static object BuildFeatureUpdateDto(string featureName)
        {
            return featureName switch
            {
                "Assassin" => new { AssassinModeUnlocked = true },
                "Chaos" => new { ChaosModeUnlocked = true },
                "TimedKills" => new { TimedKillsUnlocked = true },
                "CustomKillMethods" => new { CustomKillMethodsUnlocked = true },
                "Lobby100" => new { MaxLobbySize = "Medium" },
                "Lobby150" => new { MaxLobbySize = "MediumLarge" },
                "Lobby500" => new { MaxLobbySize = "Large" },
                "Lobby10000" => new { MaxLobbySize = "Max" },
                _ => new { }
            };
        }

        // Response DTO for deserialization
        private class VipSettingsResponse
        {
            public Guid Id { get; set; }
            public bool AssassinModeUnlocked { get; set; }
            public bool ChaosModeUnlocked { get; set; }
            public bool TimedKillsUnlocked { get; set; }
            public bool CustomKillMethodsUnlocked { get; set; }
            public string MaxLobbySize { get; set; } = string.Empty;
            public string UserPlan { get; set; } = string.Empty;
        }
    }
}
