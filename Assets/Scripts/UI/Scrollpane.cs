using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace Ambition
{
    public class Scrollpane : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IScrollHandler
    {
        public RectTransform Content;
        public RectTransform Viewport;
        public Slider Scroll;
        public bool HideScrollbars = true;
        public int ScrollRate;
        public int Overshoot;
        public float SpringbackTime = .5f;

        public void OnBeginDrag(PointerEventData data)
        {

        }

        public void OnEndDrag(PointerEventData data)
        {

        }

        public void OnScroll(PointerEventData data)
        {
            Vector2 delta = data.scrollDelta;

        }

        private void SetScroll(float x, float y)
        {

        }

        IEnumerator Springback()
        {
            yield return null;
        }
    }
}
