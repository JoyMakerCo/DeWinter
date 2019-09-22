using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class TitleListener : MonoBehaviour
    {
        public Text TitleText;

        private void Awake() => AmbitionApp.Subscribe<string>(GameMessages.SET_TITLE, HandleTitle);
        private void OnDestroy() => AmbitionApp.Unsubscribe<string>(GameMessages.SET_TITLE, HandleTitle);

        private void HandleTitle(string title) => TitleText.text = title;
	}
}
