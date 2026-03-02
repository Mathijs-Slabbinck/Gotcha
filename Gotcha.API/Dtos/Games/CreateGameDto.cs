namespace Gotcha.API.Dtos.Games
{
    public class CreateGameDto
    {
        public string Name { get; set; } = "New game";
        public int MaxPlayers { get; set; } = 1;
        public Guid CreatorId { get; set; }
    }
}
