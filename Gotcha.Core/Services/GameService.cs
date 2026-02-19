using Gotcha.Core.Entities;
using Gotcha.Core.Enums;
using Gotcha.Core.Exceptions;
using Gotcha.Core.Exceptions.NotFound;

namespace Gotcha.Core.Services
{
    public class GameService
    {
        public GameService() { }

        public void JoinPlayer(Game game, Player player, bool isAdmin = false)
        {
            if (game.Players.Count() == 0)
            {
                game.CreatorId = player.Id;
                isAdmin = true;

                game.MaxPlayers = GetMaxPlayersForLobbySize(player.User.VipSettings.MaxLobbySize);
            }

            if (game.Players.Count() >= game.MaxPlayers)
            {
                throw new GameStateException($"This game is full. Maximum players: {game.MaxPlayers}.");
            }

            game.Players.Add(player);

            if (isAdmin)
            {
                game.AdminIds.Add(player.Id);
            }
        }

        public void StartGame(Game game)
        {
            if (game.HasStarted)
            {
                throw new GameStateException("Game has already started.");
            }

            if (game.IsFinished)
            {
                throw new GameStateException("Cannot start a finished game.");
            }

            if (game.Players.Count() < 3)
            {
                throw new InsufficientPlayersException(game.Players.Count, 3);
            }

            if (game.Admins == null || !game.Admins.Any())
            {
                throw new GameStateException("Game must have at least one admin.");
            }

            try
            {
                AssignTargetsCircular(game);

                game.HasStarted = true;
                game.StartDate = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                throw new GameStateException("Failed to start game due to target assignment error.", ex);
            }
        }

