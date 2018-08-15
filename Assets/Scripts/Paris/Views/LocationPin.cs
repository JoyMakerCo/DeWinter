using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

namespace Ambition
{
    public class LocationPin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector]
        public string Incident = null;

        public GameObject Scene = null;
        public bool OneShot = false;
        public bool Discoverable = false;
        public GameObject Tooltip;

#if (UNITY_EDITOR)
        public IncidentConfig IncidentConfig;

        private void OnValidate()
        {
            Incident = IncidentConfig?.name;
        }
#endif

        public LocationVO GetLocation()
        {
            return new LocationVO(this);
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
