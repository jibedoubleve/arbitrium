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
        }

        #endregion Methods
    }
}