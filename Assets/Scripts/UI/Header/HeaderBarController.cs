using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public abstract class HeaderBarController : MonoBehaviour
    {
        private const float _T = .5f;
        private const float _1T = 2f;

        public Image FillImage;
        public Text StatText;
        public string LocalizationKey;

        void OnEnable()
        {
            int initialStat = GetInitialStat();
            FillImage.fillAmount = .01f*(float)initialStat;
            AmbitionApp.Subscribe<int>(GetMessageID(), HandleUpdate);
            UpdateText(initialStat);
        }
        void OnDisable() => AmbitionApp.Unsubscribe<int>(GetMessageID(), HandleUpdate);
        void HandleUpdate(int value)
        {
            UpdateText(value);
            StopAllCoroutines();
            StartCoroutine(AdjustValue(value));
        }
        protected abstract string GetMessageID();
        protected abstract int GetInitialStat();

        private void UpdateText(int value)
        {
            StatText.text = (value == default ? "0" : value.ToString("### ### ###")) + " " + AmbitionApp.Localize(LocalizationKey);
        }

        IEnumerator AdjustValue(int value)
        {
            float start = FillImage.fillAmount;
            float df = value * .01f - start;
            for (float t = 0; t < _T; t += Time.deltaTime)
            {
                FillImage.fillAmount = start + df - df*(1f - t * _1T)*(1f - t * _1T);
                yield return null;
            }
            FillImage.fillAmount = value * .01f;
        }
    }
}
