using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Enums;
using Gotcha.Core.Exceptions;
using Gotcha.Core.Exceptions.NotFound;
using Gotcha.Core.Services.ValidationServices;

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
        private Guid? winnerId;
        private List<Guid> adminIds;
        private int maxPlayers;
        private Guid creatorId;
        #endregion

        #region Constructors

        #region New Game Constructors

        public Game()
        {
            Name = "New game";
            _creationDate = DateTime.UtcNow;
            Players = new List<Player>();
            Kills = new List<Kill>();
            Rules = new Rules();
            HasStarted = false;
            IsFinished = false;
            Admins = new List<Player>();
            creatorId = Guid.Empty;
            MaxPlayers = 1; // set to 1 initially, when the first user joins we set it to their plan
        }

        public Game(Rules rules) : this()
        {
            Rules = rules;
        }

        public Game(string name, Rules rules) : this(rules)
        {
            Name = name;
        }
        #endregion

        /* Constructor for pre-started games loaded from the database
         * Used when trying to load a game from the database that hasn't started yet and doesn't have a start date set  */
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
            creatorId = creator.Id;
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
                if (LastLineValidationService.IsReservedUsername(value))
                {
                    throw new ValidationException("Name", "Game", value);
                }
                else
                {
                    name = value;
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
                    throw new GameStateException("Start date cannot be more than 2 years in the future.");
                }
                else if (value <= DateTime.UtcNow)
                {
                    throw new GameStateException("Start date cannot be in the past.");
                }

                startDate = value;
            }
        }

        public List<Player> Players
        {
            get { return players; }
            private set { 
                if(value.Count() > MaxPlayers)
                {
                    throw new GameStateException($"Number of players cannot exceed the maximum of {MaxPlayers} for this game.");
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

        public Guid? WinnerId
        {
            get
            {
                if (!IsFinished)
                {
                    throw new GameStateException("Cannot get winner of a game that is not finished.");
                }

                Player? winnerPlayer = Players.FirstOrDefault(p => p.Id == winnerId);
                if (winnerPlayer == null)
                {
                    if (winnerId == null)
                    {
                        throw new GameStateException("Winner ID is null but the game has ended!");
                    }

                    throw new PlayerNotFoundException((Guid)winnerId);
                }

                return winnerId;

            }
            set
            {
                if (IsFinished && winnerId != null)
                {
                    throw new GameStateException("Winner has already been set for this finished game.");
                }

                if (!Players.Any(p => p.Id == value))
                {
                    if (value == null)
                    {
                        throw new GameStateException("Cannot set winner ID to null.");
                    }
                    throw new PlayerNotFoundException((Guid)value);
                }

                IsFinished = true;
                winnerId = value;
            }
        }

        public Player Winner
        {
            get 
            {
                if (!IsFinished)
                {
                    throw new GameStateException("Cannot get winner of a game that is not finished.");
                }

                Player? winnerPlayer = Players.FirstOrDefault(p => p.Id == winnerId);
                if (winnerPlayer == null)
                {
                    if(winnerId == null)
                    {
                        throw new GameStateException("Winner ID is null but the game has ended!");
                    }

                    throw new PlayerNotFoundException((Guid)winnerId);
                }

                return winnerPlayer;
            }

            set
            {
                if(IsFinished && winnerId != null)
                {
                    throw new GameStateException("Winner has already been set for this finished game.");
                }

                if(!Players.Any(p => p.Id == value.Id))
                {
                    throw new PlayerNotFoundException(value.Id);
                }

                IsFinished = true;
                winnerId = value.Id;
            }
        }

        public List<Guid> AdminIds
        {
            get
            {
                return adminIds;
            }
            set
            {
                adminIds = value;
            }
        }

        public List<Player> Admins
        {
            get {
                List<Player> admins = Players
                                        .Where(p => AdminIds
                                        .Contains(p.Id))
                                        .ToList();
                return admins;
            }
            set {
                List<Guid> adminIds = Players
                                        .Select(p => p.Id)
                                        .Where(pi => AdminIds.Contains(pi))
                                        .ToList();
                AdminIds = adminIds;
            }
        }

        public int MaxPlayers
        {
            get { return maxPlayers; }
            set { maxPlayers = value; }
        }



        public Player Creator
        {
            get {
                Player? creator = Players.FirstOrDefault(p => p.Id == creatorId);
                if (creator == null)
                {
                    throw new PlayerNotFoundException(creatorId);
                }
                return creator;
            }
        }

        // use this method to let players join this game
        public void JoinPlayer(Player player, bool isAdmin = false)
        {
            if(Players.Count() == 0)
            {
                creatorId = player.Id;
                isAdmin = true;
                
                if(player.User.UserPlan == Plan.Standard)
                {
                    maxPlayers = 50;
                }
                else if(player.User.UserPlan == Plan.Premium)
                {
                    maxPlayers = 100;
                }
                else if(player.User.UserPlan == Plan.Deluxe)
                {
                    maxPlayers = 1000;
                }
            }

            Players.Add(player);

            if (isAdmin)
            {
                adminIds.Add(player.Id);
            }
        }

        /* Handles a valid kill event between a killer and a victim
         * Datetime parameter 'when' is optional; if not provided, the current UTC time will be used
         * We couldn't use DateTime.UtcNow directly in the method signature because default parameters must be compile-time constants
         * We also take an option weapon, weapon is optional because it should only be provided if the 'CustomWeapons' rule is enabled
         * Reason is also optional, the message gets made using killer and victim so we can't set it directly in the method signature */
        public void HandleValidKill(Player killer, Player victim, string? weapon = null)
        {
            if (!HasStarted)
            {
                throw new GameStateException("Cannot register kills before game has started.");
            }

            if (IsFinished)
            {
                throw new GameStateException("Cannot register kills after game has finished.");
            }

            if (killer == null)
            {
                throw new ArgumentNullException("Killer parameter in HandleValidKill() in Game entity cannot be null!");
            }

            if (victim == null)
            {
                throw new ArgumentNullException("Victim parameter in HandleValidKill() in Game entity cannot be null!");
            }

            if (killer.Id == victim.Id)
            {
                throw new InvalidOperationException("A player cannot kill themselves.");
            }

            if (!killer.IsAlive)
            {
                throw new GameStateException($"Killer {killer.DisplayName} is not alive.");
            }

            if (!victim.IsAlive)
            {
                throw new GameStateException($"Victim {victim.DisplayName} is already dead.");
            }

            if (!Players.Any(p => p.Id == killer.Id))
            {
                throw new PlayerNotFoundException(killer.Id);
            }

            if (!Players.Any(p => p.Id == victim.Id))
            {
                throw new PlayerNotFoundException(victim.Id);
            }

            if (!Rules.CustomKillMethods && weapon != null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are not allowed in this game.");
            }

            if (Rules.CustomKillMethods && weapon == null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are on so weapon cannot be null.");
            }

            TargetAssignment? targetAssignment = Players
                                                    .Where(p => p.Id == killer.Id || p.Id == victim.Id)
                                                    .SelectMany(p => p.TargetAssignments)
                                                    .FirstOrDefault(ta => ta.HunterId == killer.Id &&
                                                                          ta.TargetId == victim.Id &&
                                                                          ta.Kill == null);

            if (targetAssignment == null)
            {
                throw new GameStateException("No valid target assignment found for the given killer and victim.");
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
                if (Rules.GameMode == GameModes.Gotcha)
                {
                    throw new GameStateException("Killer cannot be the target of the victim in Gotcha gamemode.");
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
                                                                                                  ta.Kill == null          &&
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
                        throw new GameStateException("Victim has no ongoing target assignment to reassign and isn't the winner.");
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
            if (!HasStarted)
            {
                throw new GotchaInvalidOperationsExceptions("Cannot register kills before game has started.");
            }

            if (IsFinished)
            {
                throw new GotchaInvalidOperationsExceptions("Cannot register kills after game has finished.");
            }

            if (killer == null)
            {
                throw new ArgumentNullException("Killer parameter cannot be null in HandleInValidKill() in Game entity.");
            }

            if (victim == null)
            {
                throw new ArgumentNullException("Victim parameter cannot be null in HandleInValidKill() in Game entity.");
            }

            if (killer.Id == victim.Id)
            {
                throw new InvalidTargetAssignmentException(killer.Id, victim.Id, $"A player cannot kill themselves\nThe same player was passed as both killer and target!.");
            }

            if (!killer.IsAlive)
            {
                throw new InvalidTargetAssignmentException(killer.Id, victim.Id, $"Killer is not alive.");
            }

            if (!victim.IsAlive)
            {
                throw new InvalidTargetAssignmentException(killer.Id, victim.Id, $"Victim is not alive.");
            }

            if (!Players.Any(p => p.Id == killer.Id))
            {
                throw new PlayerNotFoundException(killer.Id);
            }

            if (!Players.Any(p => p.Id == victim.Id))
            {
                throw new PlayerNotFoundException(victim.Id);
            }

            if (!Rules.CustomKillMethods && weapon != null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are not allowed in this game.");
            }

            if (Rules.CustomKillMethods && weapon == null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are on so weapon cannot be null.");
            }

            TargetAssignment? targetAssignment = Players
                                                    .Where(p => p.Id == killer.Id || p.Id == victim.Id)
                                                    .SelectMany(p => p.TargetAssignments)
                                                    .FirstOrDefault(ta => ta.HunterId == killer.Id &&
                                                                          ta.TargetId == victim.Id &&
                                                                          ta.Kill == null);

            if (targetAssignment == null)
            {
                throw new TargetAssignmentNotFoundException(killer.Id, victim.Id);
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
                throw new GameStateException("Game has already started.");
            }

            if (IsFinished)
            {
                throw new GameStateException("Cannot start a finished game.");
            }

            if (Players.Count() < 3)
            {
                throw new InsufficientPlayersException(Players.Count, 3);
            }

            // Validate at least one admin exists
            if (Admins == null || !Admins.Any())
            {
                throw new GameStateException("Game must have at least one admin.");
            }

            try
            {
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
            catch (Exception ex)
            {
                // Log the error and provide context
                throw new GameStateException("Failed to start game due to target assignment error.", ex);
            }
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

        public List<Kill> GetDisputedKills()
        {
            return Kills
                    .Where(k => !k.IsValid)
                    .ToList();
        }

        public override string ToString()
        {
            return Name;
        }

        // Assign targets in a circular manner
        private void AssignTargetsCircular(List<string>? weapons = null)
        {
            if (!Rules.CustomKillMethods && weapons != null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are not allowed in this game.");
            }

            if (Rules.CustomKillMethods && weapons == null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are on so weapon cannot be null.");
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
                    throw new GameStateException("Not enough weapons provided for the number of players.");
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
            if (!Rules.CustomKillMethods && weapons != null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are not allowed in this game.");
            }

            if (Rules.CustomKillMethods && weapons == null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are on so weapon cannot be null.");
            }

            List<Player> players = GetLivingPlayers();
            int playerCount = players.Count;

            // Cannot assign unique targets with less than 2 players
            if (playerCount < 2)
            {
                throw new GameRuleViolationException("CustomKillMethods", "At least two players are required for target assignment.");
            }

            // Shuffle players
            Random rand = new Random();
            List<Player> shuffledPlayers = players.OrderBy(p => rand.Next()).ToList();

            // Circular assignment: each player targets the next, last targets first
            for (int i = 0; i < playerCount; i++)
            {
                Player hunter = shuffledPlayers[i];
                Player target = shuffledPlayers[(i + 1) % playerCount]; // Ensures unique, non-self assignment

                string? weapon;
                if (weapons != null && weapons.Count > i)
                {
                    weapon = weapons[i];
                }
                else
                {
                    weapon = null;
                }

                TargetAssignment targetAssignment = new TargetAssignment(hunter, target, weapon);
                hunter.TargetAssignments.Add(targetAssignment);
            }
        }
    }
}
