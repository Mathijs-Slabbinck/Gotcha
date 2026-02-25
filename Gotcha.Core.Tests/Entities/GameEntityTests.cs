using Gotcha.Core.Entities.Models;

namespace Gotcha.Core.Tests.Entities
{
    public class GameEntityTests
    {
        [Fact]
        // Set game.WinnerId to a player's Id, add that player to Players
        // game.Winner should return that player
        public void Winner_WithMatchingPlayer_ReturnsCorrectPlayer()
        {
            // Arrange
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe"
            };

            Game game = new Game();

            Player player = new Player()
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = user.Id
            };

            // Act
            game.WinnerId = player.Id;
            game.Winner = player;
            game.Players.Add(player);

            //Assert
            Assert.True(game.Winner == player);
        }

        [Fact]
        // game.WinnerId is null → game.Winner should return null
        public void Winner_NoWinnerId_ReturnsNull()
        {
            // Arrange
            Game game = new Game();

            // Assert
            Assert.Null(game.WinnerId);
            Assert.Null(game.Winner);
        }

        [Fact]
        // Setting game.Winner = somePlayer should set game.WinnerId = somePlayer.Id
        // Setting game.Winner = null should set game.WinnerId = null
        public void Winner_Setter_SetsWinnerId()
        {
            // Arrange
            Game game = new Game();
            User user = new User()
            {
                FirstName = "John",
                LastName = "Doe"
            };
            Player player = new Player()
            {
                User = user,
                UserId = user.Id
            };
            game.Players.Add(player);

            // Act — set winner to a player
            game.Winner = player;

            // Assert
            Assert.Equal(player.Id, game.WinnerId);

            // Act — set winner back to null
            game.Winner = null;

            // Assert
            Assert.Null(game.WinnerId);
        }

        [Fact]
        // Add player Ids to AdminIds, add matching players to Players
        // game.Admins should return only those matching players
        public void Admins_ReturnsPlayersMatchingAdminIds()
        {
            // Arrange
            Game game = new Game();

            User user1 = new User() { FirstName = "Alice", LastName = "Smith" };
            User user2 = new User() { FirstName = "Bob", LastName = "Jones" };
            User user3 = new User() { FirstName = "Charlie", LastName = "Brown" };

            Player admin1 = new Player() { User = user1, UserId = user1.Id };
            Player admin2 = new Player() { User = user2, UserId = user2.Id };
            Player normalPlayer = new Player() { User = user3, UserId = user3.Id };

            game.Players.Add(admin1);
            game.Players.Add(admin2);
            game.Players.Add(normalPlayer);

            game.AdminIds.Add(admin1.Id);
            game.AdminIds.Add(admin2.Id);

            // Act
            List<Player> admins = game.Admins;

            // Assert
            Assert.Equal(2, admins.Count);
            Assert.Contains(admin1, admins);
            Assert.Contains(admin2, admins);
            Assert.DoesNotContain(normalPlayer, admins);
        }

        [Fact]
        // Set game.CreatorId to a player's Id, add that player to Players
        // game.Creator should return that player
        public void Creator_WithMatchingPlayer_ReturnsCorrectPlayer()
        {
            // Arrange
            Game game = new Game();
            User user = new User() { FirstName = "John", LastName = "Doe" };
            Player player = new Player() { User = user, UserId = user.Id };

            game.Players.Add(player);
            game.CreatorId = player.Id;

            // Act
            Player? creator = game.Creator;

            // Assert
            Assert.NotNull(creator);
            Assert.Equal(player.Id, creator.Id);
        }

        [Fact]
        // Add mix of alive and dead players to game.Players
        // GetLivingPlayers() should return only players where IsAlive == true
        public void GetLivingPlayers_ReturnsOnlyAlivePlayers()
        {
            // Arrange
            Game game = new Game();

            User user1 = new User() { FirstName = "Alice", LastName = "Smith" };
            User user2 = new User() { FirstName = "Bob", LastName = "Jones" };
            User user3 = new User() { FirstName = "Charlie", LastName = "Brown" };

            Player alive1 = new Player() { User = user1, UserId = user1.Id, IsAlive = true };
            Player alive2 = new Player() { User = user2, UserId = user2.Id, IsAlive = true };
            Player dead = new Player() { User = user3, UserId = user3.Id, IsAlive = false };

            game.Players.Add(alive1);
            game.Players.Add(alive2);
            game.Players.Add(dead);

            // Act
            List<Player> living = game.GetLivingPlayers();

            // Assert
            Assert.Equal(2, living.Count);
            Assert.Contains(alive1, living);
            Assert.Contains(alive2, living);
            Assert.DoesNotContain(dead, living);
        }

        [Fact]
        // Add mix of alive and dead players
        // GetEliminatedPlayers() should return only players where IsAlive == false
        public void GetEliminatedPlayers_ReturnsOnlyDeadPlayers()
        {
            // Arrange
            Game game = new Game();

            User user1 = new User() { FirstName = "Alice", LastName = "Smith" };
            User user2 = new User() { FirstName = "Bob", LastName = "Jones" };
            User user3 = new User() { FirstName = "Charlie", LastName = "Brown" };

            Player alive = new Player() { User = user1, UserId = user1.Id, IsAlive = true };
            Player dead1 = new Player() { User = user2, UserId = user2.Id, IsAlive = false };
            Player dead2 = new Player() { User = user3, UserId = user3.Id, IsAlive = false };

            game.Players.Add(alive);
            game.Players.Add(dead1);
            game.Players.Add(dead2);

            // Act
            List<Player> eliminated = game.GetEliminatedPlayers();

            // Assert
            Assert.Equal(2, eliminated.Count);
            Assert.Contains(dead1, eliminated);
            Assert.Contains(dead2, eliminated);
            Assert.DoesNotContain(alive, eliminated);
        }

        [Fact]
        // Add kills with IsValid = true and IsValid = false to game.Kills
        // GetDisputedKills() should return only kills where IsValid == false
        public void GetDisputedKills_ReturnsOnlyInvalidKills()
        {
            // Arrange
            Game game = new Game();

            User killerUser = new User() { FirstName = "Alice", LastName = "Smith" };
            User victimUser = new User() { FirstName = "Bob", LastName = "Jones" };
            Player killer = new Player() { User = killerUser, UserId = killerUser.Id };
            Player victim = new Player() { User = victimUser, UserId = victimUser.Id };

            Kill validKill = new Kill()
            {
                GameId = game.Id,
                Game = game,
                KillerId = killer.Id,
                Killer = killer,
                VictimId = victim.Id,
                Victim = victim,
                IsValid = true
            };

            Kill invalidKill = new Kill()
            {
                GameId = game.Id,
                Game = game,
                KillerId = killer.Id,
                Killer = killer,
                VictimId = victim.Id,
                Victim = victim,
                IsValid = false
            };

            game.Kills.Add(validKill);
            game.Kills.Add(invalidKill);

            // Act
            List<Kill> disputed = game.GetDisputedKills();

            // Assert
            Assert.Single(disputed);
            Assert.False(disputed[0].IsValid);
        }

        [Fact]
        // New game should have these defaults:
        // Name = "New game", HasStarted = false, IsFinished = false
        // WinnerId = null, MaxPlayers = 1, CreatorId = Guid.Empty
        // Players and Kills should be empty lists, Rules should not be null
        public void DefaultValues_AreCorrect()
        {
            // Arrange
            Game game = new Game();

            // Assert
            Assert.Equal("New game", game.Name);
            Assert.False(game.HasStarted);
            Assert.False(game.IsFinished);
            Assert.Null(game.WinnerId);
            Assert.Equal(1, game.MaxPlayers);
            Assert.Equal(Guid.Empty, game.CreatorId);
            Assert.Empty(game.Players);
            Assert.Empty(game.Kills);
            Assert.NotNull(game.Rules);
        }

        [Fact]
        // game.ToString() should return game.Name
        public void ToString_ReturnsName()
        {
            // Arrange
            Game game = new Game();
            game.Name = "My Test Game";

            // Act
            string result = game.ToString();

            // Assert
            Assert.Equal("My Test Game", result);
        }
    }
}
