using Gotcha.Maui.Models.Items;
using Gotcha.Maui.Models.Payloads;

namespace Gotcha.Maui.Services
{
    public interface IGameService
    {
        Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetPendingGamesAsync();
        Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetActiveGamesAsync();
        Task<(IEnumerable<GameItem>? Data, string? ErrorMessage)> GetEndedGamesAsync();
        Task<(bool Success, string? ErrorMessage)> CreateGameAsync(string gameName);
        Task<(bool Success, string? ErrorMessage)> StartGameAsync(Guid gameId, Guid adminPlayerId);
        Task<(bool Success, string? ErrorMessage)> EndGameAsync(Guid gameId, Guid adminPlayerId);
        Task<(bool Success, string? ErrorMessage)> UpdateGameSettingsAsync(UpdateGameSettingsCommand command);
    }
}
