using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Data.Seeder
{
    public class Seeder
    {
        // Fixed GUIDs so the API and MAUI can reference known test users/players
        public static readonly Guid TestUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        public static readonly Guid TestUser2Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        public static readonly Guid TestUser3Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        public static async Task SeedAsync(GotchaDbContext context)
        {
            // Don't seed if data already exists
            if (context.GotchaUsers.Any()) return;

            #region Users

            string[] firstNames =   { "John", "Jane", "Alice", "Tom", "Tamara", "Johnny", "Jeremy", "Hakim", "Jiminy", "Jefried" };
            string[] lastNames =    { "Doe", "Dane", "Van Den Bosche", "Aat", "Smith", "Kartonnie", "De Vries", "Sharifi", "Jaxon", "Samson" };
            string[] userNames =    { "TheLegend27", "Parzival", "EpicNPCMan", "ShadowFox", "StormBreaker", "IronVault", "NightOwl", "QuickDraw", "PhantomX", "GhostRunner" };
            string[] emails =       { "john.doe@example.com", "jane.dane@example.com", "alice.vandenbosche@example.com", "tom.aat@example.com", "tamara.smith@example.com", "johnny.kartonnie@example.com", "jeremy.devries@example.com", "hakim.sharifi@example.com", "jiminy.jaxon@example.com", "jefried.samson@example.com" };
            DateTime[] birthDates = { new(1990, 1, 1), new(1992, 2, 2), new(1994, 1, 3), new(1996, 2, 5), new(1998, 6, 7), new(1969, 6, 9), new(1978, 7, 1), new(2000, 1, 3), new(1998, 3, 7), new(2001, 2, 9) };

            // Fixed GUIDs for the first 3 users so they can be referenced by DevConstants
            Guid[] fixedIds = { TestUserId, TestUser2Id, TestUser3Id };

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
                GotchaUser user = new GotchaUser
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    UserName = userNames[i],
                    Email = emails[i],
                    BirthDate = birthDates[i]
                };

                // Override the auto-generated Id for the first 3 users
                if (i < fixedIds.Length)
                {
                    // Use reflection to set the init-only Id property
                    typeof(GotchaUser).GetProperty("Id")!.SetValue(user, fixedIds[i]);
                }

                users.Add(user);
            }

            context.GotchaUsers.AddRange(users);
            await context.SaveChangesAsync();

            #endregion

            #region VipSettings

            // VipSettings for test users
            VipSettings vip1 = new VipSettings
            {
                AssassinModeUnlocked = true,
                TimedKillsUnlocked = true,
                UserPlan = Plan.Premium,
                MaxLobbySize = MaxLobbySize.Medium
            };

            VipSettings vip2 = new VipSettings
            {
                AssassinModeUnlocked = true,
                ChaosModeUnlocked = true,
                CustomKillMethodsUnlocked = true,
                TimedKillsUnlocked = true,
                UserPlan = Plan.Deluxe,
                MaxLobbySize = MaxLobbySize.Max
            };

            VipSettings vip3 = new VipSettings();

            // Link VipSettings to users via shadow FK
            users[0].VipSettings = vip1;
            users[1].VipSettings = vip2;
            users[2].VipSettings = vip3;

            await context.SaveChangesAsync();

            #endregion

            #region Rules

            // standard rules — with visibility options
            Rules rules1 = new Rules
            {
                ShowRealNames = true,
                ShowUsernames = true,
                ShowLivingPlayerCount = true,
                ShowLivingPlayerNames = true
            };

            // assassin gamemode
            Rules rules2 = new Rules
            {
                IsAssassin = true,
                ShowHunter = true,
                ShowUsernames = true,
                ShowLivingPlayerCount = true
            };

            // enforce player images
            Rules rules3 = new Rules
            {
                ShowPlayerImages = true,
                EnforcePlayerImages = true,
                ShowGender = true,
                ShowRealNames = true,
                ShowUsernames = true,
                ShowLivingPlayerCount = true,
                ShowLivingPlayerNames = true
            };

            // timed game
            Rules rules4 = new Rules
            {
                ShowPlayerImages = true,
                EnforcePlayerImages = true,
                ShowRealNames = true,
                ShowUsernames = true,
                ShowLivingPlayerCount = true,
                ShowLivingPlayerNames = true,
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
                ShowLivingPlayerCount = true,
                IsChaos = true,
                ChaosTimerMin = new TimeSpan(6, 0, 0),
                ChaosTimerMax = new TimeSpan(12, 0, 0),
                CustomRules = new List<string> { "No kills inside classrooms", "Safe zones: library and cafeteria" },
                KillConfirmationTimer = new TimeSpan(1, 0, 0, 0)
            };

            #endregion

            #region Games

            Game game1 = new Game { Name = "Friday Night Gotcha", Rules = rules1, CreatorId = users[0].Id, MaxPlayers = 50 };
            Game game2 = new Game { Name = "Campus Hunt", Rules = rules2, CreatorId = users[1].Id, MaxPlayers = 100 };
            Game game3 = new Game { Name = "Office Battle Royale", Rules = rules3, CreatorId = users[2].Id, MaxPlayers = 50 };
            Game game4 = new Game { Name = "Summer Showdown", Rules = rules4, CreatorId = users[0].Id, MaxPlayers = 100 };
            Game game5 = new Game { Name = "Chaos Championship", Rules = rules5, CreatorId = users[1].Id, MaxPlayers = 50 };

            context.Games.AddRange(game1, game2, game3, game4, game5);
            await context.SaveChangesAsync();

            #endregion

            #region Players

            // Game 1 - pending game (not started, 3 players) — user[0] is admin
            Player player1 = new Player { UserId = users[0].Id, User = users[0], GameId = game1.Id, Game = game1, UserName = users[0].UserName, IsAdmin = true };
            Player player2 = new Player { UserId = users[1].Id, User = users[1], GameId = game1.Id, Game = game1, UserName = users[1].UserName };
            Player player3 = new Player { UserId = users[2].Id, User = users[2], GameId = game1.Id, Game = game1, UserName = users[2].UserName };

            // Game 2 - active game (started, 4 players) — user[1] is admin
            Player player4 = new Player { UserId = users[3].Id, User = users[3], GameId = game2.Id, Game = game2, UserName = users[3].UserName };
            Player player5 = new Player { UserId = users[4].Id, User = users[4], GameId = game2.Id, Game = game2, UserName = users[4].UserName };
            Player player6 = new Player { UserId = users[0].Id, User = users[0], GameId = game2.Id, Game = game2, UserName = users[0].UserName };
            Player player7 = new Player { UserId = users[1].Id, User = users[1], GameId = game2.Id, Game = game2, UserName = users[1].UserName, IsAdmin = true };

            // Game 3 - pending game (3 players) — user[2] is admin
            Player player8 = new Player { UserId = users[7].Id, User = users[7], GameId = game3.Id, Game = game3, UserName = users[7].UserName };
            Player player9 = new Player { UserId = users[8].Id, User = users[8], GameId = game3.Id, Game = game3, UserName = users[8].UserName };
            Player player10 = new Player { UserId = users[2].Id, User = users[2], GameId = game3.Id, Game = game3, UserName = users[2].UserName, IsAdmin = true };

            // Game 4 - finished game (4 players, user[0] won) — user[0] is admin
            Player player11 = new Player { UserId = users[0].Id, User = users[0], GameId = game4.Id, Game = game4, UserName = users[0].UserName, IsAdmin = true };
            Player player12 = new Player { UserId = users[5].Id, User = users[5], GameId = game4.Id, Game = game4, UserName = users[5].UserName, IsAlive = false };
            Player player13 = new Player { UserId = users[6].Id, User = users[6], GameId = game4.Id, Game = game4, UserName = users[6].UserName, IsAlive = false };
            Player player14 = new Player { UserId = users[9].Id, User = users[9], GameId = game4.Id, Game = game4, UserName = users[9].UserName, IsAlive = false };

            // Game 5 - active chaos game (4 players, 1 dead) — user[1] is admin
            Player player15 = new Player { UserId = users[1].Id, User = users[1], GameId = game5.Id, Game = game5, UserName = users[1].UserName, IsAdmin = true };
            Player player16 = new Player { UserId = users[2].Id, User = users[2], GameId = game5.Id, Game = game5, UserName = users[2].UserName };
            Player player17 = new Player { UserId = users[3].Id, User = users[3], GameId = game5.Id, Game = game5, UserName = users[3].UserName };
            Player player18 = new Player { UserId = users[4].Id, User = users[4], GameId = game5.Id, Game = game5, UserName = users[4].UserName, IsAlive = false };

            context.Players.AddRange(player1, player2, player3, player4, player5, player6, player7, player8, player9, player10, player11, player12, player13, player14, player15, player16, player17, player18);
            await context.SaveChangesAsync();

            #endregion

            #region Start active games + set up finished game

            // Start Game 2 (active assassin game)
            game2.HasStarted = true;
            game2.StartDate = DateTime.UtcNow.AddDays(-10);
            game2.AdminIds = new List<Guid> { player7.Id };

            // Start Game 4 (finished timed game)
            game4.HasStarted = true;
            game4.StartDate = DateTime.UtcNow.AddDays(-30);
            game4.EndDate = DateTime.UtcNow.AddDays(-5);
            game4.IsFinished = true;
            game4.WinnerId = player11.Id;
            game4.AdminIds = new List<Guid> { player11.Id };

            // Start Game 5 (active chaos game)
            game5.HasStarted = true;
            game5.StartDate = DateTime.UtcNow.AddDays(-7);
            game5.AdminIds = new List<Guid> { player15.Id };

            await context.SaveChangesAsync();

            #endregion

            #region Target Assignments

            // Game 2 — circular: player4 → player5 → player6 → player7 → player4
            TargetAssignment ta1 = new TargetAssignment { HunterId = player4.Id, Hunter = player4, TargetId = player5.Id, Target = player5, TargetAssigned = game2.StartDate!.Value, Weapon = "Water Gun", AssignmentStatus = AssignmentStatus.Ongoing };
            TargetAssignment ta2 = new TargetAssignment { HunterId = player5.Id, Hunter = player5, TargetId = player6.Id, Target = player6, TargetAssigned = game2.StartDate!.Value, Weapon = "Nerf Dart", AssignmentStatus = AssignmentStatus.Ongoing };
            TargetAssignment ta3 = new TargetAssignment { HunterId = player6.Id, Hunter = player6, TargetId = player7.Id, Target = player7, TargetAssigned = game2.StartDate!.Value, Weapon = "Sock", AssignmentStatus = AssignmentStatus.Ongoing };
            TargetAssignment ta4 = new TargetAssignment { HunterId = player7.Id, Hunter = player7, TargetId = player4.Id, Target = player4, TargetAssigned = game2.StartDate!.Value, Weapon = "Water Balloon", AssignmentStatus = AssignmentStatus.Ongoing };

            // Game 4 — completed assignments (player11 won)
            TargetAssignment ta5 = new TargetAssignment { HunterId = player11.Id, Hunter = player11, TargetId = player12.Id, Target = player12, TargetAssigned = game4.StartDate!.Value, Weapon = "Water Gun", AssignmentStatus = AssignmentStatus.Killed, AssignmentFinished = game4.StartDate.Value.AddDays(3) };
            TargetAssignment ta6 = new TargetAssignment { HunterId = player11.Id, Hunter = player11, TargetId = player13.Id, Target = player13, TargetAssigned = game4.StartDate!.Value.AddDays(3), Weapon = "Nerf Dart", AssignmentStatus = AssignmentStatus.Killed, AssignmentFinished = game4.StartDate.Value.AddDays(10) };
            TargetAssignment ta7 = new TargetAssignment { HunterId = player11.Id, Hunter = player11, TargetId = player14.Id, Target = player14, TargetAssigned = game4.StartDate!.Value.AddDays(10), Weapon = "Sock", AssignmentStatus = AssignmentStatus.Killed, AssignmentFinished = game4.StartDate.Value.AddDays(20) };

            // Game 5 — chaos: player15 killed player18, others ongoing
            TargetAssignment ta8 = new TargetAssignment { HunterId = player15.Id, Hunter = player15, TargetId = player18.Id, Target = player18, TargetAssigned = game5.StartDate!.Value, Weapon = "Water Gun", AssignmentStatus = AssignmentStatus.Killed, AssignmentFinished = game5.StartDate.Value.AddDays(2) };
            TargetAssignment ta9 = new TargetAssignment { HunterId = player15.Id, Hunter = player15, TargetId = player16.Id, Target = player16, TargetAssigned = game5.StartDate!.Value.AddDays(2), Weapon = "Nerf Dart", AssignmentStatus = AssignmentStatus.Ongoing };
            TargetAssignment ta10 = new TargetAssignment { HunterId = player16.Id, Hunter = player16, TargetId = player17.Id, Target = player17, TargetAssigned = game5.StartDate!.Value, Weapon = "Sock", AssignmentStatus = AssignmentStatus.Ongoing };
            TargetAssignment ta11 = new TargetAssignment { HunterId = player17.Id, Hunter = player17, TargetId = player15.Id, Target = player15, TargetAssigned = game5.StartDate!.Value, Weapon = "Water Balloon", AssignmentStatus = AssignmentStatus.Ongoing };

            context.TargetAssignments.AddRange(ta1, ta2, ta3, ta4, ta5, ta6, ta7, ta8, ta9, ta10, ta11);
            await context.SaveChangesAsync();

            #endregion

            #region Kills

            // Game 4 kills — player11 (TheLegend27) killed 3 opponents
            Kill kill1 = new Kill
            {
                GameId = game4.Id, Game = game4,
                KillerId = player11.Id, Killer = player11,
                VictimId = player12.Id, Victim = player12,
                Moment = game4.StartDate!.Value.AddDays(3),
                Weapon = "Water Gun",
                Reason = "Valid kill",
                IsValid = true,
                KillMessage = "TheLegend27 eliminated IronVault with a Water Gun!",
                TimeSinceAssignedTarget = TimeSpan.FromDays(3)
            };

            Kill kill2 = new Kill
            {
                GameId = game4.Id, Game = game4,
                KillerId = player11.Id, Killer = player11,
                VictimId = player13.Id, Victim = player13,
                Moment = game4.StartDate!.Value.AddDays(10),
                Weapon = "Nerf Dart",
                Reason = "Valid kill",
                IsValid = true,
                KillMessage = "TheLegend27 eliminated NightOwl with a Nerf Dart!",
                TimeSinceAssignedTarget = TimeSpan.FromDays(7)
            };

            Kill kill3 = new Kill
            {
                GameId = game4.Id, Game = game4,
                KillerId = player11.Id, Killer = player11,
                VictimId = player14.Id, Victim = player14,
                Moment = game4.StartDate!.Value.AddDays(20),
                Weapon = "Sock",
                Reason = "Valid kill",
                IsValid = true,
                KillMessage = "TheLegend27 eliminated GhostRunner with a Sock!",
                TimeSinceAssignedTarget = TimeSpan.FromDays(10)
            };

            // Game 5 kill — player15 (Parzival) killed player18 (StormBreaker)
            Kill kill4 = new Kill
            {
                GameId = game5.Id, Game = game5,
                KillerId = player15.Id, Killer = player15,
                VictimId = player18.Id, Victim = player18,
                Moment = game5.StartDate!.Value.AddDays(2),
                Weapon = "Water Gun",
                Reason = "Valid kill",
                IsValid = true,
                KillMessage = "Parzival eliminated StormBreaker with a Water Gun!",
                TimeSinceAssignedTarget = TimeSpan.FromDays(2)
            };

            // Link kills to target assignments
            ta5.Kill = kill1;
            ta6.Kill = kill2;
            ta7.Kill = kill3;
            ta8.Kill = kill4;

            context.Kills.AddRange(kill1, kill2, kill3, kill4);
            await context.SaveChangesAsync();

            #endregion
        }
    }
}
