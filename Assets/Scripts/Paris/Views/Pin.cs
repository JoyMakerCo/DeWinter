﻿using System;
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
        public string SceneID;
        public bool OneShot;
        public string LocationWindowDescription;
        public Sprite LocationModalSprite;
        public GameObject Label;
        public bool Discoverable;
        public Text LabelText;
        public RequirementVO[] Requirements;

        public LocationVO GetLocation()
        {
            RequirementVO[] requirements = new RequirementVO[Requirements.Length];
            Array.Copy(Requirements, requirements, requirements.Length);
            return new LocationVO()
            {
                LocationID = name,
                Incident = IntroIncidentConfig.GetIncident(),
                SceneID = SceneID,
                OneShot = OneShot,
                Discoverable = Discoverable,
                Requirements = requirements
            };
        }

        private void Awake()
        {
            AmbitionApp.Subscribe<string>(ParisMessages.ADD_LOCATION, HandleShow);
            AmbitionApp.Subscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHide);
            Name = LabelText.text;
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(ParisMessages.ADD_LOCATION, HandleShow);
            AmbitionApp.Unsubscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHide);
        }

        public void Select()
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

        private void HandleSelect(string location)
        {
            //Tooltip.SetActive(location == gameObject.name);
        }

        public string Name { get; private set; }

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
