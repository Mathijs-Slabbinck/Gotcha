using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;
using Gotcha.Core.Exceptions;
using Gotcha.Core.Exceptions.NotFound;
using Gotcha.Core.Services;

namespace Gotcha.Core.Tests.Services
{
    public class GameServiceTests
    {
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            _gameService = new GameService();
        }

        #region JoinPlayer

        [Fact]
        // First player should become the creator (game.CreatorId == player.Id)
        // and should be added to AdminIds automatically
        public void JoinPlayer_FirstPlayer_BecomesCreatorAndAdmin()
        {
            // Arrange
            Game game = new Game();
            Player player = CreatePlayer(game, "Alice", "Smith", "alice");

            // Act
            _gameService.JoinPlayer(game, player);

            // Assert
            Assert.Equal(player.Id, game.CreatorId);
            Assert.Contains(player.Id, game.AdminIds);
            Assert.Contains(player, game.Players);
        }

        [Fact]
        // First player's VipSettings.MaxLobbySize determines game.MaxPlayers
        // Small = 50, Medium = 100, Large = 500, Max = 10000
        public void JoinPlayer_FirstPlayer_SetsMaxPlayersFromVipSettings()
        {
            // Arrange
            Game game = new Game();
            Player smallPlayer = CreatePlayer(game, "Alice", "Smith", "alice");
            smallPlayer.User.VipSettings.MaxLobbySize = MaxLobbySize.Small;

            // Act
            _gameService.JoinPlayer(game, smallPlayer);

            // Assert
            Assert.Equal(50, game.MaxPlayers);
        }

        [Fact]
        // Second player joins normally — not creator, not admin
        // game.Players.Count should be 2, AdminIds should only contain first player
        public void JoinPlayer_SecondPlayer_JoinsNormally()
        {
            // Arrange
            Game game = new Game();
            Player player1 = CreatePlayer(game, "Alice", "Smith", "alice");
            Player player2 = CreatePlayer(game, "Bob", "Jones", "bob");

            // Act
            _gameService.JoinPlayer(game, player1);
            _gameService.JoinPlayer(game, player2);

            // Assert
            Assert.Equal(2, game.Players.Count);
            Assert.Single(game.AdminIds);
            Assert.Equal(player1.Id, game.CreatorId);
            Assert.DoesNotContain(player2.Id, game.AdminIds);
        }

        [Fact]
        // When isAdmin = true, player's Id is added to game.AdminIds
        public void JoinPlayer_WithIsAdminTrue_AddsToAdminIds()
        {
            // Arrange
            Game game = new Game();
            Player player1 = CreatePlayer(game, "Alice", "Smith", "alice");
            Player player2 = CreatePlayer(game, "Bob", "Jones", "bob");

            // Act
            _gameService.JoinPlayer(game, player1);
            _gameService.JoinPlayer(game, player2, isAdmin: true);

            // Assert
            Assert.Equal(2, game.AdminIds.Count);
            Assert.Contains(player2.Id, game.AdminIds);
        }

        [Fact]
        // When game.Players.Count >= game.MaxPlayers, should throw GameStateException
        public void JoinPlayer_GameIsFull_ThrowsGameStateException()
        {
            // Arrange
            Game game = new Game();
            Player player1 = CreatePlayer(game, "Alice", "Smith", "alice");
            player1.User.VipSettings.MaxLobbySize = MaxLobbySize.Small; // MaxPlayers = 50

            _gameService.JoinPlayer(game, player1);
            game.MaxPlayers = 1; // Override to 1 so next join fails

            Player player2 = CreatePlayer(game, "Bob", "Jones", "bob");

            // Act & Assert
            Assert.Throws<GameStateException>(() => _gameService.JoinPlayer(game, player2));
        }

        #endregion

        #region StartGame

