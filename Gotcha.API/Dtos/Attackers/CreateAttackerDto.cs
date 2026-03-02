namespace Gotcha.API.Dtos.Attackers
{
    public class CreateAttackerDto
    {
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Referer { get; set; }
        public required string Path { get; set; }
        public string? InvalidInput { get; set; }
        public string? SessionId { get; set; }
        public string? MacAddress { get; set; }
        public Guid UserId { get; set; }
    }
}
