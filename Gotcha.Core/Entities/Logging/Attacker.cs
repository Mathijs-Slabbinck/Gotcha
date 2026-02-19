namespace Gotcha.Core.Entities.Logging
{
    // Stores info about users who triggered security violations (e.g. invalid input, suspicious requests)
    public class Attacker
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Referer { get; set; }
        public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
        public string Path { get; set; } = string.Empty;
        public string? InvalidInput { get; set; }
        public string? SessionId { get; set; }
        public string? MacAddress { get; set; }
        public Guid UserId { get; set; } = Guid.Empty;
    }
}
