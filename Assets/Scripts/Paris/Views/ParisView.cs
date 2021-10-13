using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Util;

using UnityEditor;


namespace Ambition
{
    public class ParisView : SceneView, IButtonInputHandler, IAnalogInputHandler, ISubmitHandler
    {
        public Transform Pins;

        private LocationConfig _selected = null;

        private Dictionary<string, Pin> _pins = new Dictionary<string, Pin>();
        private Dictionary<string, Vector2> _locations = new Dictionary<string, Vector2>();

        public void HandleInput(Vector2 [] input)
        {
        }

        public void Submit()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void Cancel()
        {
#if UNITY_STANDALONE
            AmbitionApp.OpenDialog(DialogConsts.GAME_MENU);
#else
#endif
        }

        public void HandleInput(string btn, bool holding)
        {
            if (!holding)
            {
                switch (btn)
                {
                    case "x":

                        break;
                    case "y":

                        break;
                }
            }
        }

        // The model stores the IDs of known and visited one-shot locations
        // The view shows all known locations, several random locations, and non-explorable locations that meet requirements.
        // Previously visited one-shot locations will not display.
        void Awake()
        {
            Pin pin;
            string pinId;
            List<string> dailies = new List<string>();

            AmbitionApp.Subscribe<LocationConfig>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Subscribe<string>(ParisMessages.SHOW_LOCATION, HandleShow);

            foreach (Transform child in Pins)
            {
                pin = child.GetComponent<Pin>();
                pinId = pin?.name;
                if (!string.IsNullOrEmpty(pinId))
                {
                    _pins[pinId] = pin;
                    if (pin.IsDiscoverable) dailies.Add(pinId);
                }
            }
            AmbitionApp.SendMessage(ParisMessages.SELECT_DAILIES, dailies.ToArray());
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<LocationConfig>(ParisMessages.SELECT_LOCATION, HandleSelect);
            AmbitionApp.Unsubscribe<string>(ParisMessages.SHOW_LOCATION, HandleShow);
        }

        //To Do: Add in the fader (Still needs to be made)
        private void HandleSelect(LocationConfig location)
        {
            _selected = location;
            AmbitionApp.OpenDialog(DialogConsts.PARIS_LOCATION, _selected);
        }

        private void HandleShow(string locationID)
        {
            if (_pins.TryGetValue(locationID, out Pin pin))
            {
                pin?.gameObject?.SetActive(true);
            }
        }
    }
}
