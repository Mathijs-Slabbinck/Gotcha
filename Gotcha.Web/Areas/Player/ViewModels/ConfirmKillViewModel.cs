using Gotcha.Core.Enums;

namespace Gotcha.Web.Areas.Player.ViewModels
{
    public class ConfirmKillViewModel
    {
        // Target info (who this player needs to kill)
        public string? TargetName { get; set; }
        public string? TargetUsername { get; set; }
        public string? TargetProfileImgSource { get; set; }
        public Genders? TargetGender { get; set; }

        // Hunter info (who is hunting this player — only relevant in assassin mode)
        public string? HunterName { get; set; }
        public string? HunterUsername { get; set; }
        public string? HunterProfileImgSource { get; set; }
        public Genders? HunterGender { get; set; }

        // Weapon assigned to this player for their target kill
        public string? Weapon { get; set; }

        // Display settings
        public bool ShowPlayerImages { get; set; } = false;
        public bool ShowGender { get; set; } = false;

        // Whether assassin mode is enabled (allows confirming you killed your hunter)
        public bool IsAssassinMode { get; set; } = false;

        // Whether the player can see who their hunter is
        public bool ShowHunter { get; set; } = false;
    }
}