        [Fact]
        // With 3+ players and at least 1 admin:
        // game.HasStarted should be true, game.StartDate should not be null
        // Each player should have a TargetAssignment (circular chain)
        public void StartGame_WithValidState_StartsSuccessfully()
        {
            // Arrange
            Game game = new Game();
            Player player1 = CreatePlayer(game, "Alice", "Smith", "alice");
            Player player2 = CreatePlayer(game, "Bob", "Jones", "bob");
            Player player3 = CreatePlayer(game, "Charlie", "Brown", "charlie");

            _gameService.JoinPlayer(game, player1);
            _gameService.JoinPlayer(game, player2);
            _gameService.JoinPlayer(game, player3);

            // Act
            _gameService.StartGame(game);

            // Assert
            Assert.True(game.HasStarted);
            Assert.NotNull(game.StartDate);
        }

        [Fact]
        // After starting, each player should have exactly 1 ongoing TargetAssignment
        // The assignments should form a circular chain (A->B->C->A)
        public void StartGame_AssignsTargetsCircularly()
        {
            // Arrange
            Game game = new Game();
            Player player1 = CreatePlayer(game, "Alice", "Smith", "alice");
            Player player2 = CreatePlayer(game, "Bob", "Jones", "bob");
            Player player3 = CreatePlayer(game, "Charlie", "Brown", "charlie");

            _gameService.JoinPlayer(game, player1);
            _gameService.JoinPlayer(game, player2);
            _gameService.JoinPlayer(game, player3);

            // Act
            _gameService.StartGame(game);

            // Assert — each player should have exactly 1 assignment
            foreach (Player player in game.Players)
            {
                Assert.Single(player.TargetAssignments);
            }

            // Verify circular chain: follow the chain from player1, should visit all players
            HashSet<Guid> visited = new HashSet<Guid>();
            Player? firstPlayer = game.Players.FirstOrDefault();
            Assert.NotNull(firstPlayer);
            Player current = firstPlayer;

            for (int i = 0; i < game.Players.Count; i++)
            {
                visited.Add(current.Id);
                TargetAssignment? assignment = current.TargetAssignments.FirstOrDefault();
                Assert.NotNull(assignment);
                current = assignment.Target;
            }

            // After full loop, should have visited all players and returned to start
            Assert.Equal(game.Players.Count, visited.Count);
            Assert.Equal(firstPlayer.Id, current.Id);
        }

