namespace Gotcha.Core.Entities.Models
{
    public class Kill
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid GameId { get; init; }
        public Game Game { get; init; }
        public Guid KillerId { get; init; }
        public Player Killer { get; init; }
        public Guid VictimId { get; init; }
        public Player Victim { get; init; }
        public DateTime Moment { get; init; } = DateTime.UtcNow;
        public string? Weapon { get; init; }
        public string Reason { get; set; } = "Killed in game!";
        public bool IsValid { get; set; } = true;
        public string KillMessage { get; init; } = string.Empty;
        public TimeSpan TimeSinceAssignedTarget { get; init; } = TimeSpan.Zero;
    }
}
