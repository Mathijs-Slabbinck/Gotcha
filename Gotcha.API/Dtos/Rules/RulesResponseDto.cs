namespace Gotcha.API.Dtos.Rules
{
    public class RulesResponseDto
    {
        public Guid Id { get; set; }
        public bool IsAssassin { get; set; }
        public bool ShowHunter { get; set; }
        public bool ShowPlayerImages { get; set; }
        public bool ShowGender { get; set; }
        public bool EnforcePlayerImages { get; set; }
        public bool ShowRealNames { get; set; }
        public bool ShowUsernames { get; set; }
        public bool ShowLivingPlayerCount { get; set; }
        public bool ShowLivingPlayerNames { get; set; }
        public bool ShowLivingPlayerNamesToDeath { get; set; }
        public bool IsTimed { get; set; }
        public TimeSpan TargetTimeOut { get; set; }
        public List<string>? CustomRules { get; set; }
        public bool CustomKillMethods { get; set; }
        public List<string>? KillMethods { get; set; }
        public bool IsChaos { get; set; }
        public TimeSpan ChaosTimerMin { get; set; }
        public TimeSpan ChaosTimerMax { get; set; }
        public TimeSpan KillConfirmationTimer { get; set; }
    }
}
