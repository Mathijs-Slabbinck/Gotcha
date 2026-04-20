using Gotcha.Maui.Enums;

namespace Gotcha.Maui.Models.Payloads
{
    public class PlayerActionCommand
    {
        public Guid PlayerId { get; set; }
        public AdminPlayerCommandActions Action { get; set; }

        // For toggle actions, the desired new value of IsAdmin/IsSpectator.
        // VM computes this by flipping the current state on the AdminPlayerItem,
        // avoiding a separate GET before the PATCH.
        public bool NewValue { get; set; }
    }
}
