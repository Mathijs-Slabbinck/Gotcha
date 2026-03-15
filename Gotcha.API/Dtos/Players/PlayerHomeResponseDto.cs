namespace Gotcha.API.Dtos.Players
{
    public class PlayerHomeResponseDto
    {
        // Status
        public bool IsAlive { get; set; }
        public bool IsSpectator { get; set; }

        // Target
        public string TargetName { get; set; } = string.Empty;
        public string TargetUsername { get; set; } = string.Empty;
        public string? Weapon { get; set; }
        public DateTime? AssignmentExpirationDate { get; set; }

        // Game rules
        public bool IsAssassin { get; set; }
        public bool IsChaos { get; set; }
        public bool IsTimed { get; set; }
        public int ChaosTimerMinHours { get; set; }
        public int ChaosTimerMaxHours { get; set; }
        public int TargetTimeOutHours { get; set; }
        public List<string> CustomRules { get; set; } = new();

        // Hunter info
        public bool ShowHunter { get; set; }
        public string HunterName { get; set; } = string.Empty;
        public string HunterOtherName { get; set; } = string.Empty;

        // Game stats
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string WinnerName { get; set; } = string.Empty;
        public string WinnerOtherName { get; set; } = string.Empty;
        public DateTime? KilledOnDate { get; set; }
        public string KillerName { get; set; } = string.Empty;
        public string KillerOtherName { get; set; } = string.Empty;

        // Visibility
        public bool ShowLivingPlayerCount { get; set; }
        public bool ShowLivingPlayerNames { get; set; }

        // Collections
        public List<KillItemDto> Kills { get; set; } = new();
        public List<PlayerItemDto> Players { get; set; } = new();
    }

    public class KillItemDto
    {
        public string VictimName { get; set; } = string.Empty;
        public string VictimUsername { get; set; } = string.Empty;
        public string? Weapon { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class PlayerItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string OtherName { get; set; } = string.Empty;
        public bool IsAlive { get; set; }
    }
}
