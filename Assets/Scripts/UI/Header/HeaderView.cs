using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Ambition
{
    public class HeaderView : MonoBehaviour
    {
        public GameObject Header;

        public void Start()
        {
            AmbitionApp.Subscribe(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Subscribe<string>(GameMessages.SHOW_HEADER, SetHeaderTitle);
            AmbitionApp.Subscribe(GameMessages.HIDE_HEADER, HideHeader);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Unsubscribe(GameMessages.HIDE_HEADER, HideHeader);
            AmbitionApp.Unsubscribe<string>(GameMessages.SHOW_HEADER, SetHeaderTitle);
        }

        private void ShowHeader() => Header.SetActive(true);
        private void SetHeaderTitle(string title) => Header.SetActive(true);
        private void HideHeader()
        {
            Header.SetActive(false);
        }
    }
}
