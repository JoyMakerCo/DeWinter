using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class LocationModal : MonoBehaviour {

        public LocationPin Location;
        public Text LocationNameText;
        public Text LocationDescriptionText;
        public Image LocationImage;

        public void ShowWindow()
        {
            LocationNameText.text = Location.Name();
            LocationDescriptionText.text = Location.LocationWindowDescription;
            //LocationImage.sprite = Location.LocationWindowImage; <- Waiting on the images for the locations to fit in this window
            gameObject.SetActive(true);
        }

        public void HideWindow()
        {
            gameObject.SetActive(false);
        }

        public void GoToLocation()
        {
            if(Location.Name() != "Home")
            {
                AmbitionApp.SendMessage<string>(ParisMessages.GO_TO_LOCATION, Location.name);
            } else
            {
                AmbitionApp.SendMessage(ParisMessages.REST);
            }
            gameObject.SetActive(false);
        }
    }
}
