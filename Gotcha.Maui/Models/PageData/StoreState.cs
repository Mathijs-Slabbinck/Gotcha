using Gotcha.Shared.Enums;

namespace Gotcha.Maui.Models.PageData
{
    public class StoreState
    {
        public bool AssassinUnlocked { get; set; }
        public bool ChaosUnlocked { get; set; }
        public bool TimedKillsUnlocked { get; set; }
        public bool CustomKillMethodsUnlocked { get; set; }
        public bool Lobby100Owned { get; set; }
        public bool Lobby150Owned { get; set; }
        public bool Lobby500Owned { get; set; }
        public bool Lobby10000Owned { get; set; }
        public Plan CurrentPlan { get; set; } = Plan.Standard;
    }
}
