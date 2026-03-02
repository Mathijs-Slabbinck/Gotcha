using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Data.Seeder
{
    public class Seeder
    {
        public static async Task SeedAsync(GotchaDbContext context)
        {
            // Don't seed if data already exists
            if (context.GotchaUsers.Any()) return;

            #region Users

            string[] firstNames =   { "John", "Jane", "Alice", "Tom", "Tamara", "Johnny", "Jeremy", "Hakim", "Jiminy", "Jefried" };
            string[] lastNames =    { "Doe", "Dane", "Van Den Bosche", "Aat", "Smith", "Kartonnie", "De Vries", "Sharifi", "Jaxon", "Samson" };
            string[] userNames =    { "TheLegend27", "Parzival", "EpicNPCMan", "ShadowFox", "StormBreaker", "IronVault", "NightOwl", "QuickDraw", "PhantomX", "GhostRunner" };
            string[] emails =       { "john.doe@example.com", "jane.dane@example.com", "alice.vandenbosche@example.com", "tom.aat@example.com", "tamara.smith@example.com", "johnny.kartonnie@example.com",                   "jeremy.devries@example.com", "hakim.sharifi@example.com", "jiminy.jaxon@example.com", "jefried.samson@example.com" };
            DateTime[] birthDates = { new(1990, 1, 1), new(1992, 2, 2), new(1994, 1, 3), new(1996, 2, 5), new(1998, 6, 7), new(1969, 6, 9), new(1978, 7, 1), new(2000, 1, 3), new(1998, 3, 7), new(2001, 2, 9) };

            if (!(firstNames.Length == lastNames.Length &&
                 lastNames.Length == userNames.Length &&
                 userNames.Length == emails.Length &&
                 emails.Length == birthDates.Length))
            {
                throw new Exception("The data seeding data arrays for User are not the same size!");
            }

            List<GotchaUser> users = new List<GotchaUser>();
            for (int i = 0; i < firstNames.Length; i++)
            {
                users.Add(new GotchaUser
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    UserName = userNames[i],
                    Email = emails[i],
                    BirthDate = birthDates[i]
                });
            }

            context.GotchaUsers.AddRange(users);
            await context.SaveChangesAsync();

            #endregion

            #region Rules

            // standard rules
            Rules rules1 = new Rules();

            // assassin gamemode
            Rules rules2 = new Rules { IsAssassin = true, ShowHunter = true };

            // enforce player images
            Rules rules3 = new Rules
            {
                ShowPlayerImages = true,
                EnforcePlayerImages = true,
                ShowGender = true
            };

            // timed game
            Rules rules4 = new Rules
            {
                ShowPlayerImages = true,
                EnforcePlayerImages = true,
                ShowRealNames = true,
                ShowUsernames = true,
                IsTimed = true,
                TargetTimeOut = new TimeSpan(2, 0, 0, 0),
                KillConfirmationTimer = Timeout.InfiniteTimeSpan
            };

            // chaos game
            Rules rules5 = new Rules
            {
                ShowPlayerImages = true,
                EnforcePlayerImages = true,
                ShowRealNames = true,
                ShowUsernames = true,
                IsChaos = true,
                ChaosTimerMin = new TimeSpan(6, 0, 0),
                ChaosTimerMax = new TimeSpan(12, 0, 0),
                CustomRules = new List<string> { "Lorem ipsum est" },
                KillConfirmationTimer = new TimeSpan(1, 0, 0, 0)
            };

            #endregion

            #region Games

            Game game1 = new Game { Name = "Game1", Rules = rules1 };
            Game game2 = new Game { Name = "Game2", Rules = rules2 };
            Game game3 = new Game { Name = "Game3", Rules = rules3 };
            Game game4 = new Game { Name = "Game4", Rules = rules4 };
            Game game5 = new Game { Name = "Game5", Rules = rules5 };

            context.Games.AddRange(game1, game2, game3, game4, game5);
            await context.SaveChangesAsync();

            #endregion

            #region Players

            // Game 1 - standard rules (3 players)
            Player player1 = new Player { UserId = users[0].Id, User = users[0], GameId = game1.Id, Game = game1, UserName = users[0].UserName };
            Player player2 = new Player { UserId = users[1].Id, User = users[1], GameId = game1.Id, Game = game1, UserName = users[1].UserName };
            Player player3 = new Player { UserId = users[2].Id, User = users[2], GameId = game1.Id, Game = game1, UserName = users[2].UserName };

            // Game 2 - assassin mode (4 players)
            Player player4 = new Player { UserId = users[3].Id, User = users[3], GameId = game2.Id, Game = game2, UserName = users[3].UserName };
            Player player5 = new Player { UserId = users[4].Id, User = users[4], GameId = game2.Id, Game = game2, UserName = users[4].UserName };
            Player player6 = new Player { UserId = users[5].Id, User = users[5], GameId = game2.Id, Game = game2, UserName = users[5].UserName };
            Player player7 = new Player { UserId = users[6].Id, User = users[6], GameId = game2.Id, Game = game2, UserName = users[6].UserName };

            // Game 3 - enforce player images (3 players)
            Player player8 = new Player { UserId = users[7].Id, User = users[7], GameId = game3.Id, Game = game3, UserName = users[7].UserName };
            Player player9 = new Player { UserId = users[8].Id, User = users[8], GameId = game3.Id, Game = game3, UserName = users[8].UserName };
            Player player10 = new Player { UserId = users[9].Id, User = users[9], GameId = game3.Id, Game = game3, UserName = users[9].UserName };

            context.Players.AddRange(player1, player2, player3, player4, player5, player6, player7, player8, player9, player10);
            await context.SaveChangesAsync();

            #endregion
        }
    }
}
