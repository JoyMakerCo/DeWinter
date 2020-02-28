using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class LocationDialogMediator : MonoBehaviour
    { 
        public const string DIALOG_ID = "PARIS_LOCATION";
        private Pin _pin;
        public Text LocationNameText;
        public Text LocationDescriptionText;
        public Image LocationImage;
        public Button GoButton;
        public Button CloseButton;

        public void Show(Pin location)
        {
            _pin = location;
            gameObject.SetActive(_pin != null);
            if (_pin != null)
            {
                LocationNameText.text = _pin.name;
                LocationDescriptionText.text = location.LocationWindowDescription;
                LocationImage.sprite = location.LocationModalSprite;
            }
        }

        /************************************************************************************************
        Private/Protected methods
        ************************************************************************************************/

        private void Awake()
        {
            GoButton.onClick.AddListener(HandleClick);
            CloseButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _pin = null;
            GoButton.onClick.RemoveAllListeners();
            CloseButton.onClick.RemoveAllListeners();
        }

        private void HandleClick()
        {
            LocationVO location = _pin?.GetLocation();
            if (location != null)
            {
                AmbitionApp.SendMessage(ParisMessages.GO_TO_LOCATION, location);
                AmbitionApp.SendMessage(ParisMessages.GO_TO_LOCATION);
            }
        }
    }
}
