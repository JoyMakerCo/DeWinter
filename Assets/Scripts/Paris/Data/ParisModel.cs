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
        public LocationVO Location;

        [JsonProperty("location")]
        public string LocationID;

        [JsonProperty("daily")] // Explorable locations available "Today"
        public string[] Dailies = null; // This will be populated when Daily locations have been selected

        [JsonProperty("new")] // New recently discoved locations
        public List<string> New = new List<string>(); // These locations will animate in, and then be appended to the Known list

        [JsonProperty("locations")] // Known locations
        public List<string> Locations = new List<string>();

        [JsonProperty("visited")] // Locations that have been visited. This prevents one-shot locations from redisplaying.
        public List<string> Visited = new List<string>();

        public void Reset()
        {
            Location = null;
            LocationID = null;
            Dailies = null;
            New = null;
            Locations.Clear();
            Visited.Clear();
        }
    }
}
