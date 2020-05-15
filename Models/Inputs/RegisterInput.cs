namespace CustomTrackerBackend.Models.Inputs
{
    public class RegisterInput
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}