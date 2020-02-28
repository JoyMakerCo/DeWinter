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
        public GameObject RestButton;
        public GameObject EstateButton;

        // The model stores the IDs of known and visited one-shot locations
        // The view shows all known locations, up to MaxExplorable locations, and non-explorable locations that meet requirements.
        // Previously visited one-shot locations will not display.
<<<<<<< Updated upstream
        void Awake()
        {
=======
        void Awake()
        {
>>>>>>> Stashed changes
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            Pin pin;
            AmbitionApp.Subscribe<Pin>(ParisMessages.SELECT_LOCATION, HandleSelect);
            if (model.Dailies == null || model.Dailies.Length < AmbitionApp.GetModel<GameModel>().NumParisDailies)
<<<<<<< Updated upstream
            {
                List<Pin> dailies = new List<Pin>();
=======
            {
                List<Pin> dailies = new List<Pin>();
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
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
=======
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
>>>>>>> Stashed changes
                foreach (Transform child in Pins)
                {
                    pin = child.GetComponent<Pin>();
                    pin?.gameObject.SetActive(model.Locations.Contains(pin.name) || Array.IndexOf(model.Dailies, pin.name) >= 0);
<<<<<<< Updated upstream
                }
=======
                }
>>>>>>> Stashed changes
            }
            AmbitionApp.SendMessage<string>(GameMessages.SHOW_HEADER, AmbitionApp.Localize("paris"));
        }

        //To Do: Add in the fader (Still needs to be made)
        private void HandleSelect(Pin location) => ParisLocationDialog?.Show(location);
    }
}
