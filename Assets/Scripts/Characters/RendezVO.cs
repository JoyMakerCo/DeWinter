using System;
using Newtonsoft.Json;
namespace Ambition
{
    [Serializable]
    public class RendezVO : CalendarEvent
    {
        [JsonIgnore]
        public string Character
        {
            get => ID;
            set => ID = value;
        }

        [JsonProperty("location")]
        public string Location;

        [JsonProperty("caller")]
        public bool IsCaller; // true = player initated
    }
}
