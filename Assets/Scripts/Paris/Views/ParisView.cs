﻿using UnityEngine;
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
        public GameObject RestButton;
        public GameObject EstateButton;

<<<<<<< Updated upstream
        void Awake()
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();

            // If the model hasn't been populated yet, do that now
            if (paris.Locations?.Count == 0)
                InitModel(paris);

            AmbitionApp.Subscribe<Pin>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Subscribe<string>(ParisMessages.ADD_LOCATION, HandleLocation);
            AmbitionApp.Subscribe<string>(ParisMessages.REMOVE_LOCATION, HandleHideLocation);
            AmbitionApp.SendMessage<string>(GameMessages.SET_TITLE, AmbitionApp.GetString("paris"));
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
=======
        // The model stores the IDs of known and visited one-shot locations
        // The view shows all known locations, up to MaxExplorable locations, and non-explorable locations that meet requirements.
        // Previously visited one-shot locations will not display.
        void Awake()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
>>>>>>> Stashed changes
            Pin pin;
            AmbitionApp.Subscribe<Pin>(ParisMessages.SELECT_LOCATION, HandleSelect);
            if (model.Dailies == null || model.Dailies.Length < AmbitionApp.GetModel<GameModel>().NumParisDailies)
            {
                List<Pin> dailies = new List<Pin>();
                foreach (Transform child in Pins)
                {
                    pin = child.GetComponent<Pin>();
                    if (pin != null)
                    {
                        if (pin.Discoverable && AmbitionApp.CheckRequirements(pin.Requirements) && !model.Visited.Contains(pin.name))
                        {
                            dailies.Add(pin);
                        }
                        pin.gameObject.SetActive(model.Locations.Contains(pin.name));
                        pin.HideLabel();
                    }
                }
                AmbitionApp.SendMessage(ParisMessages.SELECT_DAILIES, dailies.ToArray());
                foreach(Pin daily in dailies)
                {
                    if (Array.IndexOf(model.Dailies, daily.name) >= 0)
                    {
                        daily.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                foreach (Transform child in Pins)
                {
                    pin = child.GetComponent<Pin>();
                    pin?.gameObject.SetActive(model.Locations.Contains(pin.name) || Array.IndexOf(model.Dailies, pin.name) >= 0);
                }
            }
            AmbitionApp.SendMessage<string>(GameMessages.SHOW_HEADER, AmbitionApp.Localize("paris"));
        }

        //To Do: Add in the fader (Still needs to be made)
        private void HandleSelect(Pin location) => ParisLocationDialog?.Show(location);
    }
}
