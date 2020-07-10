using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Util;

namespace Ambition
{
    public class ParisView : MonoBehaviour
    {
        public Transform Pins;
        public GameObject LegendBtn;
        public Text LegendBtnLabel;
        public GameObject Legend;
        public Animator LegendAnimationController;
        public LocationDialogMediator ParisLocationDialog;

        private Dictionary<string, Pin> _pins = new Dictionary<string, Pin>();

        // The model stores the IDs of known and visited one-shot locations
        // The view shows all known locations, several random locations, and non-explorable locations that meet requirements.
        // Previously visited one-shot locations will not display.
        void Awake()
        {
            Pin pin;
            string pinId;
            List<string> dailies = new List<string>();

            AmbitionApp.Subscribe<Pin>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Subscribe<string>(ParisMessages.SHOW_LOCATION, HandleShow);
            AmbitionApp.Subscribe<string>(ParisMessages.ADD_LOCATION, HandleShow);
            AmbitionApp.Subscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHide);

            foreach(Transform child in Pins)
            {
                pin = child.GetComponent<Pin>();
                pinId = pin?.name;
                if (pinId != null)
                {
                    _pins[pinId] = pin;
                    if (pin.Discoverable) dailies.Add(pinId);
                }
            }
            AmbitionApp.SendMessage(ParisMessages.SELECT_DAILIES, dailies.ToArray());
            AmbitionApp.SendMessage<string>(GameMessages.SHOW_HEADER, AmbitionApp.Localize("paris"));
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<Pin>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Unsubscribe<string>(ParisMessages.SHOW_LOCATION, HandleShow);
            AmbitionApp.Unsubscribe<string>(ParisMessages.ADD_LOCATION, HandleShow);
            AmbitionApp.Unsubscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHide);
        }

        //To Do: Add in the fader (Still needs to be made)
        private void HandleSelect(Pin location) => ParisLocationDialog?.Show(location);

        private void HandleShow(string locationID)
        {
            if (_pins.TryGetValue(locationID, out Pin pin))
                pin.gameObject?.SetActive(true);
        }

        private void HandleHide(string locationID)
        {
            if (_pins.TryGetValue(locationID, out Pin pin))
                pin.gameObject?.SetActive(false);
        }
    }
}
