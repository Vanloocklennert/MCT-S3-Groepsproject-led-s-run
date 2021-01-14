using System;
using System.Collections.Generic;
using System.Text;

namespace Leds_Run.models
{
    public class Workout
    {
        public List<Interval> Intervals { get; set; }

        public class Interval
        {
            public String Name { get; set; }
            public String Type { get; set; }
            public double Distance { get; set; }
            public double Speed { get; set; }
            public double Time { get; set; }
        }
    }
}
