using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
        public string PlayerName { get; set; }
        public string? ProfileImageSource { get; set; }
        public bool IsAlive { get; set; }
        public string Notes { get; set; }
        public ICollection<TargetAssignment> TargetAssignments { get; set; }
    }
}
