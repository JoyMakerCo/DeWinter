using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambition
{
    public class ParisView : MonoBehaviour, IPointerClickHandler
    {
        public int ExploreRange = 20;
        public GameObject ExplorePin;
        public Transform Pins;

        void Awake()
        {
            AmbitionApp.Subscribe<LocationPin>(ParisMessages.SELECT_LOCATION, HandleSelect);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<LocationPin>(ParisMessages.SELECT_LOCATION, HandleSelect);
        }

        void Start()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            LocationPin pin;
            bool active;
            foreach (Transform child in Pins)
            {
                active = model.Locations.Contains(child.name);
                child.gameObject.SetActive(active);
                if (!active && !model.Visited.Contains(child.name))
                {
                    pin = child.GetComponent<LocationPin>();
                    if (!pin.Discoverable && AmbitionApp.CheckRequirements(pin.Requirements))
                    {
                        AmbitionApp.SendMessage(ParisMessages.ADD_LOCATION, pin.name);
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (data.pointerPressRaycast.gameObject == gameObject)
            {
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), data.position, data.pressEventCamera, out pos);
                ExplorePin.SetActive(true);
                ExplorePin.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                AmbitionApp.SendMessage(ParisMessages.SELECT_LOCATION, "");
            }
        }

        //To Do: Add in the fader (Still needs to be made)
        private void HandleSelect(LocationPin location)
        {
            ExplorePin.SetActive(false);
            AmbitionApp.OpenDialog("PARIS_LOCATION", location);
        }

        public void Explore()
        {
            float range = ExploreRange * ExploreRange;
            Vector3 pos = ExplorePin.transform.localPosition;
            LocationPin[] pins = GetComponentsInChildren<LocationPin>(true);
            Dictionary<LocationPin, float> mags =
                pins.Where(p=>!p.isActiveAndEnabled && p.Discoverable)
                    .ToDictionary(p => p, p => (p.transform.localPosition - pos).sqrMagnitude);
            string[] result =
                mags.Where(p => p.Value <= range)
                    .OrderBy(p=>p.Value)
                    .Select(p => p.Key.name).ToArray();
            AmbitionApp.SendMessage(ParisMessages.EXPLORE, result);
        }
    }
}
