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
        public int NumExploreLocations = 5;

        [JsonIgnore]
        public LocationVO Location;

        [JsonProperty("location")]
        private string _Location
        {
            get => Location?.LocationID;
            //set => 
            // TODO: On Restore, load the Paris Scenefab and consume the pin Prefab
        }

        [JsonIgnore] // List of explorable locations and their requirements
        public Dictionary<string, RequirementVO[]> Explorable = new Dictionary<string, RequirementVO[]>();

        [JsonProperty("daily")] // Explorable locations available "Today"
        public List<string> Daily = new List<string>();

        [JsonIgnore]
        // Locations that are unlocked via requirements and directly added to Known locations
        public Dictionary<string, RequirementVO[]> Locations = new Dictionary<string, RequirementVO[]>();

        [JsonProperty("known")] // Known locations
        public List<string> Known = new List<string>();

        [JsonProperty("visited")] // Locations that have been visited.
        public List<string> Visited = new List<string>();

        public void Reset()
        {
            Location = null;
            Locations.Clear();
            Visited.Clear();
            Daily.Clear();
        }
    }
}
