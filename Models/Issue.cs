using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomTrackerBackend.Models
{
    public class Issue
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsComplete { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}