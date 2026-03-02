namespace Gotcha.API.Dtos.Attackers
{
    public class AttackerResponseDto
    {
        public Guid Id { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Referer { get; set; }
        public DateTime TimeStamp { get; set; }
        public required string Path { get; set; }
        public string? InvalidInput { get; set; }
        public string? SessionId { get; set; }
        public string? MacAddress { get; set; }
        public Guid UserId { get; set; }
    }
}
