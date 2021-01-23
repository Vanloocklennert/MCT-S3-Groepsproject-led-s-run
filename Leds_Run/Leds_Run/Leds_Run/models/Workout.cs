using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Leds_Run.models
{

    public class Workout
    {
        public List<Interval> Intervals { get; set; }

        
        public class Interval
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "type")]
            public string Type { get; set; }

            [JsonProperty(PropertyName = "distance")]
            public double Distance { get; set; }

            [JsonProperty(PropertyName = "speed")]
            public double Speed { get; set; }

            [JsonProperty(PropertyName = "time")]
            public DateTime Time { get; set; }
        }
    }
}
