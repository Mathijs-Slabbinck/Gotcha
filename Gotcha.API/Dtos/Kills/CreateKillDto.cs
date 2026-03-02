namespace Gotcha.API.Dtos.Kills
{
    public class CreateKillDto
    {
        public Guid GameId { get; set; }
        public Guid KillerId { get; set; }
        public Guid VictimId { get; set; }
        public string? Weapon { get; set; }
        public string? Reason { get; set; }
        public string? KillMessage { get; set; }
    }
}
