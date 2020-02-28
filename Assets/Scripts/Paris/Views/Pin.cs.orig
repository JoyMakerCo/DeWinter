using System;
using System.Linq;
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

            IncidentVO[] storyIncidents = StoryIncidentConfigs.Select( x => x.GetIncident() ).ToArray();

            return new LocationVO()
<<<<<<< Updated upstream
            {
=======
            {
>>>>>>> Stashed changes
                ID = name,
                IntroIncident = IntroIncidentConfig.GetIncident(),
                StoryIncidents = storyIncidents,
                SceneID = SceneID,
                OneShot = OneShot,
                Discoverable = Discoverable,
                Requirements = requirements
            };
<<<<<<< Updated upstream
        }

=======
        }

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream

=======

>>>>>>> Stashed changes
        /******************************************************
         Private/Protected      
         *******************************************************/      

        private void Awake()
        {
            AmbitionApp.Subscribe<string>(ParisMessages.ADD_LOCATION, HandleShow);
            AmbitionApp.Subscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHide);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(ParisMessages.ADD_LOCATION, HandleShow);
            AmbitionApp.Unsubscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHide);
        }

        private void HandleShow(string locationID)
        {
            if (locationID == name)
                gameObject?.SetActive(true);
        }

        private void HandleHide(string locationID)
        {
            if (locationID == name)
                gameObject?.SetActive(false);
        }
    }
}
