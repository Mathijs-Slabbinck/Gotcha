using Gotcha.Maui.Models.Items;
using Gotcha.Maui.Models.PageData;
using Gotcha.Maui.Models.Payloads;

namespace Gotcha.Maui.Services.Mock
{
    public class MockPlayerService : IPlayerService
    {
        public Task<(PlayerHomeData? Data, string? ErrorMessage)> GetPlayerHomeDataAsync(Guid playerId)
        {
            var data = new PlayerHomeData
            {
                IsAlive = true,
                IsSpectator = false,
                TargetName = "Player Bravo",
                TargetUsername = "BravoFox",
                Weapon = "Water Gun",
                AssignmentExpirationDate = new DateTime(2026, 3, 20, 18, 0, 0),
                IsAssassin = true,
                IsChaos = false,
                IsTimed = true,
                ChaosTimerMinHours = 2,
                ChaosTimerMaxHours = 6,
                TargetTimeOutHours = 48,
                ShowHunter = true,
                HunterName = "Player Charlie",
                HunterOtherName = "CharlieGhost",
                StartDate = new DateTime(2026, 3, 1),
                ShowLivingPlayerCount = true,
                ShowLivingPlayerNames = true,
                CustomRules = new List<string>
                {
                    "No kills inside classrooms",
                    "Safe zones: library and cafeteria"
                },
                Kills = new List<KillItem>
                {
                    new KillItem
                    {
                        VictimName = "Player Delta",
                        VictimUsername = "DeltaHawk",
                        Weapon = "Water Gun",
                        TimeStamp = new DateTime(2026, 3, 5, 14, 30, 0)
                    },
                    new KillItem
                    {
                        VictimName = "Player Echo",
                        VictimUsername = "EchoRanger",
                        Weapon = "Nerf Dart",
                        TimeStamp = new DateTime(2026, 3, 8, 9, 15, 0)
                    }
                },
                Players = new List<PlayerItem>
                {
                    new PlayerItem { Name = "Player Alpha", OtherName = "AlphaWolf", IsAlive = true },
                    new PlayerItem { Name = "Player Bravo", OtherName = "BravoFox", IsAlive = true },
                    new PlayerItem { Name = "Player Charlie", OtherName = "CharlieGhost", IsAlive = true },
                    new PlayerItem { Name = "Player Delta", OtherName = "DeltaHawk", IsAlive = false }
                }
            };

            return Task.FromResult<(PlayerHomeData?, string?)>((data, null));
        }

        public Task<(ConfirmKillData? Data, string? ErrorMessage)> GetConfirmKillDataAsync(Guid playerId)
        {
            var data = new ConfirmKillData
            {
                TargetName = "Player Bravo",
                TargetUsername = "BravoFox",
                Weapon = "Water Gun",
                HunterName = "Player Charlie",
                HunterUsername = "CharlieGhost",
                IsAssassinMode = true,
                ShowHunter = true
            };

            return Task.FromResult<(ConfirmKillData?, string?)>((data, null));
        }

        public Task<(string? Data, string? ErrorMessage)> GetPlayerUsernameAsync(Guid playerId)
        {
            return Task.FromResult<(string?, string?)>(("AlphaWolf", null));
        }

        public Task<(AdminData? Data, string? ErrorMessage)> GetAdminDataAsync(Guid playerId)
        {
            var data = new AdminData
            {
                GameId = Guid.NewGuid(),
                HasStarted = false,
                GameName = "Friday Night Gotcha",
                InviteLink = "https://gotcha.app/join/abc123",
                PlayerCount = 8,
                MaxPlayers = 50,
                ShowRealNames = true,
                AssassinModeUnlocked = true,
                TimedKillsUnlocked = true,
                Players = new List<AdminPlayerItem>
                {
                    new AdminPlayerItem
                    {
                        PlayerId = Guid.NewGuid(),
                        Name = "Player Alpha",
                        Username = "AlphaWolf",
                        HasImage = true,
                        IsAdmin = true,
                        IsSpectator = false
                    },
                    new AdminPlayerItem
                    {
                        PlayerId = Guid.NewGuid(),
                        Name = "Player Bravo",
                        Username = "BravoFox",
                        HasImage = false,
                        IsAdmin = false,
                        IsSpectator = false
                    },
                    new AdminPlayerItem
                    {
                        PlayerId = Guid.NewGuid(),
                        Name = "Player Charlie",
                        Username = "CharlieGhost",
                        HasImage = true,
                        IsAdmin = false,
                        IsSpectator = true
                    }
                },
                PendingKills = new List<AdminKillItem>
                {
                    new AdminKillItem
                    {
                        KillId = Guid.NewGuid(),
                        KillerName = "AlphaWolf",
                        VictimName = "BravoFox",
                        Weapon = "Water Gun",
                        Moment = new DateTime(2026, 3, 10, 15, 30, 0)
                    }
                }
            };

            return Task.FromResult<(AdminData?, string?)>((data, null));
        }

        public Task<(bool Success, string? ErrorMessage)> ConfirmKillAsync(Guid playerId)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<(bool Success, string? ErrorMessage)> ConfirmDeathAsync(Guid playerId)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<(bool Success, string? ErrorMessage)> ConfirmHunterKillAsync(Guid playerId)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<(bool Success, string? ErrorMessage)> PerformPlayerActionAsync(PlayerActionCommand command)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<(bool Success, string? ErrorMessage)> CancelKillAsync(Guid killId, Guid adminPlayerId)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<(bool Success, string? ErrorMessage)> UpdatePlayerUsernameAsync(Guid playerId, string username)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }
    }
}
