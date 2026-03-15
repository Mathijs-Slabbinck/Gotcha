using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameItem>> GetPendingGamesAsync();
        Task<IEnumerable<GameItem>> GetActiveGamesAsync();
        Task<IEnumerable<GameItem>> GetEndedGamesAsync();
        Task<bool> CreateGameAsync(string gameName);
    }
}
