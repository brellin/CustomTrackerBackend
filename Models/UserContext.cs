using Microsoft.EntityFrameworkCore;

namespace CustomTrackerBackend.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.User)
                .WithMany(u => u.Issues)
                .HasForeignKey(i => i.UserId);
        }
    }
}