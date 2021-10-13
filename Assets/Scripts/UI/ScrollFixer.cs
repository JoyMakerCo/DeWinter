using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class ScrollFixer : MonoBehaviour
    {
        private Vector3 _scale;

        private void OnEnable()
        {
            _scale = gameObject.GetComponent<RectTransform>().localScale;
        }

        public void OnUpdate()
        {
            gameObject.GetComponent<RectTransform>().localScale = _scale;
        }
    }
}
