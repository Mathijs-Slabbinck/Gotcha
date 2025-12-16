using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public  DateTime? StartDate { get; set; }
        public List<Player> Players { get; set; }
        public List<Kill> Kills { get; set; }
        public Rules Rules { get; set; }
        public List<string>? KillMethods { get; set; }
        public bool HasStarted { get; set; }
        public bool IsFinished { get; set; }
        public Player? Winner { get; set; }
        public List<Player> Admins { get; set; }
    }
}
