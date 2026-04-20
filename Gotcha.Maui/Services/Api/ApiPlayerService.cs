using System.Net.Http.Json;
using Gotcha.Maui.Enums;
using Gotcha.Maui.Extensions;
using Gotcha.Maui.Models.Items;
using Gotcha.Maui.Models.PageData;
using Gotcha.Maui.Models.Payloads;

namespace Gotcha.Maui.Services.Api
{
    public class ApiPlayerService : IPlayerService
    {
        private readonly HttpClient _httpClient;

        public ApiPlayerService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GotchaApi");
        }

        public async Task<(PlayerHomeData? Data, string? ErrorMessage)> GetPlayerHomeDataAsync(Guid playerId)
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(
                    $"api/players/{playerId}/home");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    string? serverMessage = await httpResponse.Content.ReadJsonStringAsync();
                    return (null, serverMessage ?? "Something went wrong. Please try again.");
                }

                PlayerHomeResponse? response = await httpResponse.Content.ReadFromJsonAsync<PlayerHomeResponse>();

                if (response == null)
                {
                    return (null, "No response from server.");
                }

                PlayerHomeData data = new PlayerHomeData
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

                return (data, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.GetPlayerHomeDataAsync failed: {ex.Message}");
                return (null, "Could not reach the server. Please check your connection.");
            }
        }

        public async Task<(ConfirmKillData? Data, string? ErrorMessage)> GetConfirmKillDataAsync(Guid playerId)
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(
                    $"api/players/{playerId}/confirmkill");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    string? serverMessage = await httpResponse.Content.ReadJsonStringAsync();
                    return (null, serverMessage ?? "Something went wrong. Please try again.");
                }

                ConfirmKillResponse? response = await httpResponse.Content.ReadFromJsonAsync<ConfirmKillResponse>();

                if (response == null)
                {
                    return (null, "No response from server.");
                }

                ConfirmKillData data = new ConfirmKillData
                {
                    TargetName = response.TargetName,
                    TargetUsername = response.TargetUsername,
                    Weapon = response.Weapon ?? string.Empty,
                    HunterName = response.HunterName,
                    HunterUsername = response.HunterUsername,
                    IsAssassinMode = response.IsAssassinMode,
                    ShowHunter = response.ShowHunter
                };

                return (data, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.GetConfirmKillDataAsync failed: {ex.Message}");
                return (null, "Could not reach the server. Please check your connection.");
            }
        }

        public async Task<(string? Data, string? ErrorMessage)> GetPlayerUsernameAsync(Guid playerId)
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(
                    $"api/players/{playerId}");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    string? serverMessage = await httpResponse.Content.ReadJsonStringAsync();
                    return (null, serverMessage ?? "Something went wrong. Please try again.");
                }

                PlayerResponse? response = await httpResponse.Content.ReadFromJsonAsync<PlayerResponse>();

                if (response == null)
                {
                    return (null, "No response from server.");
                }

                return (response.UserName ?? string.Empty, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.GetPlayerUsernameAsync failed: {ex.Message}");
                return (null, "Could not reach the server. Please check your connection.");
            }
        }

        public async Task<(AdminData? Data, string? ErrorMessage)> GetAdminDataAsync(Guid playerId)
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(
                    $"api/players/{playerId}/admin");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    string? serverMessage = await httpResponse.Content.ReadJsonStringAsync();
                    return (null, serverMessage ?? "Something went wrong. Please try again.");
                }

                AdminDataResponse? response = await httpResponse.Content.ReadFromJsonAsync<AdminDataResponse>();

                if (response == null)
                {
                    return (null, "No response from server.");
                }

                AdminData data = new AdminData
                {
                    GameId = response.GameId,
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

                return (data, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.GetAdminDataAsync failed: {ex.Message}");
                return (null, "Could not reach the server. Please check your connection.");
            }
        }

        public Task<(bool Success, string? ErrorMessage)> ConfirmKillAsync(Guid playerId)
        {
            return PostBodylessAsync($"api/players/{playerId}/confirmkill");
        }

        public Task<(bool Success, string? ErrorMessage)> ConfirmDeathAsync(Guid playerId)
        {
            return PostBodylessAsync($"api/players/{playerId}/confirmdeath");
        }

        public Task<(bool Success, string? ErrorMessage)> ConfirmHunterKillAsync(Guid playerId)
        {
            return PostBodylessAsync($"api/players/{playerId}/confirmhunterkill");
        }

        private async Task<(bool Success, string? ErrorMessage)> PostBodylessAsync(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, null);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                string? serverMessage = await response.Content.ReadJsonStringAsync();
                return (false, serverMessage ?? "Something went wrong. Please try again.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService POST {endpoint} failed: {ex.Message}");
                return (false, "Could not reach the server. Please check your connection.");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> PerformPlayerActionAsync(PlayerActionCommand command)
        {
            try
            {
                HttpResponseMessage response;

                if (command.Action == AdminPlayerCommandActions.Kick)
                {
                    response = await _httpClient.DeleteAsync($"api/players/{command.PlayerId}");
                }
                else
                {
                    object patchDto;
                    if (command.Action == AdminPlayerCommandActions.ToggleAdmin)
                    {
                        patchDto = new { IsAdmin = command.NewValue };
                    }
                    else
                    {
                        patchDto = new { IsSpectator = command.NewValue };
                    }

                    response = await _httpClient.PatchAsJsonAsync(
                        $"api/players/{command.PlayerId}", patchDto);
                }

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                string? serverMessage = await response.Content.ReadJsonStringAsync();
                return (false, serverMessage ?? "Something went wrong. Please try again.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.PerformPlayerActionAsync failed: {ex.Message}");
                return (false, "Could not reach the server. Please check your connection.");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> CancelKillAsync(Guid killId, Guid adminPlayerId)
        {
            try
            {
                var dto = new { AdminPlayerId = adminPlayerId };
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                    $"api/kills/{killId}/reject", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                string? serverMessage = await response.Content.ReadJsonStringAsync();
                return (false, serverMessage ?? "Something went wrong. Please try again.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.CancelKillAsync failed: {ex.Message}");
                return (false, "Could not reach the server. Please check your connection.");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdatePlayerUsernameAsync(Guid playerId, string username)
        {
            try
            {
                var dto = new { UserName = username };
                HttpResponseMessage response = await _httpClient.PatchAsJsonAsync(
                    $"api/players/{playerId}", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                string? serverMessage = await response.Content.ReadJsonStringAsync();
                return (false, serverMessage ?? "Something went wrong. Please try again.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApiPlayerService.UpdatePlayerUsernameAsync failed: {ex.Message}");
                return (false, "Could not reach the server. Please check your connection.");
            }
        }

        #region Response DTOs for deserialization

        private class PlayerResponse
        {
            public Guid Id { get; set; }
            public string? UserName { get; set; }
            public bool IsAdmin { get; set; }
            public bool IsSpectator { get; set; }
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
            public Guid GameId { get; set; }
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
