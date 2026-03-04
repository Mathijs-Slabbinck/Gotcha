using System;
using System.Collections.Generic;
using System.Linq;

namespace Gotcha.Core.Entities.Models
{
    public class Game
    {
        #region Properties
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = "New game";
        public DateTime CreationDate { get; init; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Kill> Kills { get; set; } = new List<Kill>();
        public Rules Rules { get; set; } = new Rules();
        public bool HasStarted { get; set; } = false;
        public bool IsFinished { get; set; } = false;
        public Guid? WinnerId { get; set; }
        public ICollection<Guid> AdminIds { get; set; } = new List<Guid>();
        public int MaxPlayers { get; set; } = 1;
        public Guid CreatorId { get; set; } = Guid.Empty;
        #endregion

        #region Computed Properties
        public Player? Winner
        {
            get { return Players?.FirstOrDefault(p => p.Id == WinnerId); }
            set { WinnerId = value?.Id; }
        }

        public IEnumerable<Player> Admins
        {
            get
            {
                return Players
                        .Where(p => AdminIds.Contains(p.Id));
            }
        }

        public Player? Creator
        {
            get { return Players?.FirstOrDefault(p => p.Id == CreatorId); }
        }
        #endregion

        #region Query Methods
        public IEnumerable<Player> GetLivingPlayers()
        {
            return Players
                    .Where(p => p.IsAlive);
        }

        public IEnumerable<Player> GetEliminatedPlayers()
        {
            return Players
                    .Where(p => !p.IsAlive);
        }

        public IEnumerable<Kill> GetDisputedKills()
        {
            return Kills
                    .Where(k => !k.IsValid);
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
