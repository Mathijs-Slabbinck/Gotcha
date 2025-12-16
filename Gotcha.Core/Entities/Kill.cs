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
        public string Reason { get; set; }
        public bool IsValid { get; set; } = true
        public string? InvalidReason { get; set; }
        public TimeSpan TimeSinceAssignedTarget { get; set; }
    }
}
