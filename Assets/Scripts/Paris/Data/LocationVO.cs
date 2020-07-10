using System;
namespace Ambition
{
    // Serialization: Incidents and Scene by string ID
    // Currently no locations have requirements
    // Serialize without for now, but add if reqs added
    public class LocationVO
    {
        public string ID;
        public string IntroIncident;
        public string[] StoryIncidents;
        public String SceneID;
        public bool OneShot;
        public bool Discoverable;
        public RequirementVO[] Requirements;
    }
}
