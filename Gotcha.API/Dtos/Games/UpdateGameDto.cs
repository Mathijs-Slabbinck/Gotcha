namespace Gotcha.API.Dtos.Games
{
    public class UpdateGameDto
    {
        public string? Name { get; set; }
        public int? MaxPlayers { get; set; }
        public bool? HasStarted { get; set; }
        public bool? IsFinished { get; set; }
        public Guid? WinnerId { get; set; }
    }
}
