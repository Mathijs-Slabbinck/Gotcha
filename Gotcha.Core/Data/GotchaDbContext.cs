using Gotcha.Core.Entities.Logging.Models;
using Gotcha.Core.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Gotcha.Core.Data
{
    public class GotchaDbContext : IdentityDbContext<GotchaUser, IdentityRole<Guid>, Guid>
    {
        public GotchaDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================== USER ====================

            modelBuilder.Entity<GotchaUser>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.UserName).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(200).IsRequired();
                entity.Property(u => u.ProfileImageSource).HasMaxLength(500);
                entity.Property(u => u.GuardianEmail).HasMaxLength(200);

                // User has one VipSettings (one-to-one, shadow FK on VipSettings)
                entity.HasOne(u => u.VipSettings)
                      .WithOne()
                      .HasForeignKey<VipSettings>("UserId")
                      .OnDelete(DeleteBehavior.Cascade);

                // User has many PlayerAccounts (configured from Player side)
            });

            // ==================== VIPSETTINGS ====================

            modelBuilder.Entity<VipSettings>(entity =>
            {
                entity.HasKey(v => v.Id);
            });

            // ==================== GAME ====================

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Name).HasMaxLength(100).IsRequired();

                // Ignore computed properties — these are read-only lookups, not DB columns
                entity.Ignore(g => g.Winner);
                entity.Ignore(g => g.Admins);
                entity.Ignore(g => g.Creator);

                // Game has one Rules (one-to-one, shadow FK on Game)
                entity.HasOne(g => g.Rules)
                      .WithOne()
                      .HasForeignKey<Game>("RulesId")
                      .OnDelete(DeleteBehavior.Cascade);

                // Game has many Players (configured from Player side)
                // Game has many Kills (configured from Kill side)
            });

            // ==================== RULES ====================

            modelBuilder.Entity<Rules>(entity =>
            {
                entity.HasKey(r => r.Id);

                // TimeSpan can exceed 24 hours or be negative (InfiniteTimeSpan)
                // SQL Server time(7) only supports 0-24 hours
                // Store as ticks (long/bigint) instead
                entity.Property(r => r.TargetTimeOut)
                      .HasConversion(
                          timeSpan => timeSpan.Ticks,
                          ticks => TimeSpan.FromTicks(ticks));

                entity.Property(r => r.ChaosTimerMin)
                      .HasConversion(
                          timeSpan => timeSpan.Ticks,
                          ticks => TimeSpan.FromTicks(ticks));

                entity.Property(r => r.ChaosTimerMax)
                      .HasConversion(
                          timeSpan => timeSpan.Ticks,
                          ticks => TimeSpan.FromTicks(ticks));

                entity.Property(r => r.KillConfirmationTimer)
                      .HasConversion(
                          timeSpan => timeSpan.Ticks,
                          ticks => TimeSpan.FromTicks(ticks));
            });

            // ==================== PLAYER ====================

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.UserName).HasMaxLength(50);
                entity.Property(p => p.ProfileImageSource).HasMaxLength(500);
                entity.Property(p => p.Notes).HasMaxLength(1000);

                // Ignore computed property
                entity.Ignore(p => p.DisplayName);

                // Player belongs to User
                entity.HasOne(p => p.User)
                      .WithMany(u => u.PlayerAccounts)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Player belongs to Game
                entity.HasOne(p => p.Game)
                      .WithMany(g => g.Players)
                      .HasForeignKey(p => p.GameId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ==================== KILL ====================

            modelBuilder.Entity<Kill>(entity =>
            {
                entity.HasKey(k => k.Id);

                entity.Property(k => k.Weapon).HasMaxLength(200);
                entity.Property(k => k.Reason).HasMaxLength(500).IsRequired();
                entity.Property(k => k.KillMessage).HasMaxLength(500).IsRequired();

                // Store TimeSpan as ticks
                entity.Property(k => k.TimeSinceAssignedTarget)
                      .HasConversion(
                          timeSpan => timeSpan.Ticks,
                          ticks => TimeSpan.FromTicks(ticks));

                // Kill belongs to Game
                entity.HasOne(k => k.Game)
                      .WithMany(g => g.Kills)
                      .HasForeignKey(k => k.GameId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Kill has a Killer (Player) — Restrict to avoid multiple cascade paths
                entity.HasOne(k => k.Killer)
                      .WithMany()
                      .HasForeignKey(k => k.KillerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Kill has a Victim (Player) — Restrict to avoid multiple cascade paths
                entity.HasOne(k => k.Victim)
                      .WithMany()
                      .HasForeignKey(k => k.VictimId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ==================== TARGET ASSIGNMENT ====================

            modelBuilder.Entity<TargetAssignment>(entity =>
            {
                entity.HasKey(ta => ta.Id);

                entity.Property(ta => ta.Weapon).HasMaxLength(200);

                // Hunter (Player) — Restrict to avoid multiple cascade paths
                entity.HasOne(ta => ta.Hunter)
                      .WithMany(p => p.TargetAssignments)
                      .HasForeignKey(ta => ta.HunterId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Target (Player) — Restrict, no inverse navigation
                entity.HasOne(ta => ta.Target)
                      .WithMany()
                      .HasForeignKey(ta => ta.TargetId)
                      .OnDelete(DeleteBehavior.Restrict);

                // TargetAssignment optionally has one Kill (shadow FK on TargetAssignment)
                entity.HasOne(ta => ta.Kill)
                      .WithOne()
                      .HasForeignKey<TargetAssignment>("KillId")
                      .IsRequired(false);
            });

            // ==================== LOG ====================

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.Message).HasMaxLength(1000);
                entity.Property(l => l.ExtraInfo).HasMaxLength(2000);

                // Exception objects can't be stored in the database — ignore this property
                entity.Ignore(l => l.Exception);

                // Log optionally has an Attacker
                entity.HasOne(l => l.Attacker)
                      .WithMany()
                      .HasForeignKey(l => l.AttackerId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ==================== ATTACKER ====================

            modelBuilder.Entity<Attacker>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.IpAddress).HasMaxLength(45);
                entity.Property(a => a.UserAgent).HasMaxLength(500);
                entity.Property(a => a.Referer).HasMaxLength(500);
                entity.Property(a => a.Path).HasMaxLength(500).IsRequired();
                entity.Property(a => a.InvalidInput).HasMaxLength(2000);
                entity.Property(a => a.SessionId).HasMaxLength(100);
                entity.Property(a => a.MacAddress).HasMaxLength(17);
            });
        }

        #region DbSets
        //Define Dbsets => Tables

        // Gotcha (game) Entities
        public DbSet<GotchaUser> GotchaUsers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Rules> Rules { get; set; }
        public DbSet<TargetAssignment> TargetAssignments { get; set; }
        public DbSet<Kill> Kills { get; set; }
        public DbSet<VipSettings> VipSettings { get; set; }

        // Logging Entities
        public DbSet<Attacker> Attackers { get; set; }
        public DbSet<Log> Logs { get; set; }
        #endregion
    }
}
