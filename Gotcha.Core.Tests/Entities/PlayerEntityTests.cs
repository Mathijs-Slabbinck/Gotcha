using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Tests.Entities
{
    public class PlayerEntityTests
    {
        #region DisplayName

        [Fact]
        // Rules: ShowUsernames = true, ShowRealNames = false
        // Player has Username set → should return that Username
        public void DisplayName_ShowUsernamesOnly_ReturnsUsername()
        {
            // Arrange
            Game game = new Game()
            {
                Rules = new Rules() { ShowUsernames = true, ShowRealNames = false }
            };

            GotchaUser user = new GotchaUser()
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@test.com"
            };

            Player player = new Player()
            {
                User = user,
                UserId = user.Id,
                Game = game,
                GameId = game.Id,
                UserName = "PlayerNick"
            };

            // Act
            string displayName = player.DisplayName;

            // Assert
            Assert.Equal("PlayerNick", displayName);
        }

        [Fact]
        // Rules: ShowUsernames = true, ShowRealNames = false
        // Player.Username is null → should fallback to User.Username
        public void DisplayName_ShowUsernamesOnly_NullPlayerUsername_FallsBackToUserUsername()
        {
            // Arrange
            Game game = new Game()
            {
                Rules = new Rules() { ShowUsernames = true, ShowRealNames = false }
            };

            GotchaUser user = new GotchaUser()
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@test.com"
            };

            Player player = new Player()
            {
                User = user,
                UserId = user.Id,
                Game = game,
                GameId = game.Id,
                UserName = null
            };

            // Act
            string displayName = player.DisplayName;

            // Assert
            Assert.Equal("johndoe", displayName);
        }

        [Fact]
        // Rules: ShowUsernames = false, ShowRealNames = true
        // Should return "{User.FirstName} {User.LastName}"
        public void DisplayName_ShowRealNamesOnly_ReturnsFullName()
        {
            // Arrange
            Game game = new Game()
            {
                Rules = new Rules() { ShowUsernames = false, ShowRealNames = true }
            };

            GotchaUser user = new GotchaUser()
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@test.com"
            };

            Player player = new Player()
            {
                User = user,
                UserId = user.Id,
                Game = game,
                GameId = game.Id,
                UserName = "PlayerNick"
            };

            // Act
            string displayName = player.DisplayName;

            // Assert
            Assert.Equal("John Doe", displayName);
        }

        [Fact]
        // Rules: ShowUsernames = true, ShowRealNames = true
        // Should return "({Username})" — wrapped in parentheses
        // (real name is shown separately elsewhere)
        public void DisplayName_ShowBoth_ReturnsUsernameInParentheses()
        {
            // Arrange
            Game game = new Game()
            {
                Rules = new Rules() { ShowUsernames = true, ShowRealNames = true }
            };

            GotchaUser user = new GotchaUser()
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@test.com"
            };

            Player player = new Player()
            {
                User = user,
                UserId = user.Id,
                Game = game,
                GameId = game.Id,
                UserName = "PlayerNick"
            };

            // Act
            string displayName = player.DisplayName;

            // Assert
            Assert.Equal("(PlayerNick)", displayName);
        }

        [Fact]
        // Rules: ShowUsernames = false, ShowRealNames = false
        // Falls into the else branch → should return "{User.FirstName} {User.LastName}"
        public void DisplayName_NeitherEnabled_ReturnsFullName()
        {
            // Arrange
            Game game = new Game()
            {
                Rules = new Rules() { ShowUsernames = false, ShowRealNames = false }
            };

            GotchaUser user = new GotchaUser()
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@test.com"
            };

            Player player = new Player()
            {
                User = user,
                UserId = user.Id,
                Game = game,
                GameId = game.Id,
                UserName = "PlayerNick"
            };

            // Act
            string displayName = player.DisplayName;

            // Assert
            Assert.Equal("John Doe", displayName);
        }

        #endregion

        #region Query Methods

        [Fact]
        // Player has a TargetAssignment where HunterId == player.Id
        // and AssignmentStatus == Ongoing → GetCurrentTarget() returns the Target player
        public void GetCurrentTarget_WithOngoingAssignment_ReturnsTarget()
        {
            // Arrange
            Game game = new Game();
            GotchaUser hunterUser = new GotchaUser() { FirstName = "Alice", LastName = "Smith", UserName = "alice", Email = "alice@test.com" };
            GotchaUser targetUser = new GotchaUser() { FirstName = "Bob", LastName = "Jones", UserName = "bob", Email = "bob@test.com" };

            Player hunter = new Player() { User = hunterUser, UserId = hunterUser.Id, Game = game, GameId = game.Id };
            Player target = new Player() { User = targetUser, UserId = targetUser.Id, Game = game, GameId = game.Id };

            TargetAssignment assignment = new TargetAssignment()
            {
                HunterId = hunter.Id,
                Hunter = hunter,
                TargetId = target.Id,
                Target = target,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            hunter.TargetAssignments.Add(assignment);

            // Act
            Player? currentTarget = hunter.GetCurrentTarget();

            // Assert
            Assert.NotNull(currentTarget);
            Assert.Equal(target.Id, currentTarget.Id);
        }

        [Fact]
        // Player has no ongoing assignments → GetCurrentTarget() returns null
        public void GetCurrentTarget_NoOngoingAssignment_ReturnsNull()
        {
            // Arrange
            Game game = new Game();
            GotchaUser user = new GotchaUser() { FirstName = "Alice", LastName = "Smith", UserName = "alice", Email = "alice@test.com" };
            Player player = new Player() { User = user, UserId = user.Id, Game = game, GameId = game.Id };

            // Act
            Player? currentTarget = player.GetCurrentTarget();

            // Assert
            Assert.Null(currentTarget);
        }

        [Fact]
        // Another player in the game has an ongoing TargetAssignment
        // where TargetId == this player's Id → GetCurrentHunter(game) returns that hunter
        public void GetCurrentHunter_WithOngoingAssignment_ReturnsHunter()
        {
            // Arrange
            Game game = new Game();
            GotchaUser hunterUser = new GotchaUser() { FirstName = "Alice", LastName = "Smith", UserName = "alice", Email = "alice@test.com" };
            GotchaUser targetUser = new GotchaUser() { FirstName = "Bob", LastName = "Jones", UserName = "bob", Email = "bob@test.com" };

            Player hunter = new Player() { User = hunterUser, UserId = hunterUser.Id, Game = game, GameId = game.Id };
            Player target = new Player() { User = targetUser, UserId = targetUser.Id, Game = game, GameId = game.Id };

            game.Players.Add(hunter);
            game.Players.Add(target);

            TargetAssignment assignment = new TargetAssignment()
            {
                HunterId = hunter.Id,
                Hunter = hunter,
                TargetId = target.Id,
                Target = target,
                AssignmentStatus = AssignmentStatus.Ongoing
            };
            hunter.TargetAssignments.Add(assignment);

            // Act
            Player? currentHunter = target.GetCurrentHunter(game);

            // Assert
            Assert.NotNull(currentHunter);
            Assert.Equal(hunter.Id, currentHunter.Id);
        }

        [Fact]
        // Player has TargetAssignments with Kill != null
        // GetKilledPlayers() returns the Target of those assignments
        public void GetKilledPlayers_ReturnsVictims()
        {
            // Arrange
            Game game = new Game();
            GotchaUser killerUser = new GotchaUser() { FirstName = "Alice", LastName = "Smith", UserName = "alice", Email = "alice@test.com" };
            GotchaUser victim1User = new GotchaUser() { FirstName = "Bob", LastName = "Jones", UserName = "bob", Email = "bob@test.com" };
            GotchaUser victim2User = new GotchaUser() { FirstName = "Charlie", LastName = "Brown", UserName = "charlie", Email = "charlie@test.com" };

            Player killer = new Player() { User = killerUser, UserId = killerUser.Id, Game = game, GameId = game.Id };
            Player victim1 = new Player() { User = victim1User, UserId = victim1User.Id, Game = game, GameId = game.Id };
            Player victim2 = new Player() { User = victim2User, UserId = victim2User.Id, Game = game, GameId = game.Id };

            Kill kill1 = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = killer.Id, Killer = killer,
                VictimId = victim1.Id, Victim = victim1
            };

            Kill kill2 = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = killer.Id, Killer = killer,
                VictimId = victim2.Id, Victim = victim2
            };

            // Assignment with a kill (completed)
            TargetAssignment assignment1 = new TargetAssignment()
            {
                HunterId = killer.Id, Hunter = killer,
                TargetId = victim1.Id, Target = victim1,
                Kill = kill1
            };

            // Assignment with a kill (completed)
            TargetAssignment assignment2 = new TargetAssignment()
            {
                HunterId = killer.Id, Hunter = killer,
                TargetId = victim2.Id, Target = victim2,
                Kill = kill2
            };

            killer.TargetAssignments.Add(assignment1);
            killer.TargetAssignments.Add(assignment2);

            // Act
            IEnumerable<Player> killedPlayers = killer.GetKilledPlayers();

            // Assert
            Assert.Equal(2, killedPlayers.Count());
            Assert.Contains(victim1, killedPlayers);
            Assert.Contains(victim2, killedPlayers);
        }

        [Fact]
        // Another player has a TargetAssignment targeting this player with Kill != null
        // GetKiller(game) returns the Hunter from that assignment
        public void GetKiller_ReturnsKiller()
        {
            // Arrange
            Game game = new Game();
            GotchaUser killerUser = new GotchaUser() { FirstName = "Alice", LastName = "Smith", UserName = "alice", Email = "alice@test.com" };
            GotchaUser victimUser = new GotchaUser() { FirstName = "Bob", LastName = "Jones", UserName = "bob", Email = "bob@test.com" };

            Player killer = new Player() { User = killerUser, UserId = killerUser.Id, Game = game, GameId = game.Id };
            Player victim = new Player() { User = victimUser, UserId = victimUser.Id, Game = game, GameId = game.Id };

            game.Players.Add(killer);
            game.Players.Add(victim);

            Kill kill = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = killer.Id, Killer = killer,
                VictimId = victim.Id, Victim = victim
            };

            TargetAssignment assignment = new TargetAssignment()
            {
                HunterId = killer.Id, Hunter = killer,
                TargetId = victim.Id, Target = victim,
                Kill = kill
            };
            killer.TargetAssignments.Add(assignment);

            // Act
            Player? foundKiller = victim.GetKiller(game);

            // Assert
            Assert.NotNull(foundKiller);
            Assert.Equal(killer.Id, foundKiller.Id);
        }

        #endregion

        #region Defaults

        [Fact]
        // New player should have: IsAlive = true, Notes = ""
        // TargetAssignments should be empty collection
        public void DefaultValues_AreCorrect()
        {
            // Arrange
            GotchaUser user = new GotchaUser() { FirstName = "John", LastName = "Doe", UserName = "johndoe", Email = "john@test.com" };
            Game game = new Game();
            Player player = new Player() { User = user, UserId = user.Id, Game = game, GameId = game.Id };

            // Assert
            Assert.True(player.IsAlive);
            Assert.Equal(string.Empty, player.Notes);
            Assert.Empty(player.TargetAssignments);
        }

        #endregion
    }
}
