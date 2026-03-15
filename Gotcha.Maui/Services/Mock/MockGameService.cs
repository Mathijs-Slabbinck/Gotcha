using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services.Mock
{
    public class MockGameService : IGameService
    {
        public Task<IEnumerable<GameItem>> GetPendingGamesAsync()
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

            return Task.FromResult<IEnumerable<GameItem>>(games);
        }

        public Task<IEnumerable<GameItem>> GetActiveGamesAsync()
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

            return Task.FromResult<IEnumerable<GameItem>>(games);
        }

        public Task<IEnumerable<GameItem>> GetEndedGamesAsync()
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

            return Task.FromResult<IEnumerable<GameItem>>(games);
        }

        public Task<bool> CreateGameAsync(string gameName)
        {
            return Task.FromResult(true);
        }
    }
}
