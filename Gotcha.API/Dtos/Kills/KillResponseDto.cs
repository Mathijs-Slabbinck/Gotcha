namespace Gotcha.API.Dtos.Kills
{
    public class KillResponseDto
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid KillerId { get; set; }
        public Guid VictimId { get; set; }
        public DateTime Moment { get; set; }
        public string? Weapon { get; set; }
        public required string Reason { get; set; }
        public bool IsValid { get; set; }
        public required string KillMessage { get; set; }
        public TimeSpan TimeSinceAssignedTarget { get; set; }
    }
}
