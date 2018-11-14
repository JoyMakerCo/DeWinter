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
            LocationNameText.text = _location.Name();
            LocationDescriptionText.text = _location.LocationWindowDescription;
            LocationImage.sprite = _location.LocationModalSprite;
        }

        public void GoToLocation()
        {
            AmbitionApp.SendMessage(ParisMessages.GO_TO_LOCATION, _location);
        }
    }
}
