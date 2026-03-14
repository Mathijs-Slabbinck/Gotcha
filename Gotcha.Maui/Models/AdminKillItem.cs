namespace Gotcha.Maui.Models
{
    public class AdminKillItem
    {
        public Guid KillId { get; set; }
        public string KillerName { get; set; } = string.Empty;
        public string VictimName { get; set; } = string.Empty;
        public string Weapon { get; set; } = string.Empty;
        public DateTime Moment { get; set; }
    }
}
