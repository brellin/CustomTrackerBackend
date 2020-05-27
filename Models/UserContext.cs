using System;
using System.Threading.Tasks;
using CustomTracker.Helpers;
using CustomTracker.Models.Inputs;
using Microsoft.EntityFrameworkCore;

namespace CustomTracker.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.User)
                .WithMany(u => u.Issues)
                .HasForeignKey(i => i.UserId);
        }

        public async Task<User> CreateUserAsync(LoginInput input)
        {
            string passwordHash = input.Password.HashPassword();
            User fullUser = new User()
            {
                Username = input.Username,
                PasswordHash = passwordHash
            };
            await AddAsync(fullUser);
            await SaveChangesAsync();
            return fullUser;
        }

        public async Task<User> SignInUser(LoginInput input)
        {
            string passwordHash = input.Password.HashPassword();
            User match = await Users.FirstAsync(u => u.Username == input.Username);
            if (match.PasswordHash.Equals(passwordHash)) return match;
            else throw new Exception("Passwords do not match");
        }
    }
}
