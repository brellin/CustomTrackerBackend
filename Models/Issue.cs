using System.ComponentModel.DataAnnotations.Schema;

namespace CustomTrackerBackend.Models
{
    public class Issue
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}