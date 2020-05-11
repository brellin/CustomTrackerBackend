using System.ComponentModel.DataAnnotations.Schema;

namespace CustomTrackerBackend.Models
{
    public class Issue
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        [ForeignKey("UserName")]
        public string UserName { get; set; }
        public User User { get; set; }
    }
}