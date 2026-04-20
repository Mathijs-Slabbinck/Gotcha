using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services.Mock
{
    public class MockPlayerService : IPlayerService
    {
        public Task<PlayerHomeData> GetPlayerHomeDataAsync(Guid playerId)
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

            return Task.FromResult(data);
        }

        public Task<ConfirmKillData> GetConfirmKillDataAsync(Guid playerId)
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

            return Task.FromResult(data);
        }

        public Task<string> GetPlayerUsernameAsync(Guid playerId)
        {
            return Task.FromResult("AlphaWolf");
        }

        public Task<AdminData> GetAdminDataAsync(Guid playerId)
        {
            var data = new AdminData
            {
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

            return Task.FromResult(data);
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

        public Task<bool> UpdatePlayerUsernameAsync(Guid playerId, string username)
        {
            return Task.FromResult(true);
        }
    }
}
