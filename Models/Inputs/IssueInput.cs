namespace CustomTrackerBackend.Models.Inputs
{
    public class IssueInput
    {
        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public string Detail { get; set; }

        public string Group { get; set; }
    }
}
