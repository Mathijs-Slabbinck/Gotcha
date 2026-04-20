namespace Gotcha.Maui.Models.Items
{
    public class GameItem
    {
        public Guid GameId { get; init; }
        public string Name { get; set; } = string.Empty;
        public string CreatedDate { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string WinnerName { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
        public bool IsAlive { get; set; }
        public Guid PlayerId { get; set; }
    }
}
