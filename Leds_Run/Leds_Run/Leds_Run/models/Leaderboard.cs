using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Leds_Run.models
{
    class Leaderboard
    {
        public List<Entry> entries { get; set; }
        
        public class Entry
        {
            public int Leaderboard_id { get; set; }

            [JsonProperty(PropertyName = "username")]
            public string Username { get; set; }

            [JsonProperty(PropertyName = "time")]
            public TimeSpan Time { get; set; }

            [JsonProperty(PropertyName = "distance")]
            public decimal Distance { get; set; }

            [JsonProperty(PropertyName = "speed")]
            public decimal Speed { get; set; }

            [JsonProperty(PropertyName = "dateTime")]
            public DateTime DateTime { get; set; }
        }
    }
}
