namespace CustomTracker.Models
{
    public class Issue
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public string Detail { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