        [Fact]
        // game.HasStarted = true before calling StartGame → throw GameStateException
        public void StartGame_AlreadyStarted_ThrowsGameStateException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true };

            // Act & Assert
            Assert.Throws<GameStateException>(() => _gameService.StartGame(game));
        }

        [Fact]
        // game.IsFinished = true before calling StartGame → throw GameStateException
        public void StartGame_AlreadyFinished_ThrowsGameStateException()
        {
            // Arrange
            Game game = new Game() { IsFinished = true };

            // Act & Assert
            Assert.Throws<GameStateException>(() => _gameService.StartGame(game));
        }

        [Fact]
        // Less than 3 players → throw InsufficientPlayersException
        // Check that exception.CurrentPlayers and exception.RequiredPlayers are correct
        public void StartGame_FewerThan3Players_ThrowsInsufficientPlayersException()
        {
            // Arrange
            Game game = new Game() { MaxPlayers = 50 };
            Player player1 = CreatePlayer(game, "Alice", "Smith", "alice");
            Player player2 = CreatePlayer(game, "Bob", "Jones", "bob");

            game.Players.Add(player1);
            game.Players.Add(player2);
            game.AdminIds.Add(player1.Id);

            // Act & Assert
            InsufficientPlayersException ex = Assert.Throws<InsufficientPlayersException>(
                () => _gameService.StartGame(game)
            );
            Assert.Equal(2, ex.CurrentPlayers);
            Assert.Equal(3, ex.RequiredPlayers);
        }

        [Fact]
        // No admins in game.AdminIds → throw GameStateException
        public void StartGame_NoAdmins_ThrowsGameStateException()
        {
            // Arrange — manually add players without going through JoinPlayer
            // so that no admin is set
            Game game = new Game() { MaxPlayers = 50 };
            Player player1 = CreatePlayer(game, "Alice", "Smith", "alice");
            Player player2 = CreatePlayer(game, "Bob", "Jones", "bob");
            Player player3 = CreatePlayer(game, "Charlie", "Brown", "charlie");

            game.Players.Add(player1);
            game.Players.Add(player2);
            game.Players.Add(player3);
            // AdminIds is empty — no admins

            // Act & Assert
            Assert.Throws<GameStateException>(() => _gameService.StartGame(game));
        }

        #endregion

        #region HandleValidKill

        [Fact]
        // After a valid kill: victim.IsAlive should be false
        public void HandleValidKill_ValidKill_MarksVictimAsDead()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();

            // Act
            _gameService.HandleValidKill(game, killer, victim);

            // Assert
            Assert.False(victim.IsAlive);
        }

        [Fact]
        // After a valid kill: game.Kills should contain a Kill with IsValid = true
        // Kill.KillerId == killer.Id, Kill.VictimId == victim.Id
        public void HandleValidKill_ValidKill_CreatesKillRecordWithIsValidTrue()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();

            // Act
            _gameService.HandleValidKill(game, killer, victim);

            // Assert
            Assert.Single(game.Kills);
            Kill? kill = game.Kills.FirstOrDefault();
            Assert.NotNull(kill);
            Assert.True(kill.IsValid);
            Assert.Equal(killer.Id, kill.KillerId);
            Assert.Equal(victim.Id, kill.VictimId);
        }

        [Fact]
        // The killer's original target assignment should have AssignmentStatus.Killed
        // and its Kill property should be set
        public void HandleValidKill_ValidKill_UpdatesTargetAssignmentStatus()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();
            TargetAssignment originalAssignment = killer.TargetAssignments.First();

            // Act
            _gameService.HandleValidKill(game, killer, victim);

            // Assert
            Assert.Equal(AssignmentStatus.Killed, originalAssignment.AssignmentStatus);
            Assert.NotNull(originalAssignment.Kill);
        }

        [Fact]
        // game.HasStarted = false → throw GameStateException
        public void HandleValidKill_GameNotStarted_ThrowsGameStateException()
        {
            // Arrange
            Game game = new Game() { HasStarted = false };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");

            // Act & Assert
            Assert.Throws<GameStateException>(() => _gameService.HandleValidKill(game, killer, victim));
        }

        [Fact]
        // game.IsFinished = true → throw GameStateException
        public void HandleValidKill_GameIsFinished_ThrowsGameStateException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true, IsFinished = true };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");

            // Act & Assert
            Assert.Throws<GameStateException>(() => _gameService.HandleValidKill(game, killer, victim));
        }

        [Fact]
        // killer is null → throw ArgumentNullException
        public void HandleValidKill_KillerIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true };
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _gameService.HandleValidKill(game, null!, victim));
        }

        [Fact]
        // victim is null → throw ArgumentNullException
        public void HandleValidKill_VictimIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _gameService.HandleValidKill(game, killer, null!));
        }

        [Fact]
        // killer.Id == victim.Id → throw InvalidOperationException
        public void HandleValidKill_KillerEqualsVictim_ThrowsInvalidOperationException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true };
            Player player = CreatePlayer(game, "Alice", "Smith", "alice");
            game.Players.Add(player);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _gameService.HandleValidKill(game, player, player));
        }

        [Fact]
        // killer.IsAlive = false → throw GameStateException
        public void HandleValidKill_KillerIsDead_ThrowsGameStateException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");
            killer.IsAlive = false;

            game.Players.Add(killer);
            game.Players.Add(victim);

            // Act & Assert
            Assert.Throws<GameStateException>(() => _gameService.HandleValidKill(game, killer, victim));
        }

        [Fact]
        // victim.IsAlive = false → throw GameStateException
        public void HandleValidKill_VictimIsDead_ThrowsGameStateException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");
            victim.IsAlive = false;

            game.Players.Add(killer);
            game.Players.Add(victim);

            // Act & Assert
            Assert.Throws<GameStateException>(() => _gameService.HandleValidKill(game, killer, victim));
        }

        [Fact]
        // killer not in game.Players → throw PlayerNotFoundException
        public void HandleValidKill_KillerNotInGame_ThrowsPlayerNotFoundException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");
            // Only add victim to game, not killer
            game.Players.Add(victim);

            // Act & Assert
            Assert.Throws<PlayerNotFoundException>(() => _gameService.HandleValidKill(game, killer, victim));
        }

        [Fact]
        // victim not in game.Players → throw PlayerNotFoundException
        public void HandleValidKill_VictimNotInGame_ThrowsPlayerNotFoundException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");
            // Only add killer to game, not victim
            game.Players.Add(killer);

            // Act & Assert
            Assert.Throws<PlayerNotFoundException>(() => _gameService.HandleValidKill(game, killer, victim));
        }

        [Fact]
        // game.Rules.CustomKillMethods = true but weapon is null
        // → throw GameRuleViolationException
        public void HandleValidKill_CustomWeaponsRequiredButNoWeapon_ThrowsGameRuleViolationException()
        {
            // Arrange
            Game game = new Game()
            {
                HasStarted = true,
                Rules = new Rules() { CustomKillMethods = true }
            };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");
            game.Players.Add(killer);
            game.Players.Add(victim);

            // Act & Assert — weapon is null but CustomKillMethods is true
            Assert.Throws<GameRuleViolationException>(
                () => _gameService.HandleValidKill(game, killer, victim, weapon: null)
            );
        }

        [Fact]
        // game.Rules.CustomKillMethods = false but weapon is provided
        // → throw GameRuleViolationException
        public void HandleValidKill_CustomWeaponsDisabledButWeaponProvided_ThrowsGameRuleViolationException()
        {
            // Arrange
            Game game = new Game()
            {
                HasStarted = true,
                Rules = new Rules() { CustomKillMethods = false }
            };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");
            game.Players.Add(killer);
            game.Players.Add(victim);

            // Act & Assert — weapon provided but CustomKillMethods is false
            Assert.Throws<GameRuleViolationException>(
                () => _gameService.HandleValidKill(game, killer, victim, weapon: "Knife")
            );
        }

        [Fact]
        // Standard mode (not assassin): after killing victim,
        // killer should get a new TargetAssignment pointing to victim's old target
        // victim's old target assignment should be cancelled
        public void HandleValidKill_StandardMode_ReassignsVictimsTargetToKiller()
        {
            // Arrange — A→B→C→A circular chain
            var (game, playerA, playerB, playerC) = CreateStartedGameWithAssignments();

            TargetAssignment bToC = playerB.TargetAssignments.First();

            // Act — A kills B
            _gameService.HandleValidKill(game, playerA, playerB);

            // Assert — B→C should be cancelled
            Assert.Equal(AssignmentStatus.Cancelled, bToC.AssignmentStatus);

            // Assert — A should have a new ongoing assignment targeting C
            TargetAssignment? newAssignment = playerA.TargetAssignments
                .FirstOrDefault(ta => ta.TargetId == playerC.Id && ta.AssignmentStatus == AssignmentStatus.Ongoing);
            Assert.NotNull(newAssignment);
        }

        [Fact]
        // When only 1 player remains alive after the kill:
        // game.IsFinished should be true
        public void HandleValidKill_LastPlayerStanding_GameEnds()
        {
            // Arrange — 3 players, C is already dead, B has no ongoing assignment
            Game game = new Game()
            {
                HasStarted = true,
                MaxPlayers = 50,
                Rules = new Rules()
            };

            Player playerA = CreatePlayer(game, "Alice", "Smith", "alice");
            Player playerB = CreatePlayer(game, "Bob", "Jones", "bob");
            Player playerC = CreatePlayer(game, "Charlie", "Brown", "charlie");
            playerC.IsAlive = false;

            game.Players.Add(playerA);
            game.Players.Add(playerB);
            game.Players.Add(playerC);
            game.AdminIds.Add(playerA.Id);

            // A→B assignment (ongoing)
            TargetAssignment aToB = new TargetAssignment()
            {
                HunterId = playerA.Id, Hunter = playerA,
                TargetId = playerB.Id, Target = playerB,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerA.TargetAssignments.Add(aToB);

            // B→C assignment (already completed — killed)
            Kill previousKill = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = playerB.Id, Killer = playerB,
                VictimId = playerC.Id, Victim = playerC,
                IsValid = true
            };
            TargetAssignment bToC = new TargetAssignment()
            {
                HunterId = playerB.Id, Hunter = playerB,
                TargetId = playerC.Id, Target = playerC,
                AssignmentStatus = AssignmentStatus.Killed,
                Kill = previousKill
            };
            playerB.TargetAssignments.Add(bToC);

            // Act — A kills B, only A remains alive
            _gameService.HandleValidKill(game, playerA, playerB);

            // Assert
            Assert.True(game.IsFinished);
        }

        [Fact]
        // When game ends: game.Winner should be the killer (game.WinnerId == killer.Id)
        public void HandleValidKill_LastPlayerStanding_WinnerIsSet()
        {
            // Arrange — same setup as above
            Game game = new Game()
            {
                HasStarted = true,
                MaxPlayers = 50,
                Rules = new Rules()
            };

            Player playerA = CreatePlayer(game, "Alice", "Smith", "alice");
            Player playerB = CreatePlayer(game, "Bob", "Jones", "bob");
            Player playerC = CreatePlayer(game, "Charlie", "Brown", "charlie");
            playerC.IsAlive = false;

            game.Players.Add(playerA);
            game.Players.Add(playerB);
            game.Players.Add(playerC);
            game.AdminIds.Add(playerA.Id);

            TargetAssignment aToB = new TargetAssignment()
            {
                HunterId = playerA.Id, Hunter = playerA,
                TargetId = playerB.Id, Target = playerB,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerA.TargetAssignments.Add(aToB);

            Kill previousKill = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = playerB.Id, Killer = playerB,
                VictimId = playerC.Id, Victim = playerC,
                IsValid = true
            };
            TargetAssignment bToC = new TargetAssignment()
            {
                HunterId = playerB.Id, Hunter = playerB,
                TargetId = playerC.Id, Target = playerC,
                AssignmentStatus = AssignmentStatus.Killed,
                Kill = previousKill
            };
            playerB.TargetAssignments.Add(bToC);

            // Act
            _gameService.HandleValidKill(game, playerA, playerB);

            // Assert
            Assert.Equal(playerA.Id, game.WinnerId);
        }

        [Fact]
        // Assassin mode: victim is also the hunter of the killer (victim has an ongoing
        // assignment targeting killer). The victim's hunter assignment should be cancelled.
        // If more than 1 player remains, a new assignment is created from victim to killer.
        public void HandleValidKill_AssassinMode_KillsHunterWhoIsAlsoTarget()
        {
            // Arrange — A→B mutual: A hunts B, B hunts A. C and D exist so >1 alive after kill.
            Game game = new Game()
            {
                HasStarted = true,
                MaxPlayers = 50,
                Rules = new Rules() { IsAssassin = true }
            };

            Player playerA = CreatePlayer(game, "Alice", "Smith", "alice");
            Player playerB = CreatePlayer(game, "Bob", "Jones", "bob");
            Player playerC = CreatePlayer(game, "Charlie", "Brown", "charlie");
            Player playerD = CreatePlayer(game, "Diana", "Prince", "diana");

            game.Players.Add(playerA);
            game.Players.Add(playerB);
            game.Players.Add(playerC);
            game.Players.Add(playerD);
            game.AdminIds.Add(playerA.Id);

            // A→B (ongoing)
            TargetAssignment aToB = new TargetAssignment()
            {
                HunterId = playerA.Id, Hunter = playerA,
                TargetId = playerB.Id, Target = playerB,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerA.TargetAssignments.Add(aToB);

            // B→A (ongoing) — B is hunting A (this triggers the assassin path)
            TargetAssignment bToA = new TargetAssignment()
            {
                HunterId = playerB.Id, Hunter = playerB,
                TargetId = playerA.Id, Target = playerA,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerB.TargetAssignments.Add(bToA);

            // Act — A kills B (A's target). B was also hunting A.
            _gameService.HandleValidKill(game, playerA, playerB);

            // Assert — B→A should be cancelled
            Assert.Equal(AssignmentStatus.Cancelled, bToA.AssignmentStatus);

            // Assert — game should NOT be finished (3 players still alive: A, C, D)
            Assert.False(game.IsFinished);
        }

        [Fact]
        // Assassin mode with only 2 players left: killing the hunter/target
        // should end the game with killer as winner
        public void HandleValidKill_AssassinMode_LastTwoPlayers_GameEnds()
        {
            // Arrange — A and B hunt each other, C is already dead
            Game game = new Game()
            {
                HasStarted = true,
                MaxPlayers = 50,
                Rules = new Rules() { IsAssassin = true }
            };

            Player playerA = CreatePlayer(game, "Alice", "Smith", "alice");
            Player playerB = CreatePlayer(game, "Bob", "Jones", "bob");
            Player playerC = CreatePlayer(game, "Charlie", "Brown", "charlie");
            playerC.IsAlive = false;

            game.Players.Add(playerA);
            game.Players.Add(playerB);
            game.Players.Add(playerC);
            game.AdminIds.Add(playerA.Id);

            // A→B (ongoing)
            TargetAssignment aToB = new TargetAssignment()
            {
                HunterId = playerA.Id, Hunter = playerA,
                TargetId = playerB.Id, Target = playerB,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerA.TargetAssignments.Add(aToB);

            // B→A (ongoing) — B hunts A
            TargetAssignment bToA = new TargetAssignment()
            {
                HunterId = playerB.Id, Hunter = playerB,
                TargetId = playerA.Id, Target = playerA,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerB.TargetAssignments.Add(bToA);

            // Act — A kills B, only A remains
            _gameService.HandleValidKill(game, playerA, playerB);

            // Assert
            Assert.True(game.IsFinished);
            Assert.Equal(playerA.Id, game.WinnerId);
        }

        [Fact]
        // Standard mode (not assassin): if victim is also the hunter of the killer
        // → throw GameStateException ("Killer cannot be the target of the victim in Gotcha gamemode")
        public void HandleValidKill_StandardMode_VictimIsHunterOfKiller_ThrowsGameStateException()
        {
            // Arrange — A→B mutual hunting, but NOT assassin mode
            Game game = new Game()
            {
                HasStarted = true,
                MaxPlayers = 50,
                Rules = new Rules() { IsAssassin = false }
            };

            Player playerA = CreatePlayer(game, "Alice", "Smith", "alice");
            Player playerB = CreatePlayer(game, "Bob", "Jones", "bob");
            Player playerC = CreatePlayer(game, "Charlie", "Brown", "charlie");

            game.Players.Add(playerA);
            game.Players.Add(playerB);
            game.Players.Add(playerC);
            game.AdminIds.Add(playerA.Id);

            // A→B (ongoing)
            TargetAssignment aToB = new TargetAssignment()
            {
                HunterId = playerA.Id, Hunter = playerA,
                TargetId = playerB.Id, Target = playerB,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerA.TargetAssignments.Add(aToB);

            // B→A (ongoing) — B hunts A, triggers the check
            TargetAssignment bToA = new TargetAssignment()
            {
                HunterId = playerB.Id, Hunter = playerB,
                TargetId = playerA.Id, Target = playerA,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerB.TargetAssignments.Add(bToA);

            // Act & Assert — standard mode should throw when victim is hunter of killer
            Assert.Throws<GameStateException>(() => _gameService.HandleValidKill(game, playerA, playerB));
        }

        #endregion

        #region HandleInValidKill

        [Fact]
        // After an invalid kill: game.Kills should contain a Kill with IsValid = false
        public void HandleInValidKill_CreatesKillWithIsValidFalse()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();

            // Act
            _gameService.HandleInValidKill(game, killer, victim);

            // Assert
            Assert.Single(game.Kills);
            Kill? invalidKill = game.Kills.FirstOrDefault();
            Assert.NotNull(invalidKill);
            Assert.False(invalidKill.IsValid);
        }

        [Fact]
        // After an invalid kill: killer should get a new TargetAssignment
        // pointing to the same victim (retry)
        public void HandleInValidKill_CreatesNewAssignmentForRetry()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();

            // Act
            _gameService.HandleInValidKill(game, killer, victim);

            // Assert — killer should have 2 assignments: original (Failed) + new retry (Ongoing)
            Assert.Equal(2, killer.TargetAssignments.Count);

            TargetAssignment? retryAssignment = killer.TargetAssignments
                .FirstOrDefault(ta => ta.AssignmentStatus == AssignmentStatus.Ongoing);
            Assert.NotNull(retryAssignment);
            Assert.Equal(victim.Id, retryAssignment.TargetId);
        }

        [Fact]
        // When reason is null, the kill.Reason should be "No reason provided."
        public void HandleInValidKill_NullReason_UsesDefaultReason()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();

            // Act
            _gameService.HandleInValidKill(game, killer, victim, reason: null);

            // Assert
            Kill? defaultReasonKill = game.Kills.FirstOrDefault();
            Assert.NotNull(defaultReasonKill);
            Assert.Equal("No reason provided.", defaultReasonKill.Reason);
        }

        [Fact]
        // When a custom reason is provided, the kill.Reason should be that custom reason
        public void HandleInValidKill_CustomReason_UsesProvidedReason()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();

            // Act
            _gameService.HandleInValidKill(game, killer, victim, reason: "Victim disputed the kill");

            // Assert
            Kill? customReasonKill = game.Kills.FirstOrDefault();
            Assert.NotNull(customReasonKill);
            Assert.Equal("Victim disputed the kill", customReasonKill.Reason);
        }

        [Fact]
        // assignmentStatus = AssignmentStatus.Ongoing → throw GotchaInvalidOperationException
        public void HandleInValidKill_StatusOngoing_ThrowsGotchaInvalidOperationException()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();

            // Act & Assert
            Assert.Throws<GotchaInvalidOperationException>(
                () => _gameService.HandleInValidKill(game, killer, victim, assignmentStatus: AssignmentStatus.Ongoing)
            );
        }

        [Fact]
        // assignmentStatus = AssignmentStatus.Killed → throw GotchaInvalidOperationException
        public void HandleInValidKill_StatusKilled_ThrowsGotchaInvalidOperationException()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();

            // Act & Assert
            Assert.Throws<GotchaInvalidOperationException>(
                () => _gameService.HandleInValidKill(game, killer, victim, assignmentStatus: AssignmentStatus.Killed)
            );
        }

        [Fact]
        // game.HasStarted = false → throw GotchaInvalidOperationException
        public void HandleInValidKill_GameNotStarted_ThrowsGotchaInvalidOperationException()
        {
            // Arrange
            Game game = new Game() { HasStarted = false };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");

            // Act & Assert
            Assert.Throws<GotchaInvalidOperationException>(
                () => _gameService.HandleInValidKill(game, killer, victim)
            );
        }

        [Fact]
        // game.IsFinished = true → throw GotchaInvalidOperationException
        public void HandleInValidKill_GameIsFinished_ThrowsGotchaInvalidOperationException()
        {
            // Arrange
            Game game = new Game() { HasStarted = true, IsFinished = true };
            Player killer = CreatePlayer(game, "Alice", "Smith", "alice");
            Player victim = CreatePlayer(game, "Bob", "Jones", "bob");

            // Act & Assert
            Assert.Throws<GotchaInvalidOperationException>(
                () => _gameService.HandleInValidKill(game, killer, victim)
            );
        }

        [Fact]
        // The original target assignment's status should be updated to the given assignmentStatus
        // (default is AssignmentStatus.Failed)
        public void HandleInValidKill_UpdatesOriginalAssignmentStatus()
        {
            // Arrange
            var (game, killer, victim, _) = CreateStartedGameWithAssignments();
            TargetAssignment originalAssignment = killer.TargetAssignments.First();

            // Act — use Revoked status instead of default Failed
            _gameService.HandleInValidKill(game, killer, victim, assignmentStatus: AssignmentStatus.Revoked);

            // Assert
            Assert.Equal(AssignmentStatus.Revoked, originalAssignment.AssignmentStatus);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Creates a Player with a linked User. The Player references the given Game
        /// so that DisplayName and other navigation properties work correctly.
        /// </summary>
        private Player CreatePlayer(Game game, string firstName, string lastName, string username)
        {
            GotchaUser user = new GotchaUser()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                Email = $"{username}@test.com",
                VipSettings = new VipSettings()
            };

            Player player = new Player()
            {
                UserId = user.Id,
                User = user,
                GameId = game.Id,
                Game = game,
                UserName = username
            };

            return player;
        }

        /// <summary>
        /// Creates a started game with 3 players (A, B, C) and circular assignments A→B→C→A.
        /// Returns (game, playerA, playerB, playerC) where playerA is the killer with assignment to playerB.
        /// All players are alive and in the game. CustomKillMethods is false (no weapon needed).
        /// </summary>
        private (Game game, Player playerA, Player playerB, Player playerC) CreateStartedGameWithAssignments(
            bool isAssassin = false)
        {
            Game game = new Game()
            {
                HasStarted = true,
                MaxPlayers = 50,
                Rules = new Rules() { IsAssassin = isAssassin }
            };

            Player playerA = CreatePlayer(game, "Alice", "Smith", "alice");
            Player playerB = CreatePlayer(game, "Bob", "Jones", "bob");
            Player playerC = CreatePlayer(game, "Charlie", "Brown", "charlie");

            game.Players.Add(playerA);
            game.Players.Add(playerB);
            game.Players.Add(playerC);
            game.AdminIds.Add(playerA.Id);
            game.CreatorId = playerA.Id;

            // Circular chain: A→B→C→A
            TargetAssignment aToB = new TargetAssignment()
            {
                HunterId = playerA.Id, Hunter = playerA,
                TargetId = playerB.Id, Target = playerB,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerA.TargetAssignments.Add(aToB);

            TargetAssignment bToC = new TargetAssignment()
            {
                HunterId = playerB.Id, Hunter = playerB,
                TargetId = playerC.Id, Target = playerC,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerB.TargetAssignments.Add(bToC);

            TargetAssignment cToA = new TargetAssignment()
            {
                HunterId = playerC.Id, Hunter = playerC,
                TargetId = playerA.Id, Target = playerA,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            playerC.TargetAssignments.Add(cToA);

            return (game, playerA, playerB, playerC);
        }

        #endregion
    }
}
