using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomTrackerBackend.Models
{
    public class Issue
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsComplete { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}