using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class TextWrapHaxx : MonoBehaviour
    {
        public Text OverflowTxt;
        public Text WrapTxt;

        private void OnEnable()
        {
            StartCoroutine(Reflow());
        }

        IEnumerator Reflow()
        {
            yield return new WaitForEndOfFrame();
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            if (((RectTransform)OverflowTxt.transform).sizeDelta.x > ((RectTransform)WrapTxt.transform).sizeDelta.x)
            {
                WrapTxt.text = OverflowTxt.text;
                OverflowTxt.gameObject.SetActive(false);
                WrapTxt.gameObject.SetActive(true);
            }
        }
    }
}
