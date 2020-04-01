using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    [Saveable]
    public class ParisModel : Model, IResettable
    {
        [JsonProperty("location")]
        public LocationVO Location;

        public uint NumDailies = 5;

        [JsonProperty("daily")] // Explorable locations available "Today"
        public string[] Daily = null; // This will be populated when Daily locations have been selected

        [JsonProperty("new")] // New recently discoved locations
        public string[] New = null; // These locations will animate in, and then be appended to the Known list

        [JsonProperty("locations")] // Known locations
        public List<string> Locations = new List<string>();

        [JsonProperty("visited")] // Locations that have been visited. This prevents one-shot locations from redisplaying.
        public List<string> Visited = new List<string>();

        public void Reset()
        {
            Location = null;
            Daily = null;
            New = null;
            NumDailies = 0;
            Locations.Clear();
            Visited.Clear();
        }
    }
}
