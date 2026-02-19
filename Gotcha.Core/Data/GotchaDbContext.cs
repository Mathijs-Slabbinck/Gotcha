using Gotcha.Core.Entities;
using Gotcha.Core.Entities.Logging;
using Microsoft.EntityFrameworkCore;


namespace Gotcha.Core.Data
{
    public class GotchaDbContext : DbContext
    {
        public GotchaDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region DbSets
        //Define Dbsets => Tables
        // Gotcha (game) Entities vv
        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Rules> Rules { get; set; }
        public DbSet<TargetAssignment> TargetAssignments { get; set; }
        public DbSet<Kill> Kills { get; set; }

        // Other Entities vv
        public DbSet<Attacker> Attackers { get; set; } // keeps track of people that tried (possible) hack attempts (but failed cuz I'm great)
        public DbSet<Log> Logs { get; set; }
        #endregion
    }
}
