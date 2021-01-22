using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leds_run_azure_functions.Models
{
    class Leaderboard
    {
        [JsonIgnore]
        public int LeaderboardId { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "time")]
        public TimeSpan Time { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public double Speed { get; set; }

        public DateTime DateTime { get; set; }
    }
}
