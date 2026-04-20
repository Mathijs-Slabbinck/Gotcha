using System.Net.Http.Json;
using Gotcha.Maui.Models;
using Gotcha.Shared.Constants;

namespace Gotcha.Maui.Services.Api
{
    public class ApiUserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public ApiUserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GotchaApi");
        }

        public async Task<UserProfile> GetProfileAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<UserProfileResponse>(
                    $"api/gotchausers/{DevConstants.TestUserId}/profile");

                if (response == null)
                {
                    return new UserProfile();
                }

                return new UserProfile
                {
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Username = response.UserName,
                    Email = response.Email,
                    Birthday = response.BirthDate,
                    GamesPlayed = response.GamesPlayed,
                    GamesWon = response.GamesWon,
                    TotalKills = response.TotalKills,
                    BiggestKillStreak = response.BiggestKillStreak,
                    AccountCreated = response.AccountCreationDate.ToString("MMMM yyyy")
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiUserService.GetProfileAsync failed: {ex.Message}");
                return new UserProfile();
            }
        }

        public async Task<bool> UpdateProfileAsync(UserProfile profile)
        {
            try
            {
                var dto = new
                {
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    UserName = profile.Username,
                    Email = profile.Email
                };

                var response = await _httpClient.PutAsJsonAsync(
                    $"api/gotchausers/{DevConstants.TestUserId}", dto);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiUserService.UpdateProfileAsync failed: {ex.Message}");
                return false;
            }
        }

        // Response DTO for deserialization (matches API's UserProfileResponseDto)
        private class UserProfileResponse
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public DateTime BirthDate { get; set; }
            public DateTime AccountCreationDate { get; set; }
            public int GamesPlayed { get; set; }
            public int GamesWon { get; set; }
            public int TotalKills { get; set; }
            public int BiggestKillStreak { get; set; }
        }
    }
}
