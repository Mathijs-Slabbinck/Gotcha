namespace Gotcha.Core.Entities.Models
{
    public class Rules
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public bool IsAssassin { get; set; } = false;
        public bool ShowPlayerImages { get; set; } = false;
        public bool EnforcePlayerImages { get; set; } = false;
        public bool ShowRealNames { get; set; } = true;
        public bool ShowUsernames { get; set; } = false;
        public bool ShowLivingPlayerCount { get; set; } = true;
        public bool ShowLivingPlayerNames { get; set; } = false;
        public bool ShowLivingPlayerNamesToDeath { get; set; } = false;
        public bool IsTimed { get; set; } = false;
        public TimeSpan TargetTimeOut { get; set; } = Timeout.InfiniteTimeSpan;
        public string? CustomRules { get; set; }
        public bool CustomKillMethods { get; set; } = false;
        public List<string>? KillMethods { get; set; }
        public bool IsChaos { get; set; } = false;
        public TimeSpan ChaosTimer { get; set; } = Timeout.InfiniteTimeSpan;
        public TimeSpan KillConfirmationTimer { get; set; } = TimeSpan.FromMinutes(10);
    }
}
