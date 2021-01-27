using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leds_run_azure_functions.Models
{
    class Response
    {
        //[JsonProperty(PropertyName = "status")]
        //public string Status { get; set; }

        [JsonProperty(PropertyName = "succes")]
        public bool Succes { get; set; }

        [JsonProperty(PropertyName = "info")]
        public string Info { get; set; }
    }
}
