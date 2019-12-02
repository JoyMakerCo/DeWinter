using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambition
{
    public class ParisView : MonoBehaviour
    {
        public Transform Pins;

        void Awake()
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();

            // If the model hasn't been populated yet, do that now
            if (paris.Locations?.Count == 0)
                InitModel(paris);

            AmbitionApp.Subscribe<Pin>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Subscribe<string>(ParisMessages.ADD_LOCATION, HandleLocation);
            AmbitionApp.Subscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHideLocation);
            AmbitionApp.SendMessage<string>(GameMessages.SET_TITLE, AmbitionApp.Localize("paris"));
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<Pin>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Unsubscribe<string>(ParisMessages.ADD_LOCATION, HandleLocation);
            AmbitionApp.Unsubscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHideLocation);
        }

        //To Do: Add in the fader (Still needs to be made)
        private void HandleSelect(Pin location) => AmbitionApp.OpenDialog("PARIS_LOCATION", location);
        private void HandleLocation(string location) => Pins?.Find(location)?.gameObject?.SetActive(true);
        private void HandleHideLocation(string location) => Pins?.Find(location)?.gameObject?.SetActive(false);


        // Called the first time this view is opened
        // Populates the Paris model with all Explorable and Requirements-triggered locations
        private void InitModel(ParisModel paris)
        {
            Pin pin;
            foreach (Transform child in Pins)
            {
                pin = child.GetComponent<Pin>();
                if (pin != null)
                {
                    if (pin.Discoverable)
                    {
                        paris.Explorable[pin.name] = pin.Requirements;
                    }
                    else if (pin.Requirements?.Length > 0)
                    {
                        paris.Locations[pin.name] = pin.Requirements;
                    }
                }
            }
        }
    }
}
