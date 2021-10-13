using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
    public class PrefabButtonTextController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Text ButtonLabel;
        public Button Button;
        public Color ActiveColor;

        private Color _passiveColor;

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            _passiveColor = ButtonLabel.color;
            if (Button?.interactable ?? false)
                ButtonLabel.color = ActiveColor;
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            ButtonLabel.color = _passiveColor;
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Image img = Button.gameObject.GetComponent<Image>();
                if (img != null) img.sprite = Button.image.sprite;
            }
        }
    }
}
