using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

namespace Ambition
{
    public class RestAtHomeView : MonoBehaviour, IPointerClickHandler
    {
        public GameObject Tooltip;

        private void Awake()
        {
            AmbitionApp.Subscribe<string>(ParisMessages.SELECT_LOCATION, HandleSelect);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(ParisMessages.SELECT_LOCATION, HandleSelect);
        }

        public void OnPointerClick(PointerEventData data)
        {
            Tooltip.SetActive(true);
        }

        void HandleSelect(string location)
        {
            Tooltip.SetActive(false);
        }
    }
}
