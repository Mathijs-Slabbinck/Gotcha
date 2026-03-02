namespace Gotcha.API.Dtos.VipSettings
{
    public class VipSettingsResponseDto
    {
        public Guid Id { get; set; }
        public bool AssassinModeUnlocked { get; set; }
        public bool ChaosModeUnlocked { get; set; }
        public bool TimedKillsUnlocked { get; set; }
        public bool CustomKillMethodsUnlocked { get; set; }
        public required string MaxLobbySize { get; set; }
        public required string UserPlan { get; set; }
    }
}
