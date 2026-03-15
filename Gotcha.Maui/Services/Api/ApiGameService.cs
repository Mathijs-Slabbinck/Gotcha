using System.Net.Http.Json;
using Gotcha.Maui.Constants;
using Gotcha.Maui.Models;

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

        public async Task<IEnumerable<GameItem>> GetPendingGamesAsync()
        {
            return await GetGamesByStatusAsync(GameStatus.Pending);
        }

        public async Task<IEnumerable<GameItem>> GetActiveGamesAsync()
        {
            return await GetGamesByStatusAsync(GameStatus.Active);
        }

        public async Task<IEnumerable<GameItem>> GetEndedGamesAsync()
        {
            return await GetGamesByStatusAsync(GameStatus.Ended);
        }

        public async Task<bool> CreateGameAsync(string gameName)
        {
            try
            {
                var dto = new
                {
                    Name = gameName,
                    MaxPlayers = 50,
                    CreatorId = DevConstants.TestUserId
                };

                var response = await _httpClient.PostAsJsonAsync("api/games", dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiGameService.CreateGameAsync failed: {ex.Message}");
                return false;
            }
        }

        private async Task<IEnumerable<GameItem>> GetGamesByStatusAsync(string status)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<GameItemResponse>>(
                    $"api/gotchausers/{DevConstants.TestUserId}/games?status={status}");

                if (response == null)
                {
                    return new List<GameItem>();
                }

                return response.Select(g => new GameItem
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiGameService.GetGamesByStatusAsync({status}) failed: {ex.Message}");
                return new List<GameItem>();
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
