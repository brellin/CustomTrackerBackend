using Microsoft.EntityFrameworkCore;

namespace CustomTrackerBackend.Models
{
    public class IssueContext : DbContext
    {
        public IssueContext(DbContextOptions<IssueContext> options) : base(options) { }
        public DbSet<Issue> Issue { get; set; }
        public DbSet<User> User { get; set; }
    }
}
