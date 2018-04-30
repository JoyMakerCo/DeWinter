using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class SliderCheck : MonoBehaviour
    {
        //This script is used to determine if the text is long enough to require a scrollbar/slider
        public RectTransform Slider;
        private float _sliderHeight;

        void Awake()
        {
            _sliderHeight = Slider.rect.height;
            Slider.gameObject.SetActive(false);
        }

        public void OnRectTransformDimensionsChange()
        {
            bool showSlider = this.GetComponent<RectTransform>().rect.height > _sliderHeight;
            Slider.gameObject.SetActive(showSlider);
        }
    }
}
