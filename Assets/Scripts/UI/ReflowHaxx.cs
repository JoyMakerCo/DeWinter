using System;
using System.Collections;
using UnityEngine;

namespace Ambition
{
    public class ReflowHaxx : MonoBehaviour
    {
        private void OnEnable() => StartCoroutine(Reflow());
        IEnumerator Reflow()
        {
            yield return new WaitForEndOfFrame();
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
