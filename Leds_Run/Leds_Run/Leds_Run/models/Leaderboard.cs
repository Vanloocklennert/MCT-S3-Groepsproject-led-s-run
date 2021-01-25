using System;
using System.Collections.Generic;
using System.Text;

namespace Leds_Run.models
{
    class Leaderboard
    {
        public List<Entry> entries { get; set; }

        public class Entry
        {
            public int Leaderboard_id { get; set; }
            public string username { get; set; }
            public TimeSpan Time { get; set; }
            public decimal distance { get; set; }
            public decimal speed { get; set; }
            public DateTime dateTime { get; set; }
        }
    }
}
