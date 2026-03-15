namespace Gotcha.API.Dtos.Players
{
    public class ConfirmKillResponseDto
    {
        public string TargetName { get; set; } = string.Empty;
        public string TargetUsername { get; set; } = string.Empty;
        public string? Weapon { get; set; }
        public string HunterName { get; set; } = string.Empty;
        public string HunterUsername { get; set; } = string.Empty;
        public bool IsAssassinMode { get; set; }
        public bool ShowHunter { get; set; }
    }
}
