using System.ComponentModel.DataAnnotations;

namespace CustomTracker.Models.Inputs
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
