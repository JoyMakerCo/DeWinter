using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Ambition
{
    [RequireComponent(typeof(Button))]
    public class BetterScroll : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public ScrollRect ScrollRect;
        public RectTransform ScrollArea;
        public RectTransform HandleArea;
        public bool Horizontal;
        public bool Vertical;

        Vector3 _mouseDown;

        private void Awake()
        {

        }

        public void OnPointerDown(PointerEventData data)
        {
            _mouseDown = Input.mousePosition;
        }

        public void OnDrag(PointerEventData data)
        {
            Vector3 delta = Input.mousePosition - _mouseDown;
            delta.Scale(new Vector3(.016f, .016f, 0));
            transform.position += delta;
            _mouseDown = Input.mousePosition;
        }

        private void OnEnable()
        {
  //          ScrollRect.onValueChanged.AddListener(UpdateScroll);
        }

        private void OnDisable()
        {
//            ScrollRect.onValueChanged.RemoveListener(UpdateScroll);
        }

        private void UpdateScroll(Vector2 offset)
        {

        }
    }
}
