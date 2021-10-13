using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class ForceMatchContent : MonoBehaviour
    {
        public RectTransform Selector;
        public RectTransform Content;
        private void OnEnable()
        {
            StartCoroutine(WaitForFrame());
        }

        IEnumerator WaitForFrame()
        {
            Vector2 sizeDelta = Content.sizeDelta;
            yield return new WaitForEndOfFrame();
            (this.transform as RectTransform).sizeDelta = Content.sizeDelta - sizeDelta;
        }
    }
}
