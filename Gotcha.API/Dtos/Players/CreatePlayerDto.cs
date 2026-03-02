namespace Gotcha.API.Dtos.Players
{
    public class CreatePlayerDto
    {
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImageSource { get; set; }
    }
}
