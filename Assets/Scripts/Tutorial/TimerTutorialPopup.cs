using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class TimerTutorialPopup : MonoBehaviour
    {
        public int Countdown=5;
        public GameObject Popup;

        private void OnEnable()
        {
            StartCoroutine(OnCountdown());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            Popup.SetActive(false);
        }

        IEnumerator OnCountdown()
        {
            for (float t=(float)Countdown; t>=0; t-=Time.deltaTime)
            {
                yield return null;
            }
            Popup.SetActive(true);
        }
    }
}
