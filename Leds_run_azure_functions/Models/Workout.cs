using System;
using System.Collections.Generic;
using System.Text;

namespace Leds_run_azure_functions.Models
{
    class Workout
    {
        public int Default_Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public double Speed { get; set; }
        public TimeSpan Time { get; set; }
    }
}
