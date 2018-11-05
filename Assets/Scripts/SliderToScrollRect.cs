using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderToScrollRect : MonoBehaviour
    {
        //This script exists because our UI scrolling objects behave more like a slider than a Scrollbar object (the handle in our version doesn't resize based on amount of items in the list)
        //Thus, this conversion script is necessary in the 'On Value Changed' settings of the Slider script, so the two types play nice without us having to make a whole new kind of ScrollRect
        public Slider slider;
        public ScrollRect scrollRect;

        void Awake()
        {
            ChangeScrollPos();
        }

        public void ChangeScrollPos()
        {
            scrollRect.verticalNormalizedPosition = 1-slider.value;
        }
    }
