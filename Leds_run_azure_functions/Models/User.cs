using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leds_run_azure_functions.Models
{
    class User
    {
        [JsonIgnore]
        public int User_Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string EMail { get; set; }

        //[JsonProperty(PropertyName = "passwordhash")]
        //public string PasswordHash { get; set; }

        // Make it possible to GET and SET the PasswordHash Value but with Json only to SET it.
        [JsonIgnore]
        public string PasswordHash { get; set; }

        [JsonProperty(PropertyName = "passwordhash")]
        private string SetPasswordHash
        {
            set => PasswordHash = value;
        }
    }
}
