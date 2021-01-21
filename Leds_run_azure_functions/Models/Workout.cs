using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leds_run_azure_functions.Models
{
    class Workout
    {
        public int Default_Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public double Speed { get; set; }

        [JsonProperty(PropertyName = "time")]
        public TimeSpan Time { get; set; }
    }
}
