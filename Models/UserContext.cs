using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CustomTrackerBackend.Models
{
    public class UserContext : IdentityDbContext<User>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .ToTable("users")
                .Ignore(f => f.AccessFailedCount)
                .Ignore(f => f.ConcurrencyStamp)
                .Ignore(f => f.EmailConfirmed)
                .Ignore(f => f.LockoutEnabled)
                .Ignore(f => f.LockoutEnd)
                .Ignore(f => f.PhoneNumber)
                .Ignore(f => f.PhoneNumberConfirmed)
                .Ignore(f => f.SecurityStamp)
                .Ignore(f => f.TwoFactorEnabled);
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.User)
                .WithMany(u => u.Issues)
                .HasForeignKey(i => i.UserId);
        }
    }
}