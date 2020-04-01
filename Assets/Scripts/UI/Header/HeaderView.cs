using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class HeaderView : MonoBehaviour
    {
        public Text HeaderTitle;

        void Awake()
        {
            AmbitionApp.Subscribe<string>(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Subscribe(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Subscribe(GameMessages.HIDE_HEADER, HideHeader);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Unsubscribe(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Unsubscribe(GameMessages.HIDE_HEADER, HideHeader);
        }

        private void ShowHeader() => gameObject.SetActive(true);
        private void HideHeader() => gameObject.SetActive(false);

        private void ShowHeader(string title)
        {
            ShowHeader();
            HeaderTitle.text = title;
        }
    }
}
