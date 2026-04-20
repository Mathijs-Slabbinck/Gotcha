using Gotcha.Core.Enums;
using Gotcha.Shared.Enums;

namespace Gotcha.Core.Entities.Models
{
    public class VipSettings
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public bool AssassinModeUnlocked { get; set; } = false;
        public bool ChaosModeUnlocked { get; set; } = false;
        public bool TimedKillsUnlocked { get; set; } = false;
        public bool CustomKillMethodsUnlocked { get; set; } = false;
        public MaxLobbySize MaxLobbySize { get; set; } = MaxLobbySize.Small;
        public Plan UserPlan { get; set; } = Plan.Standard;
    }
}
