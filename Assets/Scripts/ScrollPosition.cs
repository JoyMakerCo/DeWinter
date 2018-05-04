using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPosition : MonoBehaviour
{
    //This script is here to make a slider provide inputs like a scroll bar. Scroll bars naturally expand their handles to fit their content, which our UI doesn't do.
    //However, scroll rects don't play nice with sliders. Hence this strange little script.

    public GameObject sliderObject;
    public Slider slider;
    public ScrollRect scrollRect;

    public void ChangeScrollPos()
    {
        scrollRect.verticalNormalizedPosition = slider.value;
    }

    public void ChangeSliderPos()
    {
        slider.value = scrollRect.verticalNormalizedPosition;
    }
}
