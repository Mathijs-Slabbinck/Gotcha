namespace Gotcha.API.Dtos.VipSettings
{
    public class UpdateVipSettingsDto
    {
        public bool? AssassinModeUnlocked { get; set; }
        public bool? ChaosModeUnlocked { get; set; }
        public bool? TimedKillsUnlocked { get; set; }
        public bool? CustomKillMethodsUnlocked { get; set; }
        public string? MaxLobbySize { get; set; }
        public string? UserPlan { get; set; }
    }
}
