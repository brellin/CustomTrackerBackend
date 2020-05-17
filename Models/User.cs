using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using System;

namespace CustomTrackerBackend.Models
{
    public class User : IdentityUser
    {
        public virtual List<Issue> Issues { get; set; }

        // Ignoring Identity fields
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override string PasswordHash { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override string Email { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override string PhoneNumber { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override string NormalizedUserName { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override string NormalizedEmail { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override bool EmailConfirmed { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override string SecurityStamp { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override string ConcurrencyStamp { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override bool LockoutEnabled { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public override int AccessFailedCount { get; set; }
    }
}