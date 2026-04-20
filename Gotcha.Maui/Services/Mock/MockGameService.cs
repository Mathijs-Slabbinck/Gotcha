using Gotcha.Maui.Models.Items;
using Gotcha.Maui.Models.Payloads;

namespace Gotcha.Maui.Services.Mock
{
    public class MockGameService : IGameService
    {
        public Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetPendingGamesAsync()
        {
            var games = new List<GameItem>
            {
                new GameItem
                {
                    GameId = Guid.NewGuid(),
                    Name = "Friday Night Gotcha",
                    CreatedDate = "2025-03-10",
                    PlayerCount = 8,
                    PlayerId = Guid.NewGuid()
                },
                new GameItem
                {
                    GameId = Guid.NewGuid(),
                    Name = "Office Battle Royale",
                    CreatedDate = "2025-03-12",
                    PlayerCount = 15,
                    PlayerId = Guid.NewGuid()
                }
            };

            return Task.FromResult<(IEnumerable<GameItem>?, string?)>((games, null));
        }

        public Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetActiveGamesAsync()
        {
            var games = new List<GameItem>
            {
                new GameItem
                {
                    GameId = Guid.NewGuid(),
                    Name = "Campus Hunt",
                    CreatedDate = "2025-03-01",
                    StartDate = "2025-03-05",
                    PlayerCount = 12,
                    IsAlive = true,
                    PlayerId = Guid.NewGuid()
                }
            };

            return Task.FromResult<(IEnumerable<GameItem>?, string?)>((games, null));
        }

        public Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetEndedGamesAsync()
        {
            var games = new List<GameItem>
            {
                new GameItem
                {
                    GameId = Guid.NewGuid(),
                    Name = "Summer Showdown",
                    CreatedDate = "2025-01-15",
                    StartDate = "2025-01-20",
                    EndDate = "2025-02-10",
                    WinnerName = "TheLegend27",
                    PlayerCount = 20,
                    IsAlive = false,
                    PlayerId = Guid.NewGuid()
                }
            };

            return Task.FromResult<(IEnumerable<GameItem>?, string?)>((games, null));
        }

        public Task<(bool Success, string? ErrorMessage)> CreateGameAsync(string gameName)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<(bool Success, string? ErrorMessage)> StartGameAsync(Guid gameId, Guid adminPlayerId)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<(bool Success, string? ErrorMessage)> EndGameAsync(Guid gameId, Guid adminPlayerId)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<(bool Success, string? ErrorMessage)> UpdateGameSettingsAsync(UpdateGameSettingsCommand command)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }
    }
}
