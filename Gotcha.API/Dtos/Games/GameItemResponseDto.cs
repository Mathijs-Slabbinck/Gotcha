namespace Gotcha.API.Dtos.Games
{
    public class GameItemResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool HasStarted { get; set; }
        public bool IsFinished { get; set; }
        public string? WinnerName { get; set; }
        public int PlayerCount { get; set; }
        public bool IsAlive { get; set; }
        public Guid PlayerId { get; set; }
    }
}
