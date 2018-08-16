﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

namespace Ambition
{
    public class LocationPin : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector]
        public string Incident = null;

        public GameObject Scene = null;
        public bool OneShot = false;
        public bool Discoverable = false;
        public GameObject Tooltip;
        public CommodityVO[] Requirements;

        private void Awake()
        {
            AmbitionApp.Subscribe<string>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Subscribe<string>(ParisMessages.SHOW_LOCATION, HandleShow);
            AmbitionApp.Subscribe<string>(ParisMessages.HIDE_LOCATION, HandleHide);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Unsubscribe<string>(ParisMessages.SHOW_LOCATION, HandleShow);
            AmbitionApp.Unsubscribe<string>(ParisMessages.HIDE_LOCATION, HandleHide);
        }

#if (UNITY_EDITOR)
        public IncidentConfig IncidentConfig;

        private void OnValidate()
        {
            Incident = IncidentConfig?.name;
        }
#endif

        public void OnPointerClick(PointerEventData data)
        {
            AmbitionApp.SendMessage<string>(ParisMessages.SELECT_LOCATION, this.name);
        }

        private void HandleSelect(string location)
        {
            Tooltip.SetActive(location == gameObject.name);
        }

        public void GoToLocation()
        {
            AmbitionApp.SendMessage<string>(ParisMessages.GO_TO_LOCATION, this.name);
        }

        private void HandleShow(string locationID)
        {
            if (locationID == name)
                gameObject.SetActive(true);
        }

        private void HandleHide(string locationID)
        {
            if (locationID == name)
                gameObject.SetActive(false);
        }
    }
}
