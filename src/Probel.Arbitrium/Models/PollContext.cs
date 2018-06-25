using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Probel.Arbitrium.Models
{
    public class PollContext : DbContext
    {
        #region Constructors

        public PollContext(DbContextOptions<PollContext> options) : base(options)
        {
        }

        #endregion Constructors

        #region Properties

        public DbSet<Choice> Choices { get; set; }

        public DbSet<Decision> Decisions { get; set; }

        public DbSet<IdentityRoleClaim<long>> IdentityRoleClaims { get; set; }
        public DbSet<IdentityRole<long>> IdentityRoles { get; set; }
        public DbSet<IdentityUserClaim<long>> IdentityUserClaims { get; set; }
        public DbSet<IdentityUserRole<long>> IdentityUserRoles { get; set; }
        public DbSet<Poll> Polls { get; set; }

        public DbSet<User> Users { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Choice>()
                .HasOne(p => p.Poll)
                .WithMany(p => p.Choices)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IdentityUserRole<long>>().HasKey(p => new { p.UserId, p.RoleId });
        }

        #endregion Methods
    }
}