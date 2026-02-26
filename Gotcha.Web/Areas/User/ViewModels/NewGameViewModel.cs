namespace Gotcha.Web.Areas.User.ViewModels
{
    public class NewGameViewModel
    {
        public string Name { get; set; } = string.Empty;
        public List<string>? CustomRules { get; set; }

        // Display settings
        public bool ShowPlayerImages { get; set; }
        public bool ShowGender { get; set; }
        public bool EnforcePlayerImages { get; set; }
        public bool ShowRealNames { get; set; } = true;
        public bool ShowUsernames { get; set; }
        public bool ShowLivingPlayerCount { get; set; }
        public bool ShowLivingPlayerNames { get; set; }
        public bool ShowLivingPlayerNamesToDeath { get; set; }

        // Game modes
        public bool IsAssassin { get; set; }
        public bool ShowHunter { get; set; }
        public bool IsChaos { get; set; }
        public int ChaosTimerMinHours { get; set; }
        public int ChaosTimerMaxHours { get; set; }
        public bool IsTimed { get; set; }
        public int TargetTimeOutHours { get; set; }
        public bool CustomKillMethods { get; set; }
        public List<string>? KillMethods { get; set; }

        // VIP unlock flags (to disable paid features if not owned)
        public bool AssassinModeUnlocked { get; set; }
        public bool ChaosModeUnlocked { get; set; }
        public bool TimedKillsUnlocked { get; set; }

        // Join link (generated on creation)
        public string InviteLink { get; set; } = string.Empty;
    }
}
