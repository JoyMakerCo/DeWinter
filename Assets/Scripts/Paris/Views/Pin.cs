using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

namespace Ambition
{
    public class Pin : MonoBehaviour
    {
        public LocationConfig Config;
        public GameObject Label;
        public Text LabelText;

        public RequirementVO[] Requirements => Config?.Requirements;
        public bool IsDiscoverable => Config?.IsDiscoverable ?? false;
        public bool IsRendezvous => Config?.IsRendezvous ?? false;

        public void HandleClick()
        {
            AmbitionApp.SendMessage(ParisMessages.SELECT_LOCATION, Config);
        }

        public void ShowLabel(bool active)
        {
            LabelText.text = AmbitionApp.Localize(ParisConsts.LABEL + Config.name);
            Label.SetActive(active);
        }
    }
}
