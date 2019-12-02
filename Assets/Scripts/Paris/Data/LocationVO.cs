using System;
namespace Ambition
{
    public class LocationVO
    {
        public string LocationID;
        public IncidentVO IntroIncident;
        public IncidentVO[] StoryIncidents;
        public String SceneID;
        public bool OneShot;
        public bool Discoverable;
        public RequirementVO[] Requirements;
    }
}

