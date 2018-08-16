using UnityEngine;
using UnityEngine.EventSystems;

namespace Ambition
{
    public class ParisView : MonoBehaviour, IPointerClickHandler
    {
        public GameObject ExplorePin;

        void Awake()
        {
            AmbitionApp.Subscribe<string>(ParisMessages.SELECT_LOCATION, HandleSelect);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(ParisMessages.SELECT_LOCATION, HandleSelect);
        }

        void Start()
        {
            bool active;
            LocationPin[] locations = transform.GetComponentsInChildren<LocationPin>(true);
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            foreach (LocationPin pin in locations)
            {
                active = model.Locations.ContainsKey(name)
                || (!pin.Discoverable
                    && !model.VisitedLocations.ContainsKey(pin.name)
                    && AmbitionApp.CheckRequirements(pin.Requirements));
                pin.gameObject.SetActive(active);
                if (active)
                {
                    LocationVO location = new LocationVO
                    {
                        Name = pin.name,
                        ID = pin.GetInstanceID(),
                        Scene = (pin.Scene != null) ? pin.Scene.name : null,
                        OneShot = pin.OneShot,
                        Discoverable = pin.Discoverable,
                        Requirements = pin.Requirements
                    };
                    model.Locations[pin.name] = location;
                }
            }
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (data.pointerPressRaycast.gameObject == gameObject)
            {
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), data.position, data.pressEventCamera, out pos);
                ExplorePin.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                AmbitionApp.SendMessage(ParisMessages.SELECT_LOCATION, "");
            }
        }

        private void HandleSelect(string location)
        {
            ExplorePin.SetActive(location.Length == 0);
        }

        public void Explore()
        {
            
        }
    }
}
