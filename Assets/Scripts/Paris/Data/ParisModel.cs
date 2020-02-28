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
<<<<<<< Updated upstream
        public int NumExploreLocations = 5;

        [JsonIgnore]
        public Pin Location;

        [JsonProperty("location")]
        private string _Location
        {
            get => Location?.name;
            //set => 
            // TODO: On Restore, load the Paris Scenefab and consume the pin Prefab
        }

        [JsonIgnore] // List of explorable locations and their requirements
        public Dictionary<string, RequirementVO[]> Explorable = new Dictionary<string, RequirementVO[]>();
=======
        public LocationVO Location;

        [JsonProperty("location")]
        public string LocationID;
>>>>>>> Stashed changes

        [JsonProperty("daily")] // Explorable locations available "Today"
        public string[] Dailies = null; // This will be populated when Daily locations have been selected

        [JsonProperty("new")] // New recently discoved locations
        public List<string> New = new List<string>(); // These locations will animate in, and then be appended to the Known list

        [JsonProperty("locations")] // Known locations
        public List<string> Locations = new List<string>();

<<<<<<< Updated upstream
        [JsonProperty("visited")] // One-Shot locations that have been visited
=======
        [JsonProperty("visited")] // Locations that have been visited. This prevents one-shot locations from redisplaying.
>>>>>>> Stashed changes
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
