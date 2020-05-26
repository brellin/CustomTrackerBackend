using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomTrackerBackend.Models
{
    public class User
    {
        private string guid = Guid.NewGuid().ToString();
        public string Id
        {
            get { return guid; }
            set { guid = value; }
        }
        public string Username { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
        public virtual List<Issue> Issues { get; set; }
    }
}
