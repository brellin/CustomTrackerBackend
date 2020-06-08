using System;
using System.Threading.Tasks;
using CustomTrackerBackend.Helpers;
using CustomTrackerBackend.Models.Inputs;
using Microsoft.EntityFrameworkCore;

namespace CustomTrackerBackend.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
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
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Group)
                .WithMany(g => g.Issues)
                .HasForeignKey(i => i.GroupId);
            modelBuilder.Entity<Group>()
                .HasKey(g => g.Id);
            modelBuilder.Entity<Group>()
                .HasIndex(g => g.Name)
                .IsUnique();
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Owner)
                .WithMany(u => u.Groups)
                .HasForeignKey(g => g.OwnerId);
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
            try
            {
                User match = await Users.FirstAsync(u => u.Username == input.Username);
                bool passwordsMatch = match.PasswordHash.Equals(passwordHash);
                if (passwordsMatch) return match;
                else throw new Exception("Passwords do not match");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Passwords do not match") throw ex;
                throw new Exception($"User '{input.Username}' does not exist");
            }
        }
    }
}
