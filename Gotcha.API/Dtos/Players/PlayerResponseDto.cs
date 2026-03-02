namespace Gotcha.API.Dtos.Players
{
    public class PlayerResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImageSource { get; set; }
        public bool IsAlive { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSpectator { get; set; }
        public required string Notes { get; set; }
    }
}
