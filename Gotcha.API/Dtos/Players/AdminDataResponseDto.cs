namespace Gotcha.API.Dtos.Players
{
    public class AdminDataResponseDto
    {
        public Guid GameId { get; set; }
        public bool HasStarted { get; set; }
        public string GameName { get; set; } = string.Empty;
        public string InviteLink { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
        public int MaxPlayers { get; set; }
        public List<AdminPlayerItemDto> Players { get; set; } = new();
        public List<AdminKillItemDto> PendingKills { get; set; } = new();

        // Display settings
        public bool ShowPlayerImages { get; set; }
        public bool ShowGender { get; set; }
        public bool EnforcePlayerImages { get; set; }
        public bool ShowRealNames { get; set; }
        public bool ShowUsernames { get; set; }
        public bool ShowLivingPlayerCount { get; set; }
        public bool ShowLivingPlayerNames { get; set; }
        public bool ShowLivingPlayerNamesToDeath { get; set; }

        // Game mode
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

        // VIP unlocks
        public bool AssassinModeUnlocked { get; set; }
        public bool ChaosModeUnlocked { get; set; }
        public bool TimedKillsUnlocked { get; set; }
    }

    public class AdminPlayerItemDto
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool HasImage { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSpectator { get; set; }
    }

    public class AdminKillItemDto
    {
        public Guid KillId { get; set; }
        public string KillerName { get; set; } = string.Empty;
        public string VictimName { get; set; } = string.Empty;
        public string? Weapon { get; set; }
        public DateTime Moment { get; set; }
    }
}
