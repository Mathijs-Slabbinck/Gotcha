using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services
{
    public interface IPlayerService
    {
        Task<PlayerHomeData> GetPlayerHomeDataAsync(Guid playerId);
        Task<ConfirmKillData> GetConfirmKillDataAsync(Guid playerId);
        Task<string> GetPlayerUsernameAsync(Guid playerId);
        Task<AdminData> GetAdminDataAsync(Guid playerId);
        Task<bool> ConfirmKillAsync(Guid playerId);
        Task<bool> ConfirmDeathAsync(Guid playerId);
        Task<bool> ConfirmHunterKillAsync(Guid playerId);
        Task<bool> UpdatePlayerUsernameAsync(Guid playerId, string username);
    }
}
