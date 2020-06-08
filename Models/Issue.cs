using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CustomTrackerBackend.Models
{
    public class Issue
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public string Detail { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [JsonIgnore]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}
