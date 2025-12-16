using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Enums;
using Gotcha.Core.Services;

namespace Gotcha.Core.Entities
{
    public class Game
    {
        #region Fields
        private readonly Guid _id;
        private string name;
        private readonly DateTime _creationDate;
        private DateTime? startDate;
        private List<Player> players;
        private List<Kill> kills;
        private Rules rules;
        private bool hasStarted;
        private bool isFinished;
        private Player? winner;
        private List<Player> admins;
        private int maxPlayers;
        private readonly Player _creator;
        #endregion

        #region Constructors

        #region New Game Constructors

        /* Constructor for creating a new game with only the creation player (before they add others)
         * The creator will be the only player and admin initially
         * Used when a player creates a new game without adding other players or admins */
        public Game(string name, Player creationPlayer, Rules rules)
        {
            Name = name;
            _creationDate = DateTime.UtcNow;
            Players = new List<Player> { creationPlayer };
            Kills = new List<Kill>();
            Rules = rules;
            HasStarted = false;
            IsFinished = false;
            Admins = new List<Player> { creationPlayer };

            if (creationPlayer.User.plan == Plan.Standard)
            {
                MaxPlayers = 50;
            }

            if (creationPlayer.User.plan == Plan.Premium)
            {
                MaxPlayers = 250;
            }

            if (creationPlayer.User.plan == Plan.Deluxe)
            {
                MaxPlayers = 750;
            }

            _creator = creationPlayer;
        }

        /* Constructor for creating a new game with multiple players but no admins were selected
         * Se we request the admin param as the creator will be the only admin initially
         * Used when a player creates a new game and adds other players but no admins */
        public Game(string name, List<Player> players, Rules rules, Player admin)
        {
            Name = name;
            _creationDate = DateTime.UtcNow;
            Players = players;
            Kills = new List<Kill>();
            Rules = rules;
            HasStarted = false;
            IsFinished = false;
            Admins = new List<Player> { admin };

            if (admin.User.plan == Plan.Standard)
            {
                MaxPlayers = 50;
            }

            if (admin.User.plan == Plan.Premium)
            {
                MaxPlayers = 250;
            }

            if (admin.User.plan == Plan.Deluxe)
            {
                MaxPlayers = 750;
            }

            _creator = admin;
        }

        /* Constructor for creating a new game with multiple players and multiple admins
         * Used when a player creates a new game and adds other players and admins */
        public Game(string name, List<Player> players, Rules rules, List<Player> admins, Player creator)
        {
            Name = name;
            _creationDate = DateTime.UtcNow;
            Players = players;
            Kills = new List<Kill>();
            Rules = rules;
            HasStarted = false;
            IsFinished = false;
            Admins = admins;

            if (creator.User.plan == Plan.Standard)
            {
                MaxPlayers = 50;
            }

            if (creator.User.plan == Plan.Premium)
            {
                MaxPlayers = 250;
            }

            if (creator.User.plan == Plan.Deluxe)
            {
                MaxPlayers = 750;
            }

            _creator = creator;
        }
        #endregion

        /* Constructor for pre-started games loaded from the database
         * Used when trying to load a game from the database that hasn't started yet and doesn't have a start dare set  */
        public Game(Guid id, string name, List<Player> players, Rules rules, DateTime creationDate, List<Player> admins, Player creator)
        {
            _id = id;
            Name = name;
            _creationDate = creationDate;
            Players = players;
            Kills = new List<Kill>();
            Rules = rules;
            HasStarted = false;
            IsFinished = false;
            Admins = admins;
            _creator = creator;
        }

        /* Constructor for pre-started games loaded from the database
         * Used when trying to load a game from the database that hasn't started yet (but does have a startdate set)  */
        public Game(Guid id, string name, List<Player> players, Rules rules, DateTime creationDate, List<Player> admins, Player creator, DateTime startDate) : this(id, name, players, rules, creationDate, admins, creator)
        {
            StartDate = startDate;
        }

