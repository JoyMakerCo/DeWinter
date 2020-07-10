using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

namespace Ambition
{
    public class Pin : MonoBehaviour
    {
        public IncidentConfig IntroIncidentConfig;

        public IncidentConfig[] StoryIncidentConfigs;
        public string SceneID;
        public bool OneShot;
        public string LocationWindowDescription;
        public Sprite LocationModalSprite;
        public GameObject Label;
        public bool Discoverable;
        public RequirementVO[] Requirements;

        public LocationVO GetLocation()
        {
            RequirementVO[] requirements = new RequirementVO[Requirements.Length];
            Array.Copy(Requirements, requirements, requirements.Length);
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();

            string[] incidents = new string[StoryIncidentConfigs.Length];
            IncidentVO incident;
            if (IntroIncidentConfig != null && !model.Incidents.ContainsKey(IntroIncidentConfig.name))
            {
                model.Incidents.Add(IntroIncidentConfig.name, IntroIncidentConfig.GetIncident());
            }
            for (int i= StoryIncidentConfigs.Length-1; i>=0; --i)
            {
                if (StoryIncidentConfigs[i] != null && !model.Incidents.ContainsKey(StoryIncidentConfigs[i].name))
                {
                    incident = StoryIncidentConfigs[i].GetIncident();
                    if (incident != null)
                    {
                        model.Incidents[incident.ID] = incident;
                    }
                    incidents[i] = incident?.ID;
                }
            }

            return new LocationVO()
            {
                ID = name,
                IntroIncident = IntroIncidentConfig?.name,
                StoryIncidents = incidents,
                SceneID = SceneID,
                OneShot = OneShot,
                Discoverable = Discoverable,
                Requirements = requirements
            };
        }

        public void HandleClick()
        {
            AmbitionApp.SendMessage<Pin>(ParisMessages.SELECT_LOCATION, this);
        }

        public void ShowLabel()
        {
            Label.SetActive(true);
        }

        public void HideLabel()
        {
            Label.SetActive(false);
        }

        public string Name { get; private set; }
    }
}
