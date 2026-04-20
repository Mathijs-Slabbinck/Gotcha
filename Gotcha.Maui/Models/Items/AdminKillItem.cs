namespace Gotcha.Maui.Models.Items
{
    public class AdminKillItem
    {
        public Guid KillId { get; init; }
        public string KillerName { get; set; } = string.Empty;
        public string VictimName { get; set; } = string.Empty;
        public string Weapon { get; set; } = string.Empty;
        public DateTime Moment { get; set; }
    }
}
