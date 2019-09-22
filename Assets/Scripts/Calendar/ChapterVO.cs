using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    public class ChapterVO : ICalendarEvent
    {
        [JsonProperty("date")]
        private long _date
        {
            set => Date = new DateTime(value);
            get => Date.Ticks;
        }
        public DateTime Date { get; set; }

        public string Name { get; set; }

        [JsonProperty("complete")]
        public bool IsComplete { set; get; }

        [JsonIgnore]
        public Sprite Splash;

        [JsonIgnore]
        public FMODEvent Sting;
    }
}
