using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CustomTrackerBackend.Models
{
    public class User : IdentityUser
    {
        public List<Issue> Issues { get; set; }
    }
}