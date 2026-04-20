namespace Gotcha.Maui.Models.Items
{
    public class KillItem
    {
        public string VictimName { get; set; } = string.Empty;
        public string VictimUsername { get; set; } = string.Empty;
        public string Weapon { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
    }
}
