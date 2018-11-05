using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialog;

namespace Ambition
{
    public class LocationDialogMediator : DialogView, Util.IInitializable<LocationPin>
    { 

        public const string DIALOG_ID = "PARIS_LOCATION";
        private LocationPin _location;
        public Text LocationNameText;
        public Text LocationDescriptionText;
        public Image LocationImage;

        public void Initialize(LocationPin location)
        {
            _location = location;
            SetUpWindow();
        }

        private void SetUpWindow()
        {
            LocationNameText.text = _location.Name();
            LocationDescriptionText.text = _location.LocationWindowDescription;
            LocationImage.sprite = _location.LocationModalSprite;
        }

        public void GoToLocation()
        {
            AmbitionApp.SendMessage<LocationPin>(ParisMessages.GO_TO_LOCATION, _location);
        }

        public override void OnOpen()
        {
            AmbitionApp.SendMessage<string>(GameMessages.DIALOG_OPENED, ID);
        }

        public override void OnClose()
        {
            AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
        }
    }
}
