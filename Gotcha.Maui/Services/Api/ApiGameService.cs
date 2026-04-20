using System.Net.Http.Json;
using Gotcha.Maui.Extensions;
using Gotcha.Maui.Models.Items;
using Gotcha.Maui.Models.Payloads;
using Gotcha.Shared.Constants;

namespace Gotcha.Maui.Services.Api
{
    public class ApiGameService : IGameService
    {
        private static class GameStatus
        {
            public const string Pending = "pending";
            public const string Active = "active";
            public const string Ended = "ended";
        }

        private readonly HttpClient _httpClient;

        public ApiGameService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GotchaApi");
        }

        public Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetPendingGamesAsync()
        {
            return GetGamesByStatusAsync(GameStatus.Pending);
        }

        public Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetActiveGamesAsync()
        {
            return GetGamesByStatusAsync(GameStatus.Active);
        }

        public Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetEndedGamesAsync()
        {
            return GetGamesByStatusAsync(GameStatus.Ended);
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateGameAsync(string gameName)
        {
            try
            {
                var dto = new
                {
                    Name = gameName,
                    MaxPlayers = 50,
                    CreatorId = DevConstants.TestUserId
                };

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/games", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                string? serverMessage = await response.Content.ReadJsonStringAsync();
                return (false, serverMessage ?? "Something went wrong. Please try again.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiGameService.CreateGameAsync failed: {ex.Message}");
                return (false, "Could not reach the server. Please check your connection.");
            }
        }

        public Task<(bool Success, string? ErrorMessage)> StartGameAsync(Guid gameId, Guid adminPlayerId)
        {
            var dto = new { AdminPlayerId = adminPlayerId };
            return PostJsonAsync($"api/games/{gameId}/start", dto);
        }

        public Task<(bool Success, string? ErrorMessage)> EndGameAsync(Guid gameId, Guid adminPlayerId)
        {
            var dto = new { AdminPlayerId = adminPlayerId };
            return PostJsonAsync($"api/games/{gameId}/end", dto);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateGameSettingsAsync(UpdateGameSettingsCommand command)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PatchAsJsonAsync(
                    $"api/games/{command.GameId}/settings", command);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                string? serverMessage = await response.Content.ReadJsonStringAsync();
                return (false, serverMessage ?? "Something went wrong. Please try again.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiGameService.UpdateGameSettingsAsync failed: {ex.Message}");
                return (false, "Could not reach the server. Please check your connection.");
            }
        }

        private async Task<(bool Success, string? ErrorMessage)> PostJsonAsync(string endpoint, object body)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, body);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                string? serverMessage = await response.Content.ReadJsonStringAsync();
                return (false, serverMessage ?? "Something went wrong. Please try again.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiGameService POST {endpoint} failed: {ex.Message}");
                return (false, "Could not reach the server. Please check your connection.");
            }
        }

        private async Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetGamesByStatusAsync(string status)
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(
                    $"api/gotchausers/{DevConstants.TestUserId}/games?status={status}");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    string? serverMessage = await httpResponse.Content.ReadJsonStringAsync();
                    return (null, serverMessage ?? "Something went wrong. Please try again.");
                }

                List<GameItemResponse>? response = await httpResponse.Content.ReadFromJsonAsync<List<GameItemResponse>>();

                if (response == null)
                {
                    return (null, "No response from server.");
                }

                IEnumerable<GameItem> games = response.Select(g => new GameItem
                {
                    GameId = g.Id,
                    Name = g.Name,
                    CreatedDate = g.CreationDate.ToString("yyyy-MM-dd"),
                    StartDate = g.StartDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    EndDate = g.EndDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                    WinnerName = g.WinnerName ?? string.Empty,
                    PlayerCount = g.PlayerCount,
                    IsAlive = g.IsAlive,
                    PlayerId = g.PlayerId
                }).ToList();

                return (games, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiGameService.GetGamesByStatusAsync({status}) failed: {ex.Message}");
                return (null, "Could not reach the server. Please check your connection.");
            }
        }

        // Response DTO for deserialization (matches API's GameItemResponseDto)
        private class GameItemResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public DateTime CreationDate { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public bool HasStarted { get; set; }
            public bool IsFinished { get; set; }
            public string? WinnerName { get; set; }
            public int PlayerCount { get; set; }
            public bool IsAlive { get; set; }
            public Guid PlayerId { get; set; }
        }
    }
}
