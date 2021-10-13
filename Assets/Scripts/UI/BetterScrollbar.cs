using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Ambition
{
    public class BetterScrollbar : MonoBehaviour, IDragHandler
    {
        public RectTransform ScrollBar;
        public Image Fill;
        public bool IsVertical;
        public bool ReverseDirection;
        
        public float Min, Max;

        private float _value;
        private RectTransform _handleRect;
        
        public float Value
        {
            get => _value;
            set
            {
                if (value > Max) _value = Max;
                else if (value < Min) _value = Min;
                else _value = value;

            }
        }

        public void OnDrag(PointerEventData data)
        {
            Vector3 position = _handleRect.position;
            Rect rect = _handleRect.rect;
            if (IsVertical)
            {
                position.y = data.pointerCurrentRaycast.worldPosition.y;
                if (ScrollBar != null)
                {
                    rect.position = position;
                    if (rect.yMin < ScrollBar.rect.yMin)
                        rect.yMin = ScrollBar.rect.yMin;
                    else if (rect.yMax > ScrollBar.rect.yMax)
                        rect.yMax = ScrollBar.rect.yMax;
                    position = rect.position;
                }
            }
            else
            {
                position.x = data.pointerCurrentRaycast.worldPosition.x;
                if (ScrollBar != null)
                {
                    rect.position = position;
                    if (rect.xMin < ScrollBar.rect.xMin)
                        rect.xMin = ScrollBar.rect.xMin;
                    else if (rect.xMax > ScrollBar.rect.xMax)
                        rect.xMax = ScrollBar.rect.xMax;
                    position = rect.position;
                }
            }
            _handleRect.position = position;
        }

        private void OnEnable() =>  _handleRect = GetComponent<RectTransform>();
    }
}
