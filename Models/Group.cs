using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CustomTrackerBackend.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual List<Issue> Issues { get; set; }
    }
}
