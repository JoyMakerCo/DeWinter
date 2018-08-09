using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ambition
{
    public class LocationPin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public IncidentConfig Incident = null;
        public GameObject Scene = null;
        public bool OneShot = false;
        public bool Discoverable = false;
        public GameObject Tooltip;

        public LocationVO Location
        {
            get
            {
                return new LocationVO(this);
            }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            Tooltip.SetActive(true);
        }

        public void OnPointerExit(PointerEventData data)
        {
            Tooltip.SetActive(false);
        }
    }
}
