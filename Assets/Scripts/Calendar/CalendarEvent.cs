using System;
using Newtonsoft.Json;

namespace Ambition
{
    [Serializable]
    public class CalendarEvent : IComparable<CalendarEvent>
    {
        [JsonProperty("id")]
        public string ID = null;

        [JsonProperty("day")]
        public int Day = -1;

        [JsonProperty("created")]
        public int Created = -1; // Date event created; -1 = new

        [JsonProperty("rsvp")]
        public RSVP RSVP = RSVP.New;

        [JsonIgnore]
        public bool IsAttending => RSVP == RSVP.Accepted || RSVP == RSVP.Required;

        public int CompareTo(CalendarEvent o) => o != null ? Day.CompareTo(((CalendarEvent)o).Day) : 1;
    }
}
