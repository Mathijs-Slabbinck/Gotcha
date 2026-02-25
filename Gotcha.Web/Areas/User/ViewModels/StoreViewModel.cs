using Gotcha.Core.Enums;

namespace Gotcha.Web.Areas.User.ViewModels
{
    public class StoreViewModel
    {
        // Current unlocked features
        public bool AssassinUnlocked { get; set; }
        public bool ChaosUnlocked { get; set; }
        public bool TimedKillsUnlocked { get; set; }
        public bool CustomKillMethodsUnlocked { get; set; }

        // Current lobby size tier
        public MaxLobbySize CurrentLobbySize { get; set; }

        // Current subscription plan
        public Plan CurrentPlan { get; set; }
    }
}
