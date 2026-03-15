using System.Net.Http.Json;
using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services.Api
{
    public class ApiPlayerService : IPlayerService
    {
        private readonly HttpClient _httpClient;

        public ApiPlayerService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GotchaApi");
        }

        public async Task<PlayerHomeData> GetPlayerHomeDataAsync(Guid playerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PlayerHomeResponse>(
                    $"api/players/{playerId}/home");

                if (response == null)
                {
                    return new PlayerHomeData();
                }

                return new PlayerHomeData
                {
                    IsAlive = response.IsAlive,
                    IsSpectator = response.IsSpectator,
                    TargetName = response.TargetName,
                    TargetUsername = response.TargetUsername,
                    Weapon = response.Weapon ?? string.Empty,
                    AssignmentExpirationDate = response.AssignmentExpirationDate,
                    IsAssassin = response.IsAssassin,
                    IsChaos = response.IsChaos,
                    IsTimed = response.IsTimed,
                    ChaosTimerMinHours = response.ChaosTimerMinHours,
                    ChaosTimerMaxHours = response.ChaosTimerMaxHours,
                    TargetTimeOutHours = response.TargetTimeOutHours,
                    CustomRules = response.CustomRules,
                    ShowHunter = response.ShowHunter,
                    HunterName = response.HunterName,
                    HunterOtherName = response.HunterOtherName,
                    StartDate = response.StartDate,
                    EndDate = response.EndDate,
                    WinnerName = response.WinnerName,
                    WinnerOtherName = response.WinnerOtherName,
                    KilledOnDate = response.KilledOnDate,
                    KillerName = response.KillerName,
                    KillerOtherName = response.KillerOtherName,
                    ShowLivingPlayerCount = response.ShowLivingPlayerCount,
                    ShowLivingPlayerNames = response.ShowLivingPlayerNames,
                    Kills = response.Kills.Select(k => new KillItem
                    {
                        VictimName = k.VictimName,
                        VictimUsername = k.VictimUsername,
                        Weapon = k.Weapon ?? string.Empty,
                        TimeStamp = k.TimeStamp
                    }).ToList(),
                    Players = response.Players.Select(p => new PlayerItem
                    {
                        Name = p.Name,
                        OtherName = p.OtherName,
                        IsAlive = p.IsAlive
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.GetPlayerHomeDataAsync failed: {ex.Message}");
                return new PlayerHomeData();
            }
        }

        public async Task<ConfirmKillData> GetConfirmKillDataAsync(Guid playerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ConfirmKillResponse>(
                    $"api/players/{playerId}/confirmkill");

                if (response == null)
                {
                    return new ConfirmKillData();
                }

                return new ConfirmKillData
                {
                    TargetName = response.TargetName,
                    TargetUsername = response.TargetUsername,
                    Weapon = response.Weapon ?? string.Empty,
                    HunterName = response.HunterName,
                    HunterUsername = response.HunterUsername,
                    IsAssassinMode = response.IsAssassinMode,
                    ShowHunter = response.ShowHunter
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.GetConfirmKillDataAsync failed: {ex.Message}");
                return new ConfirmKillData();
            }
        }

        public async Task<string> GetPlayerUsernameAsync(Guid playerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PlayerResponse>(
                    $"api/players/{playerId}");

                return response?.UserName ?? string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.GetPlayerUsernameAsync failed: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task<AdminData> GetAdminDataAsync(Guid playerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<AdminDataResponse>(
                    $"api/players/{playerId}/admin");

                if (response == null)
                {
                    return new AdminData();
                }

                return new AdminData
                {
                    HasStarted = response.HasStarted,
                    GameName = response.GameName,
                    InviteLink = response.InviteLink,
                    PlayerCount = response.PlayerCount,
                    MaxPlayers = response.MaxPlayers,
                    ShowPlayerImages = response.ShowPlayerImages,
                    ShowGender = response.ShowGender,
                    EnforcePlayerImages = response.EnforcePlayerImages,
                    ShowRealNames = response.ShowRealNames,
                    ShowUsernames = response.ShowUsernames,
                    ShowLivingPlayerCount = response.ShowLivingPlayerCount,
                    ShowLivingPlayerNames = response.ShowLivingPlayerNames,
                    ShowLivingPlayerNamesToDeath = response.ShowLivingPlayerNamesToDeath,
                    IsAssassin = response.IsAssassin,
                    ShowHunter = response.ShowHunter,
                    IsChaos = response.IsChaos,
                    IsTimed = response.IsTimed,
                    CustomKillMethods = response.CustomKillMethods,
                    KillMethods = response.KillMethods,
                    ChaosTimerMinHours = response.ChaosTimerMinHours,
                    ChaosTimerMaxHours = response.ChaosTimerMaxHours,
                    TargetTimeOutHours = response.TargetTimeOutHours,
                    CustomRulesText = response.CustomRulesText,
                    AssassinModeUnlocked = response.AssassinModeUnlocked,
                    ChaosModeUnlocked = response.ChaosModeUnlocked,
                    TimedKillsUnlocked = response.TimedKillsUnlocked,
                    Players = response.Players.Select(p => new AdminPlayerItem
                    {
                        PlayerId = p.PlayerId,
                        Name = p.Name,
                        Username = p.Username,
                        HasImage = p.HasImage,
                        IsAdmin = p.IsAdmin,
                        IsSpectator = p.IsSpectator
                    }).ToList(),
                    PendingKills = response.PendingKills.Select(k => new AdminKillItem
                    {
                        KillId = k.KillId,
                        KillerName = k.KillerName,
                        VictimName = k.VictimName,
                        Weapon = k.Weapon ?? string.Empty,
                        Moment = k.Moment
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.GetAdminDataAsync failed: {ex.Message}");
                return new AdminData();
            }
        }

        public Task<bool> ConfirmKillAsync(Guid playerId)
        {
            throw new NotImplementedException("Kill confirmation endpoint not yet wired up.");
        }

        public Task<bool> ConfirmDeathAsync(Guid playerId)
        {
            throw new NotImplementedException("Death confirmation endpoint not yet wired up.");
        }

        public Task<bool> ConfirmHunterKillAsync(Guid playerId)
        {
            throw new NotImplementedException("Hunter kill confirmation endpoint not yet wired up.");
        }

        public async Task<bool> UpdatePlayerUsernameAsync(Guid playerId, string username)
        {
            try
            {
                var dto = new { UserName = username };
                var response = await _httpClient.PatchAsJsonAsync(
                    $"api/players/{playerId}", dto);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.UpdatePlayerUsernameAsync failed: {ex.Message}");
                return false;
            }
        }

        #region Response DTOs for deserialization

        private class PlayerResponse
        {
            public Guid Id { get; set; }
            public string? UserName { get; set; }
        }

        private class PlayerHomeResponse
        {
            public bool IsAlive { get; set; }
            public bool IsSpectator { get; set; }
            public string TargetName { get; set; } = string.Empty;
            public string TargetUsername { get; set; } = string.Empty;
            public string? Weapon { get; set; }
            public DateTime? AssignmentExpirationDate { get; set; }
            public bool IsAssassin { get; set; }
            public bool IsChaos { get; set; }
            public bool IsTimed { get; set; }
            public int ChaosTimerMinHours { get; set; }
            public int ChaosTimerMaxHours { get; set; }
            public int TargetTimeOutHours { get; set; }
            public List<string> CustomRules { get; set; } = new();
            public bool ShowHunter { get; set; }
            public string HunterName { get; set; } = string.Empty;
            public string HunterOtherName { get; set; } = string.Empty;
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string WinnerName { get; set; } = string.Empty;
            public string WinnerOtherName { get; set; } = string.Empty;
            public DateTime? KilledOnDate { get; set; }
            public string KillerName { get; set; } = string.Empty;
            public string KillerOtherName { get; set; } = string.Empty;
            public bool ShowLivingPlayerCount { get; set; }
            public bool ShowLivingPlayerNames { get; set; }
            public List<KillItemResponse> Kills { get; set; } = new();
            public List<PlayerItemResponse> Players { get; set; } = new();
        }

        private class KillItemResponse
        {
            public string VictimName { get; set; } = string.Empty;
            public string VictimUsername { get; set; } = string.Empty;
            public string? Weapon { get; set; }
            public DateTime TimeStamp { get; set; }
        }

        private class PlayerItemResponse
        {
            public string Name { get; set; } = string.Empty;
            public string OtherName { get; set; } = string.Empty;
            public bool IsAlive { get; set; }
        }

        private class ConfirmKillResponse
        {
            public string TargetName { get; set; } = string.Empty;
            public string TargetUsername { get; set; } = string.Empty;
            public string? Weapon { get; set; }
            public string HunterName { get; set; } = string.Empty;
            public string HunterUsername { get; set; } = string.Empty;
            public bool IsAssassinMode { get; set; }
            public bool ShowHunter { get; set; }
        }

        private class AdminDataResponse
        {
            public bool HasStarted { get; set; }
            public string GameName { get; set; } = string.Empty;
            public string InviteLink { get; set; } = string.Empty;
            public int PlayerCount { get; set; }
            public int MaxPlayers { get; set; }
            public List<AdminPlayerItemResponse> Players { get; set; } = new();
            public List<AdminKillItemResponse> PendingKills { get; set; } = new();
            public bool ShowPlayerImages { get; set; }
            public bool ShowGender { get; set; }
            public bool EnforcePlayerImages { get; set; }
            public bool ShowRealNames { get; set; }
            public bool ShowUsernames { get; set; }
            public bool ShowLivingPlayerCount { get; set; }
            public bool ShowLivingPlayerNames { get; set; }
            public bool ShowLivingPlayerNamesToDeath { get; set; }
            public bool IsAssassin { get; set; }
            public bool ShowHunter { get; set; }
            public bool IsChaos { get; set; }
            public bool IsTimed { get; set; }
            public bool CustomKillMethods { get; set; }
            public string KillMethods { get; set; } = string.Empty;
            public int ChaosTimerMinHours { get; set; }
            public int ChaosTimerMaxHours { get; set; }
            public int TargetTimeOutHours { get; set; }
            public string CustomRulesText { get; set; } = string.Empty;
            public bool AssassinModeUnlocked { get; set; }
            public bool ChaosModeUnlocked { get; set; }
            public bool TimedKillsUnlocked { get; set; }
        }

        private class AdminPlayerItemResponse
        {
            public Guid PlayerId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public bool HasImage { get; set; }
            public bool IsAdmin { get; set; }
            public bool IsSpectator { get; set; }
        }

        private class AdminKillItemResponse
        {
            public Guid KillId { get; set; }
            public string KillerName { get; set; } = string.Empty;
            public string VictimName { get; set; } = string.Empty;
            public string? Weapon { get; set; }
            public DateTime Moment { get; set; }
        }

        #endregion
    }
}
