using System;
using System.Collections.Generic;

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
        public string PasswordHash { get; set; }
        public virtual List<Issue> Issues { get; set; }
    }
}
