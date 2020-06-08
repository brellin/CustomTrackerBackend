using System.ComponentModel.DataAnnotations;

namespace CustomTrackerBackend.Models.Inputs
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
