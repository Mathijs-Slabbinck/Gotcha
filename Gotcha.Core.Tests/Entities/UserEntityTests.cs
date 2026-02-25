using Gotcha.Core.Entities.Models;

namespace Gotcha.Core.Tests.Entities
{
    public class UserEntityTests
    {
        [Fact]
        // User with 3 PlayerAccounts in 3 different games
        // GetAllGamesPlayed() should return all 3 games
        public void GetAllGamesPlayed_ReturnsAllGames()
        {
            // Arrange
            User user = new User() { FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@test.com" };

            Game game1 = new Game() { Name = "Game 1" };
            Game game2 = new Game() { Name = "Game 2" };
            Game game3 = new Game() { Name = "Game 3" };

            Player player1 = new Player() { User = user, UserId = user.Id, Game = game1, GameId = game1.Id };
            Player player2 = new Player() { User = user, UserId = user.Id, Game = game2, GameId = game2.Id };
            Player player3 = new Player() { User = user, UserId = user.Id, Game = game3, GameId = game3.Id };

            user.PlayerAccounts.Add(player1);
            user.PlayerAccounts.Add(player2);
            user.PlayerAccounts.Add(player3);

            // Act
            List<Game> games = user.GetAllGamesPlayed();

            // Assert
            Assert.Equal(3, games.Count);
            Assert.Contains(game1, games);
            Assert.Contains(game2, games);
            Assert.Contains(game3, games);
        }

        [Fact]
        // User with 2 games: one with IsFinished = false, one with IsFinished = true
        // GetAllActiveGames() should return only the unfinished game
        public void GetAllActiveGames_ReturnsOnlyUnfinishedGames()
        {
            // Arrange
            User user = new User() { FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@test.com" };

            Game activeGame = new Game() { Name = "Active", IsFinished = false };
            Game finishedGame = new Game() { Name = "Finished", IsFinished = true };

            Player player1 = new Player() { User = user, UserId = user.Id, Game = activeGame, GameId = activeGame.Id };
            Player player2 = new Player() { User = user, UserId = user.Id, Game = finishedGame, GameId = finishedGame.Id };

            user.PlayerAccounts.Add(player1);
            user.PlayerAccounts.Add(player2);

            // Act
            List<Game> activeGames = user.GetAllActiveGames();

            // Assert
            Assert.Single(activeGames);
            Assert.Equal("Active", activeGames[0].Name);
        }

        [Fact]
        // User with 2 games: one finished, one not
        // GetAllFinishedGames() should return only the finished game
        public void GetAllFinishedGames_ReturnsOnlyFinishedGames()
        {
            // Arrange
            User user = new User() { FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@test.com" };

            Game activeGame = new Game() { Name = "Active", IsFinished = false };
            Game finishedGame = new Game() { Name = "Finished", IsFinished = true };

            Player player1 = new Player() { User = user, UserId = user.Id, Game = activeGame, GameId = activeGame.Id };
            Player player2 = new Player() { User = user, UserId = user.Id, Game = finishedGame, GameId = finishedGame.Id };

            user.PlayerAccounts.Add(player1);
            user.PlayerAccounts.Add(player2);

            // Act
            List<Game> finishedGames = user.GetAllFinishedGames();

            // Assert
            Assert.Single(finishedGames);
            Assert.Equal("Finished", finishedGames[0].Name);
        }

        [Fact]
        // User won 1 out of 2 games (game.Winner.Id == player.Id)
        // GetAllWonGames() should return only that game
        public void GetAllWonGames_ReturnsOnlyWonGames()
        {
            // Arrange
            User user = new User() { FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@test.com" };

            Game wonGame = new Game() { Name = "Won" };
            Game lostGame = new Game() { Name = "Lost" };

            Player winnerPlayer = new Player() { User = user, UserId = user.Id, Game = wonGame, GameId = wonGame.Id };
            Player loserPlayer = new Player() { User = user, UserId = user.Id, Game = lostGame, GameId = lostGame.Id };

            wonGame.Players.Add(winnerPlayer);
            wonGame.WinnerId = winnerPlayer.Id;

            lostGame.Players.Add(loserPlayer);
            // lostGame has no winner set

            user.PlayerAccounts.Add(winnerPlayer);
            user.PlayerAccounts.Add(loserPlayer);

            // Act
            List<Game> wonGames = user.GetAllWonGames();

            // Assert
            Assert.Single(wonGames);
            Assert.Equal("Won", wonGames[0].Name);
        }

        [Fact]
        // User's player has valid kills (IsValid = true, KillerId == player.Id)
        // GetAllKills() should return only those valid kills
        // Invalid kills should NOT be included
        public void GetAllKills_ReturnsOnlyValidKillsByUser()
        {
            // Arrange
            User user = new User() { FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@test.com" };
            User victimUser = new User() { FirstName = "Jane", LastName = "Smith", Username = "janes", Email = "jane@test.com" };

            Game game = new Game();

            Player killer = new Player() { User = user, UserId = user.Id, Game = game, GameId = game.Id };
            Player victim = new Player() { User = victimUser, UserId = victimUser.Id, Game = game, GameId = game.Id };

            game.Players.Add(killer);
            game.Players.Add(victim);

            Kill validKill = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = killer.Id, Killer = killer,
                VictimId = victim.Id, Victim = victim,
                IsValid = true
            };

            Kill invalidKill = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = killer.Id, Killer = killer,
                VictimId = victim.Id, Victim = victim,
                IsValid = false
            };

            game.Kills.Add(validKill);
            game.Kills.Add(invalidKill);

            user.PlayerAccounts.Add(killer);

            // Act
            List<Kill> kills = user.GetAllKills();

            // Assert
            Assert.Single(kills);
            Assert.True(kills[0].IsValid);
        }

        [Fact]
        // User's player is the victim in some kills (VictimId == player.Id)
        // GetAllDeaths() should return those kills (both valid and invalid)
        public void GetAllDeaths_ReturnsKillsWhereUserIsVictim()
        {
            // Arrange
            User user = new User() { FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@test.com" };
            User killerUser = new User() { FirstName = "Jane", LastName = "Smith", Username = "janes", Email = "jane@test.com" };

            Game game = new Game();

            Player victim = new Player() { User = user, UserId = user.Id, Game = game, GameId = game.Id };
            Player killer = new Player() { User = killerUser, UserId = killerUser.Id, Game = game, GameId = game.Id };

            game.Players.Add(victim);
            game.Players.Add(killer);

            Kill validKill = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = killer.Id, Killer = killer,
                VictimId = victim.Id, Victim = victim,
                IsValid = true
            };

            Kill invalidKill = new Kill()
            {
                GameId = game.Id, Game = game,
                KillerId = killer.Id, Killer = killer,
                VictimId = victim.Id, Victim = victim,
                IsValid = false
            };

            game.Kills.Add(validKill);
            game.Kills.Add(invalidKill);

            user.PlayerAccounts.Add(victim);

            // Act
            List<Kill> deaths = user.GetAllDeaths();

            // Assert — both valid and invalid kills where user is victim
            Assert.Equal(2, deaths.Count);
        }

        [Fact]
        // User with empty PlayerAccounts list
        // All query methods should return empty lists (not throw)
        public void EmptyPlayerAccounts_ReturnsEmptyLists()
        {
            // Arrange
            User user = new User() { FirstName = "John", LastName = "Doe", Username = "johndoe", Email = "john@test.com" };

            // Act & Assert — none should throw
            Assert.Empty(user.GetAllGamesPlayed());
            Assert.Empty(user.GetAllActiveGames());
            Assert.Empty(user.GetAllFinishedGames());
            Assert.Empty(user.GetAllWonGames());
            Assert.Empty(user.GetAllKills());
            Assert.Empty(user.GetAllDeaths());
        }

        [Fact]
        // user.ToString() should return "(FirstName LastName - (Username))"
        // Example: "(John Doe - (johnd))"
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            User user = new User()
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johnd"
            };

            // Act
            string result = user.ToString();

            // Assert
            Assert.Equal("(John Doe - (johnd))", result);
        }
    }
}
