using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Entities
{
    public class Kill
    {
        private readonly Guid _id;
        private readonly Guid _gameId;
        private readonly Game _game;
        private readonly Guid _killerId;
        private readonly Player _killer;
        private readonly Guid _victimId;
        private readonly Player _victim;
        private readonly DateTime _moment;
        private readonly string? _weapon;
        private readonly string _killMessage;
        private readonly TimeSpan _timeSinceAssignedTarget;
        private string reason;
        private bool isValid;

        /*
         * Constructor without weapon and reason
         * Used when creating a new Kill object during gameplay, providing only the essential information */
        public Kill(Game game, Player killer, Player victim)
        {
            DateTime killMoment = DateTime.UtcNow;
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(killMoment, localTimeZone);

            _id = Guid.NewGuid();
            _gameId = game.Id;
            _game = game;
            _killerId = killer.Id;
            _killer = killer;
            _victimId = victim.Id;
            _victim = victim;
            _moment = killMoment;
            _killMessage = $"{killer} has killed {victim} on {localTime.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")}";
            reason = $"{killer} has killed {victim} on {localTime.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")}";
            isValid = true;
            _timeSinceAssignedTarget = (killMoment - killer.TargetAssignments
                                            .FirstOrDefault(ta => ta.TargetId == victim.Id && ta.Kill == null)?
                                            .TargetAssigned) ?? TimeSpan.Zero;

        }

        /* Constructor including weapon but no reason
         * Used when creating a new Kill object during gameplay, providing weapon information 
         * This is needed for if the Rule 'CustomWeapons' gets enabled */
        public Kill(Game game, Player killer, Player victim, string? weapon) : this(game, killer, victim)
        {
            _weapon = weapon;
        }

        /* Constructor including a reason
         * Used when creating a new Kill object and wanting to provide a custom reason
         * Since we couldn't do constructor chaining for weapon, we made it nullable
         * This way it can also be used when the 'CustomWeapons' rules is disabled */
        public Kill(Game game, Player killer, Player victim, string? weapon, string? reason) : this(game, killer, victim, weapon)
        {
            if(reason == null)
            {
                reason = "Killed in game!";
            }
            Reason = reason;
            _weapon = weapon;
            _killMessage = $"{killer} has killed {victim} on {Moment.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")} using: {weapon}";
        }

        /* Constructor when the kill is invalid
         * Used when creating a new Kill object that is invalid
         * reason is optional, if not provided a default message will be used */
        public Kill(Game game, Player killer, Player victim, string? weapon, string? reason, bool isValid) : this(game, killer, victim, weapon, reason)
        {
            if (!isValid)
            {
                if(weapon == null)
                {
                    _killMessage = $"{killer} has attempted to kill {victim} on {Moment.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")}";
                }
                else
                {
                    _killMessage = $"{killer} has attempted to kill {victim} on {Moment.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")} using: {weapon}";
                }
                Reason = reason ?? "No reason provided.";
            }
            else
            {
                if (reason == null)
                {
                    if(weapon == null)
                    {
                        _killMessage = $"{killer} has killed {victim} on {Moment.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")}";
                    }
                    else
                    {
                        _killMessage = $"{killer} has killed {victim} on {Moment.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")} using: {weapon}";
                    }
                }
                Reason = "Killed in game!";
            }
        }

        /* Constructor including all parameters
         * Used for reconstructing Kill objects from the database */
        public Kill(Guid id, Game game, Player killer, Player victim, string reason, string? weapon, bool isValid, DateTime moment, string killMessage, TimeSpan timeSinceAssignedTarget) : this(game, killer, victim)
        {
            _id = id;
            _gameId = game.Id;
            _game = game;
            _killerId = killer.Id;
            _killer = killer;
            _victimId = victim.Id;
            _victim = victim;
            Reason = reason;
            _weapon = weapon;
            IsValid = isValid;
            _moment = moment;
            _killMessage = killMessage;
            _timeSinceAssignedTarget = timeSinceAssignedTarget;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public Guid GameId
        {
            get { return _gameId; }
        }

        public Game Game
        {
            get { return _game; }
        }

        public Guid KillerId
        {
            get { return _killerId; }
        }

        public Player Killer
        {
            get { return _killer; }
        }

        public Guid VictimId
        {
            get { return _victimId; }
        }

        public Player Victim
        {
            get { return _victim; }
        }

        public DateTime Moment
        {
            get { return _moment; }
        }

        public string? Weapon
        {
            get { return _weapon; }
        }
            
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value;  }
        }

        public string KillMessage
        {
            get { return _killMessage; }
        }

        public TimeSpan TimeSinceAssignedTarget
        {
            get { return _timeSinceAssignedTarget; }
        }
    }
}
