using System;
using System.Runtime.Serialization;
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
        public string Location;

        public int NumDailies = 5;

        [JsonIgnore] // TODO
        public RequirementVO[] ExploreParisRequirements = new RequirementVO[]
        {
            new RequirementVO()
            {
                Type = CommodityType.Date,
                ID = "March",
                Value = 24,
                Operator = RequirementOperator.GreaterOrEqual
            }
        };

        [JsonProperty("daily")] // Explorable locations available "Today"
        public string[] Daily = null; // This will be populated when Daily locations have been selected

        [JsonProperty("exploration")] // Explored Locations.
        public List<string> Exploration = new List<string>();

        [JsonProperty("completed")] // Locations that cannot be revisited.
        public List<string> Completed = new List<string>();

        [JsonProperty("rendezvous")] // Explored Rendezvous locations.
        public List<string> Rendezvous = new List<string>();

        [JsonIgnore] // All known Location data
        public Dictionary<string, LocationVO> Locations = new Dictionary<string, LocationVO>();

        public bool IsComplete(string locationID) => Completed.Contains(locationID);

        public LocationVO GetLocation(string locationID)
        {
            if (string.IsNullOrEmpty(locationID)) return null;
            Locations.TryGetValue(locationID, out LocationVO location);
            if (location != null
                && (string.IsNullOrEmpty(location.IntroIncident) || AmbitionApp.Story.Incidents.ContainsKey(location.IntroIncident))
                && Array.TrueForAll(location.StoryIncidents, AmbitionApp.Story.Incidents.ContainsKey))
                return location;

            // If the Location isn't currently stored, load it
            LocationConfig config = UnityEngine.Resources.Load<LocationConfig>(ParisConsts.DIR + locationID);
            return SaveLocation(config);
        }

        public LocationVO GetLocation() => string.IsNullOrEmpty(Location)
            ? null
            : Locations.TryGetValue(Location, out LocationVO loc)
            ? loc
            : null;

        public void Reset()
        {
            Daily = null;
            Exploration.Clear();
            Rendezvous.Clear();
            Completed.Clear();
            Location = null;
        }

        public LocationVO SaveLocation(LocationConfig config)
        {
            if (config == null) return null;

            List<string> incidents = new List<string>();
            IncidentModel story = AmbitionApp.Story;
            IncidentVO incident = config.IntroIncidentConfig?.GetIncident();
            if (incident != null)
            {
                story.Incidents[incident.ID] = incident;
                story.AddDependency(incident, config.name, IncidentType.Location);
            }

            if (config.StoryIncidentConfigs != null)
            {
                foreach (IncidentConfig iconfig in config.StoryIncidentConfigs)
                {
                    incident = iconfig?.GetIncident();
                    if (incident != null)
                    {
                        incidents.Add(incident.ID);
                        story.Incidents[incident.ID] = incident;
                        story.AddDependency(incident, config.name, IncidentType.Location);
                    }
                }
            }

            return Locations[config.name] = new LocationVO()
            {
                ID = config.name,
                IntroIncident = config.IntroIncidentConfig?.name,
                StoryIncidents = incidents.ToArray(),
                SceneID = config.SceneID,
                IsOneShot = config.OneShot,
                IsDiscoverable = config.IsDiscoverable,
                IsRendezvous = config.IsRendezvous,
                Requirements = config.Requirements ?? new RequirementVO[0]
            };
        }

        ////////////// PRIVATE/PROTECTED METHODS ///

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Daily != null) LoadAll(Daily);
            LoadAll(Exploration);
            LoadAll(Rendezvous);
        }

        private void LoadAll(IEnumerable<string> list)
        {
            foreach(string locID in list)
            {
                GetLocation(locID);
            }
        }
    }
}
