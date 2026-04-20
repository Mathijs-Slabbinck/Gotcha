namespace Gotcha.Maui.Models.Payloads
{
    public class UpdateGameSettingsCommand
    {
        public Guid GameId { get; set; }
        public Guid AdminPlayerId { get; set; }
        public string? GameName { get; set; }

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
        public string? KillMethods { get; set; }
        public int? ChaosTimerMinHours { get; set; }
        public int? ChaosTimerMaxHours { get; set; }
        public int? TargetTimeOutHours { get; set; }
        public string? CustomRulesText { get; set; }
    }
}
