using System;
namespace Ambition
{
    public class LocationVO
    {
        public string ID;
        public string IntroIncident;
        public string[] StoryIncidents;
        public String SceneID;
        public bool IsOneShot;
        public bool IsDiscoverable;
        public bool IsRendezvous;
        public RequirementVO[] Requirements;
    }
}