        public void HandleValidKill(Game game, Player killer, Player victim, string? weapon = null)
        {
            if (!game.HasStarted)
            {
                throw new GameStateException("Cannot register kills before game has started.");
            }

            if (game.IsFinished)
            {
                throw new GameStateException("Cannot register kills after game has finished.");
            }

            if (killer == null)
            {
                throw new ArgumentNullException("Killer parameter in HandleValidKill() in GameService cannot be null!");
            }

            if (victim == null)
            {
                throw new ArgumentNullException("Victim parameter in HandleValidKill() in GameService cannot be null!");
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

            if (!game.Players.Any(p => p.Id == killer.Id))
            {
                throw new PlayerNotFoundException(killer.Id);
            }

            if (!game.Players.Any(p => p.Id == victim.Id))
            {
                throw new PlayerNotFoundException(victim.Id);
            }

            if (!game.Rules.CustomKillMethods && weapon != null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are not allowed in this game.");
            }

            if (game.Rules.CustomKillMethods && weapon == null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are on so weapon cannot be null.");
            }

            TargetAssignment? targetAssignment = game.Players
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

            Kill kill = CreateKill(game, killer, victim, weapon, isValid: true);
            targetAssignment.Kill = kill;
            game.Kills.Add(kill);

            victim.IsAlive = false;

            // Check if the victim is the hunter of the killer
            TargetAssignment? killersHunterAssignment = game.Players
                                                            .SelectMany(p => p.TargetAssignments)
                                                            .FirstOrDefault(ta => ta.HunterId == victim.Id &&
                                                                                  ta.TargetId == killer.Id &&
                                                                                  ta.Kill == null &&
                                                                                  ta.AssignmentStatus == AssignmentStatus.Ongoing);

            if (killersHunterAssignment != null)
            {
                if (!game.Rules.IsAssassin)
                {
                    throw new GameStateException("Killer cannot be the target of the victim in Gotcha gamemode.");
                }
                // Victim guessed the killer correctly (Assassin gamemode)
                killersHunterAssignment.AssignmentStatus = AssignmentStatus.Cancelled;

                if (game.GetLivingPlayers().Count() == 1)
                {
                    game.IsFinished = true;
                    game.Winner = killer;
                }
                else
                {
                    TargetAssignment newAssignment = CreateTargetAssignment(killersHunterAssignment.Hunter, killer, weapon);
                    killersHunterAssignment.Hunter.TargetAssignments.Add(newAssignment);
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
                    if (game.GetLivingPlayers().Count() == 1)
                    {
                        game.IsFinished = true;
                        game.Winner = killer;
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
                    TargetAssignment newAssignment = CreateTargetAssignment(killer, victimsTarget, weapon);
                    killer.TargetAssignments.Add(newAssignment);
                }
            }
        }

        public void HandleInValidKill(Game game, Player killer, Player victim, string? reason = null, string? weapon = null, AssignmentStatus assignmentStatus = AssignmentStatus.Failed)
        {
            if (!game.HasStarted)
            {
                throw new GotchaInvalidOperationException("Cannot register kills before game has started.");
            }

            if (game.IsFinished)
            {
                throw new GotchaInvalidOperationException("Cannot register kills after game has finished.");
            }

            if (killer == null)
            {
                throw new ArgumentNullException("Killer parameter cannot be null in HandleInValidKill() in GameService.");
            }

            if (victim == null)
            {
                throw new ArgumentNullException("Victim parameter cannot be null in HandleInValidKill() in GameService.");
            }

            if (killer.Id == victim.Id)
            {
                throw new InvalidTargetAssignmentException(killer.Id, victim.Id, "A player cannot kill themselves\nThe same player was passed as both killer and target!.");
            }

            if (!killer.IsAlive)
            {
                throw new InvalidTargetAssignmentException(killer.Id, victim.Id, "Killer is not alive.");
            }

            if (!victim.IsAlive)
            {
                throw new InvalidTargetAssignmentException(killer.Id, victim.Id, "Victim is not alive.");
            }

            if (!game.Players.Any(p => p.Id == killer.Id))
            {
                throw new PlayerNotFoundException(killer.Id);
            }

            if (!game.Players.Any(p => p.Id == victim.Id))
            {
                throw new PlayerNotFoundException(victim.Id);
            }

            if (!game.Rules.CustomKillMethods && weapon != null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are not allowed in this game.");
            }

            if (game.Rules.CustomKillMethods && weapon == null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are on so weapon cannot be null.");
            }

            if (assignmentStatus == AssignmentStatus.Ongoing)
            {
                throw new GotchaInvalidOperationException("AssignmentStatus parameter can't be Ongoing in HandleInValidKill().");
            }

            if (assignmentStatus == AssignmentStatus.Killed)
            {
                throw new GotchaInvalidOperationException("AssignmentStatus parameter can't be Killed in HandleInValidKill().");
            }

            TargetAssignment? targetAssignment = game.Players
                                                    .Where(p => p.Id == killer.Id || p.Id == victim.Id)
                                                    .SelectMany(p => p.TargetAssignments)
                                                    .FirstOrDefault(ta => ta.HunterId == killer.Id &&
                                                                          ta.TargetId == victim.Id &&
                                                                          ta.Kill == null);

            if (targetAssignment == null)
            {
                throw new TargetAssignmentNotFoundException(killer.Id, victim.Id);
            }

            targetAssignment.AssignmentStatus = assignmentStatus;

            if (reason == null)
                reason = "No reason provided.";

            Kill kill = CreateKill(game, killer, victim, weapon, isValid: false, reason);
            targetAssignment.Kill = kill;
            game.Kills.Add(kill);

            TargetAssignment newAssignment = CreateTargetAssignment(killer, victim, weapon);
            killer.TargetAssignments.Add(newAssignment);
        }

        #region Private Helpers

        private void AssignTargetsCircular(Game game, List<string>? weapons = null)
        {
            if (!game.Rules.CustomKillMethods && weapons != null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are not allowed in this game.");
            }

            if (game.Rules.CustomKillMethods && weapons == null)
            {
                throw new GameRuleViolationException("CustomKillMethods", "Custom weapons are on so weapon cannot be null.");
            }

            List<Player> players = game.GetLivingPlayers();

            Random rand = new Random();
            List<Player> shuffledPlayers = players.OrderBy(p => rand.Next()).ToList();
            int playerCount = shuffledPlayers.Count;

            if (weapons != null && players.Count() > weapons.Count())
            {
                throw new GameStateException("Not enough weapons provided for the number of players.");
            }

            for (int i = 0; i < playerCount; i++)
            {
                Player hunter = shuffledPlayers[i];
                Player target = shuffledPlayers[(i + 1) % playerCount];
                string? weapon;

                if (weapons != null)
                    weapon = weapons[i % weapons.Count];
                else
                    weapon = null;

                TargetAssignment assignment = CreateTargetAssignment(hunter, target, weapon);
                hunter.TargetAssignments.Add(assignment);
            }
        }

        private TargetAssignment CreateTargetAssignment(Player hunter, Player target, string? weapon = null)
        {
            return new TargetAssignment
            {
                HunterId = hunter.Id,
                Hunter = hunter,
                TargetId = target.Id,
                Target = target,
                Weapon = weapon
            };
        }

        private Kill CreateKill(Game game, Player killer, Player victim, string? weapon, bool isValid, string? reason = null)
        {
            DateTime killMoment = DateTime.UtcNow;
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(killMoment, TimeZoneInfo.Local);
            string timeFormatted = localTime.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'");

            string killMessage;

            if (isValid)
            {
                if (weapon != null)
                    killMessage = $"{killer} has killed {victim} on {timeFormatted} using: {weapon}";
                else
                    killMessage = $"{killer} has killed {victim} on {timeFormatted}";
            }
            else
            {
                if (weapon != null)
                    killMessage = $"{killer} has attempted to kill {victim} on {timeFormatted} using: {weapon}";
                else
                    killMessage = $"{killer} has attempted to kill {victim} on {timeFormatted}";
            }


            TargetAssignment? assignment = killer.TargetAssignments
                                                            .FirstOrDefault(ta => ta.TargetId == victim.Id &&
                                                                                  ta.Kill == null);

            if (assignment == null)
            {
                throw new TargetAssignmentNotFoundException(killer.Id, victim.Id);
            }

            TimeSpan timeSinceAssigned = killMoment - assignment.TargetAssigned;

            string defaultReason;
            if (isValid)
                defaultReason = "Killed in game!";
            else
                defaultReason = "No reason provided.";

            return new Kill
            {
                GameId = game.Id,
                Game = game,
                KillerId = killer.Id,
                Killer = killer,
                VictimId = victim.Id,
                Victim = victim,
                Moment = killMoment,
                Weapon = weapon,
                IsValid = isValid,
                Reason = reason ?? defaultReason,
                KillMessage = killMessage,
                TimeSinceAssignedTarget = timeSinceAssigned
            };
        }
        private int GetMaxPlayersForLobbySize(MaxLobbySize lobbySize)
        {
            switch (lobbySize)
            {
                case MaxLobbySize.Small:
                    return 50;

                case MaxLobbySize.Medium:
                    return 100;

                case MaxLobbySize.Large:
                    return 500;
                case MaxLobbySize.Max:
                    return 10000;
                default:
                    return 50; // fallback / default
            }
        }


        #endregion
    }
}