        /* Constructors for games that just started
         * Used when trying to load a game from the database that has started but has no Kills yet
         */
        public Game(Guid id, string name, List<Player> players, Rules rules, DateTime creationDate, List<Player> admins, Player creator, DateTime startDate, bool hasStarted) : this(id, name, players, rules, creationDate, admins, creator, startDate)
        {
            HasStarted = hasStarted;
        }

        /* Constructors for games that are ongoing
         * Used when trying to load a game from the database that has started (and has killed players)
         */
        public Game(Guid id, string name, List<Player> players, Rules rules, DateTime creationDate, List<Player> admins, Player creator, DateTime startDate, bool hasStarted, List<Kill> kills) : this(id, name, players, rules, creationDate, admins, creator, startDate, hasStarted)
        {
            Kills = kills;
        }

        /* Constructors for games that are ongoing
         * Used when trying to load a game from the database that has ended
         * (it doesn't need an IsFinished parameter as it's implied by having a winner)
         */
        public Game(Guid id, string name, List<Player> players, Rules rules, DateTime creationDate, List<Player> admins, Player creator, DateTime startDate, bool hasStarted, List<Kill> kills, Player winner) : this(id, name, players, rules, creationDate, admins, creator, startDate, hasStarted, kills)
        {
            IsFinished = true;
            Winner = winner;
        }

        #endregion

