using System;
using System.Collections.Generic;
using System.Text;
using Gotcha.Core.Entities;
using Gotcha.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Core.Data.Seeder
{
    public class Seeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            #region Users

            List<User> usersList = new List<User>();

            string[] firstNames = { "John",
                                    "Jane",
                                    "Alice",
                                    "Tom",
                                    "Tamara",
                                    "Johnny",
                                    "Jeremy",
                                    "Hakim",
                                    "Jiminy",
                                    "Jefried",
            };

            string[] lastNames = { "Doe",
                                   "Dane",
                                   "Van Den Bosche",
                                   "Aat",
                                   "Smith",
                                   "Kartonnie",
                                   "De Naaktgeborene",
                                   "Sharifi",
                                   "Jaxon",
                                   "Samson"
            };

            string[] userNames = { "TheLegend27",
                                   "Parzival",
                                   "EpicNPCMan",
                                   "PoopHead27",
                                   "NoobMaster69",
                                   "Pafkantoor",
                                   "Ben Dover",
                                   "Username",
                                   "NoName",
                                   "Ligma Balls"
            };

            string[] emails = { "hohn.doe@example.com",
                                "hane.dane@example.com",
                                "alice.vandenbosche@example.com",
                                "tom.aat@example.com",
                                "tamara.smith@example.com",
                                "johnny.kartonnie@example.com",
                                "jemery.denaaktgeborene@example.com",
                                "hakim.sharifi@example.com",
                                "jimini.jaxon@example.com",
                                "jefried.samson@example.com"

            };

            DateTime[] birthDates = { new DateTime(1990, 1, 1),
                                      new DateTime(1992, 2, 2),
                                      new DateTime(1994, 1, 3),
                                      new DateTime(1996, 2, 5),
                                      new DateTime(1998, 6, 7),
                                      new DateTime(1969, 6, 9),
                                      new DateTime(1978, 7, 1),
                                      new DateTime(2000, 1, 3),
                                      new DateTime(1998, 3, 7),
                                      new DateTime(2001, 2, 9)
            };

            if (!(firstNames.Length == lastNames.Length &&
                 lastNames.Length == userNames.Length &&
                 userNames.Length == emails.Length &&
                 emails.Length == birthDates.Length))
            {
                throw new Exception("The data seeding data arrays for User are not the same size!");
            }

            for (int i = 0; i < firstNames.Length; i++)
            {
                User user = new User(firstNames[i], lastNames[i], userNames[i], emails[i], birthDates[i]);
                usersList.Add(user);
            }

            User[] users = usersList.ToArray();

            #endregion

            #region Rules
            // standard rules
            Rules rules1 = new Rules();
            // assassing gameMode
            Rules rules2 = new Rules(GameModes.Assassin);
            // enforce player images
            Rules rules3 = new Rules(GameModes.Gotcha, true, true);
            // timed game (TimeSpan(2, 0, 0, 0) =  2d, 0u, 0m, 0S)
            Rules rules4 = new Rules(Guid.NewGuid(), GameModes.Gotcha, true, true, true, true, true, new TimeSpan(2, 0, 0, 0), "", false, new List<string>(), false, false, Timeout.InfiniteTimeSpan);
            // chaos game (TimeSpan(1, 0, 0, 0) =  1d, 0u, 0m, 0S)
            Rules rules5 = new Rules(Guid.NewGuid(), GameModes.Gotcha, true, true, true, true, false, Timeout.InfiniteTimeSpan, "Lorem ipsum est", false, new List<string>(), false, true, new TimeSpan(1, 0, 0, 0));

            List<Rules> rulesList = new List<Rules> { rules1, rules2, rules3, rules4, rules5 };

            Rules[] rules = rulesList.ToArray();
            #endregion

            #region Games
            List<Game> gamesList = new List<Game>();

            Game game1 = new Game("Game1", rules1);
            Game game2 = new Game("Game2", rules2);
            Game game3 = new Game("Game3", rules3);
            Game game4 = new Game("Game4", rules4);
            Game game5 = new Game("Game5", rules5);
            #endregion

            #region Players
            List<Player> playersList = new List<Player>();

            Player player = new Player(users[0], game1);
            #endregion
        }
    }
}
