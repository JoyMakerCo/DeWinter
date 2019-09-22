using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class HeaderView : MonoBehaviour
    {
        void Awake()
        {
            AmbitionApp.Subscribe(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Subscribe(GameMessages.HIDE_HEADER, HideHeader);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Unsubscribe(GameMessages.HIDE_HEADER, HideHeader);
        }

        private void ShowHeader() => gameObject.SetActive(true);
        private void HideHeader() => gameObject.SetActive(false);
    }
}
