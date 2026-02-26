using Gotcha.Web.Areas.Player.ViewModels.BaseViewModels;

namespace Gotcha.Web.Areas.Player.ViewModels
{
    public class AdminViewModel
    {
        public bool HasStarted { get; set; }
        public string GameName { get; set; } = string.Empty;
        public string InviteLink { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
        public int MaxPlayers { get; set; }
        public List<AdminPlayerViewModel> Players { get; set; } = new List<AdminPlayerViewModel>();
        public List<AdminKillViewModel> PendingKills { get; set; } = new List<AdminKillViewModel>();

        // Game settings (from Rules) — pre-game
        public bool ShowPlayerImages { get; set; }
        public bool ShowGender { get; set; }
        public bool EnforcePlayerImages { get; set; }
        public bool ShowRealNames { get; set; }
        public bool ShowUsernames { get; set; }
        public bool ShowLivingPlayerCount { get; set; }
        public bool ShowLivingPlayerNames { get; set; }
        public bool ShowLivingPlayerNamesToDeath { get; set; }
        public bool CustomKillMethods { get; set; }
        public List<string>? CustomRules { get; set; }
        public List<string>? KillMethods { get; set; }

        // VIP-gated settings (during game)
        public bool AssassinModeUnlocked { get; set; }
        public bool IsAssassin { get; set; }
        public bool ShowHunter { get; set; }
        public bool ChaosModeUnlocked { get; set; }
        public bool IsChaos { get; set; }
        public int ChaosTimerMinHours { get; set; }
        public int ChaosTimerMaxHours { get; set; }
        public bool TimedKillsUnlocked { get; set; }
        public bool IsTimed { get; set; }
        public int TargetTimeOutHours { get; set; }
    }
}
