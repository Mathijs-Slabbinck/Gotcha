namespace Gotcha.API.Dtos.Games
{
    public class GameResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool HasStarted { get; set; }
        public bool IsFinished { get; set; }
        public Guid? WinnerId { get; set; }
        public Guid CreatorId { get; set; }
        public List<Guid> AdminIds { get; set; } = new List<Guid>();
        public int MaxPlayers { get; set; }
    }
}