        public Guid Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return name; }
            set {
                if (!ThirdLineValidationService.IsValidLength(value, 2, 25))
                {
                    throw new ArgumentException("Username must be between 2 and 25 characters long.");
                }

                string sanitizedValue = ThirdLineValidationService.SaniziteInput(value);

                if (ThirdLineValidationService.IsInputClean(sanitizedValue))
                {
                    name = sanitizedValue;
                }
                else
                {
                    throw new ArgumentException("Invalid characters in username.");
                }
            }
        }

        public DateTime CreationDate
        {
            get { return _creationDate; }
        }

        public DateTime? StartDate
        {
            get { return startDate; }
            set {
                if (value == null)
                {
                    startDate = null;
                    return;
                }

                value = value.Value.ToUniversalTime();
                if ((value.Value.Year - DateTime.UtcNow.Year) > 2)
                {
                    throw new ArgumentException("Start date cannot be more than 2 years in the future.");
                }
                else if (value <= DateTime.UtcNow)
                {
                    throw new ArgumentException("Start date cannot be in the past.");
                }

                startDate = value;
            }
        }

        public List<Player> Players
        {
            get { return players; }
            set { 
                if(value.Count() > MaxPlayers)
                {
                    throw new ArgumentException($"Number of players cannot exceed the maximum of {MaxPlayers} for this game.");
                }
                players = value;
            }
        }

        public List<Kill> Kills
        {
            get { return kills; }
            set { kills = value; }
        }

        public Rules Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        public bool HasStarted
        {
            get { return hasStarted; }
            set { hasStarted = value; }
        }

        public bool IsFinished
        {
            get { return isFinished; }
            set { isFinished = value; }
        }

        public Player? Winner
        {
            get { return winner; }
            set { winner = value; }
        }

        public List<Player> Admins
        {
            get { return admins; }
            set { admins = value; }
        }

        public int MaxPlayers
        {
            get { return maxPlayers; }
            set { maxPlayers = value; }
        }

        public Player Creator
        {
            get { return _creator; }
        }

        /* Handles a valid kill event between a killer and a victim
         * Datetime parameter 'when' is optional; if not provided, the current UTC time will be used
         * We couldn't use DateTime.UtcNow directly in the method signature because default parameters must be compile-time constants
         * We also take an option weapon, weapon is optional because it should only be provided if the 'CustomWeapons' rule is enabled
         * Reason is also optional, the message gets made using killer and victim so we can't set it directly in the method signature */
        public void HandleValidKill(Player killer, Player victim, string? weapon = null)
        {
            if (killer == null || victim == null)
            {
                throw new ArgumentNullException("Killer or victim cannot be null.");
            }

            if (!Rules.CustomWeapons && weapon != null)
            {
                throw new InvalidOperationException("Custom weapons are not allowed in this game.");
            }

            if (Rules.CustomWeapons && weapon == null)
            {
                throw new InvalidOperationException("Custom weapons are on so weapon cannot be null.");
            }

            TargetAssignment? targetAssignment = Players
                                                    .Where(p => p.Id == killer.Id || p.Id == victim.Id)
                                                    .SelectMany(p => p.TargetAssignments)
                                                    .FirstOrDefault(ta => ta.HunterId == killer.Id &&
                                                                          ta.TargetId == victim.Id &&
                                                                          ta.Kill == null);

            if (targetAssignment == null)
            {
                throw new InvalidOperationException("No valid target assignment found for the given killer and victim.");
            }

            targetAssignment.AssignmentStatus = AssignmentStatus.Killed;

            Kill kill = new Kill(this, killer, victim, weapon);
            targetAssignment.Kill = kill;
            Kills.Add(kill);

            victim.IsAlive = false;

            // Check if the victim is the hunter of the killer
            TargetAssignment? killersHunterAssignment = Players
                .SelectMany(p => p.TargetAssignments)
                .FirstOrDefault(ta => ta.HunterId == victim.Id &&
                                     ta.TargetId == killer.Id &&
                                     ta.Kill == null &&
                                     ta.AssignmentStatus == AssignmentStatus.Ongoing);

            if (killersHunterAssignment != null)
            {
                if (Rules.GameModes == GameModes.Gotcha)
                {
                    throw new InvalidOperationException("Killer cannot be the target of the victim in Gotcha gamemode.");
                }
                // Victim guessed the killer correctly (Assassing gamemode)
                killersHunterAssignment.AssignmentStatus = AssignmentStatus.Cancelled;

                if (this.GetLivingPlayers().Count() == 1)
                {
                    IsFinished = true;
                    Winner = killer;
                }
                else
                {
                    TargetAssignment newTargetAssignment = new TargetAssignment(killersHunterAssignment.Hunter, killer, weapon);
                    killersHunterAssignment.Hunter.TargetAssignments.Add(newTargetAssignment);
                }
            }
            else
            {
                // Standard logic for updating the victim's target assignment
                TargetAssignment? victimsTargetAssignment = victim.TargetAssignments
                    .FirstOrDefault(ta => ta.HunterId == victim.Id &&
                                         ta.Kill == null &&
                                         ta.AssignmentStatus == AssignmentStatus.Ongoing);

                if (victimsTargetAssignment == null)
                {
                    if (this.GetLivingPlayers().Count() == 1)
                    {
                        IsFinished = true;
                        Winner = killer;
                    }
                    else
                    {
                        throw new InvalidOperationException("Victim has no ongoing target assignment to reassign.");
                    }
                }
                else
                {
                    victimsTargetAssignment.AssignmentStatus = AssignmentStatus.Cancelled;

                    Player victimsTarget = victimsTargetAssignment.Target;
                    TargetAssignment newTargetAssignment = new TargetAssignment(killer, victimsTarget, weapon);
                    killer.TargetAssignments.Add(newTargetAssignment);
                }
            }
        }


        public void HandleInValidKill(Player killer, Player victim, string? reason = null, string? weapon = null, AssignmentStatus assignmentStatus = AssignmentStatus.Failed)
        {
            if (killer == null || victim == null)
            {
                throw new ArgumentNullException("Killer or victim cannot be null.");
            }

            if (!Rules.CustomWeapons && weapon != null)
            {
                throw new InvalidOperationException("Custom weapons are not allowed in this game.");
            }

            if (Rules.CustomWeapons && weapon == null)
            {
                throw new InvalidOperationException("Custom weapons are on so weapon cannot be null.");
            }

            TargetAssignment? targetAssignment = Players
                .Where(p => p.Id == killer.Id || p.Id == victim.Id)
                .SelectMany(p => p.TargetAssignments)
                .FirstOrDefault(ta => ta.HunterId == killer.Id &&
                                      ta.TargetId == victim.Id &&
                                      ta.Kill == null);

            if (targetAssignment == null)
            {
                throw new InvalidOperationException("No valid target assignment found for the given killer and victim.");
            }

            if(reason == null)
            {
                reason = "No reason provided.";
            }

            targetAssignment.AssignmentStatus = assignmentStatus;
            Kill kill = new Kill(this, killer, victim, weapon, reason, false);
            targetAssignment.Kill = kill;
            Kills.Add(kill);

            TargetAssignment newTargetAssignment = new TargetAssignment(killer, victim, weapon);
            killer.TargetAssignments.Add(newTargetAssignment);
        }

        public void StartGame()
        {
            if (HasStarted)
            {
                throw new InvalidOperationException("Game has already started.");
            }

            if (Players.Count() < 3)
            {
                throw new InvalidOperationException("Not enough players to start the game. Minimum 3 players required.");
            }

            if (Rules.RandomTargetAssignment)
            {
                AssignTargetsRandomly();
            }
            else
            {
                AssignTargetsCircular();
            }

            HasStarted = true;
            startDate = DateTime.UtcNow;
        }

        public List<Player> GetLivingPlayers()
        {
            return Players
                    .Where(p => p.IsAlive)
                    .ToList();
        }

        public List<Player> GetEliminatedPlayers()
        {
            return Players
                    .Where(p => !p.IsAlive)
                    .ToList();
        }

        public override string ToString()
        {
            return Name;
        }

        // Assign targets in a circular manner
        private void AssignTargetsCircular(List<string>? weapons = null)
        {
            if (!Rules.CustomWeapons && weapons != null)
            {
                throw new InvalidOperationException("Custom weapons are not allowed in this game.");
            }

            if (Rules.CustomWeapons && weapons == null)
            {
                throw new InvalidOperationException("Custom weapons are on so weapon cannot be null.");
            }

            List<Player> players = GetLivingPlayers();

            Random rand = new Random();
            List<Player> shuffledPlayers = players.OrderBy(p => rand.Next()).ToList();
            int playerCount = shuffledPlayers.Count;

            // if 'CustomWeapons' rule is enabled, we need to assign weapons as well
            if (weapons != null)
            {
                if (players.Count() > weapons.Count())
                {
                    throw new InvalidOperationException("Not enough weapons provided for the number of players.");
                }

                for (int i = 0; i < playerCount; i++)
                {
                    Player hunter = shuffledPlayers[i];
                    Player target = shuffledPlayers[(i + 1) % playerCount];
                    string weapon = weapons[i % weapons.Count];
                    TargetAssignment targetAssignment = new TargetAssignment(hunter, target, weapon);
                    hunter.TargetAssignments.Add(targetAssignment);
                }
            }
            // if 'CustomWeapons' rule is disabled, we just assign targets normally
            else
            {
                for (int i = 0; i < playerCount; i++)
                {
                    Player hunter = shuffledPlayers[i];
                    Player target = shuffledPlayers[(i + 1) % playerCount];
                    TargetAssignment targetAssignment = new TargetAssignment(hunter, target);
                    hunter.TargetAssignments.Add(targetAssignment);
                }
            }
        }

        // Assigns targets randomly to players, ensuring no player is assigned to themselves
        private void AssignTargetsRandomly(List<string>? weapons = null)
        {
            if (!Rules.CustomWeapons && weapons != null)
            {
                throw new InvalidOperationException("Custom weapons are not allowed in this game.");
            }

            if (Rules.CustomWeapons && weapons == null)
            {
                throw new InvalidOperationException("Custom weapons are on so weapon cannot be null.");
            }

            List<Player> players = GetLivingPlayers();
            Random rand = new Random();
            List<Player> shuffledPlayers = players.OrderBy(p => rand.Next()).ToList();
            int playerCount = shuffledPlayers.Count;

            for (int i = 0; i < playerCount; i++)
            {
                Player hunter = shuffledPlayers[i];
                Player target;
                do
                {
                    target = shuffledPlayers[rand.Next(playerCount)];
                } while (target.Id == hunter.Id); // Ensure hunter and target are not the same

                TargetAssignment targetAssignment = new TargetAssignment(hunter, target);
                hunter.TargetAssignments.Add(targetAssignment);
            }
        }

    }
}
