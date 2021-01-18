using System;
using System.Collections.Generic;
using System.Text;

namespace Leds_run_azure_functions.Models
{
    class User
    {
        public int User_Id { get; set; }
        public string Username { get; set; }
        public string EMail { get; set; }
        public string PasswordHash { get; set; }
    }
}
