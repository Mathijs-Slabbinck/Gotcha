using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Entities
{
    public class Kill
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
        public Guid KillerId { get; set; }
        public Player Killer { get; set; }
        public Guid VictimId { get; set; }
        public Player Victim { get; set; }
        public DateTime Moment { get; set; }
        public string Weapon { get; set; }
        public string Reason { get; set; }
        public bool IsValid { get; set; }
        public string? InvalidReason { get; set; }
        public TimeSpan TimeSinceAssignedTarget { get; set; }

        public Kill(Game game, Player killer, Player victim)
        {
            DateTime killMoment = DateTime.UtcNow;
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(killMoment, localTimeZone);

            Id = Guid.NewGuid();
            Game = game;
            Killer = killer;
            Victim = victim;
            Moment = killMoment;
            Reason = $"{killer} has killed {victim} on {localTime.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")}";
            IsValid = true;
            TimeSinceAssignedTarget = killMoment - killer.TargetAssignMents
                                                        .FirstOrDefault(ta => ta.AssignedPlayerId == victim.Id && ta.IsActive)?
                                                        .AssignedMoment ?? TimeSpan.Zero;
        }

        public Kill() : this()
        {
            Id = Guid.NewGuid();

        }
    }
}
