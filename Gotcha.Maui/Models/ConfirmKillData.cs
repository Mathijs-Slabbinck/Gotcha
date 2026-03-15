namespace Gotcha.Maui.Models
{
    public class ConfirmKillData
    {
        public string TargetName { get; set; } = string.Empty;
        public string TargetUsername { get; set; } = string.Empty;
        public string Weapon { get; set; } = string.Empty;
        public string HunterName { get; set; } = string.Empty;
        public string HunterUsername { get; set; } = string.Empty;
        public bool IsAssassinMode { get; set; }
        public bool ShowHunter { get; set; }
    }
}
