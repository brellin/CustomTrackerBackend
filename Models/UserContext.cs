using Microsoft.EntityFrameworkCore;

namespace CustomTrackerBackend.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .Ignore(i => i.Email)
                .Ignore(i => i.EmailConfirmed)
                .Ignore(i => i.NormalizedEmail)
                .Ignore(i => i.PhoneNumber)
                .Ignore(i => i.PhoneNumberConfirmed)
                .Ignore(i => i.TwoFactorEnabled);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Issues)
                .WithOne();

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.User)
                .WithMany(u => u.Issues)
                .HasForeignKey(i => i.UserName);
        }
    }
}