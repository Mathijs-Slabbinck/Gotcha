namespace Gotcha.Web.Areas.User.ViewModels
{
    public class GameItemViewModel
    {
        public Guid GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? WinnerName { get; set; }
        public int PlayerCount { get; set; }
        public bool IsAlive { get; set; }
    }
}
