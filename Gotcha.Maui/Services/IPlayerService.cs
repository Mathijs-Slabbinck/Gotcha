using Gotcha.Maui.Models.PageData;
using Gotcha.Maui.Models.Payloads;

namespace Gotcha.Maui.Services
{
    public interface IPlayerService
    {
        Task<(PlayerHomeData? Data, string? ErrorMessage)> GetPlayerHomeDataAsync(Guid playerId);
        Task<(ConfirmKillData? Data, string? ErrorMessage)> GetConfirmKillDataAsync(Guid playerId);
        Task<(string? Data, string? ErrorMessage)> GetPlayerUsernameAsync(Guid playerId);
        Task<(AdminData? Data, string? ErrorMessage)> GetAdminDataAsync(Guid playerId);
        Task<(bool Success, string? ErrorMessage)> ConfirmKillAsync(Guid playerId);
        Task<(bool Success, string? ErrorMessage)> ConfirmDeathAsync(Guid playerId);
        Task<(bool Success, string? ErrorMessage)> ConfirmHunterKillAsync(Guid playerId);
        Task<(bool Success, string? ErrorMessage)> PerformPlayerActionAsync(PlayerActionCommand command);
        Task<(bool Success, string? ErrorMessage)> CancelKillAsync(Guid killId, Guid adminPlayerId);
        Task<(bool Success, string? ErrorMessage)> UpdatePlayerUsernameAsync(Guid playerId, string username);
    }
}
