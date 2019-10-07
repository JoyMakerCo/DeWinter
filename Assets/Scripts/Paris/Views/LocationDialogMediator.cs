using UnityEngine.UI;
using Dialog;

namespace Ambition
{
    public class LocationDialogMediator : DialogView<Pin>
    { 
        public const string DIALOG_ID = "PARIS_LOCATION";
        private Pin _location;
        public Text LocationNameText;
        public Text LocationDescriptionText;
        public Image LocationImage;

        public override void OnOpen(Pin location)
        {
            _location = location;
            LocationNameText.text = _location.Name;
            LocationDescriptionText.text = _location.LocationWindowDescription;
            LocationImage.sprite = _location.LocationModalSprite;
        }

        public void GoToLocation()
        {
            LocationVO location = _location?.GetLocation();
            if (location != null)
            {
                AmbitionApp.SendMessage(ParisMessages.GO_TO_LOCATION, location);
            }
        }
    }
}
