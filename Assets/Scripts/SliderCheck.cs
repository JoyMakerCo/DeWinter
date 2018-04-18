using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCheck : MonoBehaviour {
    //This script is used to determine if the text is long enough to require a scrollbar/slider


    public GameObject sliderObject;

    public RectTransform scrollRectTransform;
    public RectTransform contentRectTransform;

    public void OnRectTransformDimensionsChange()
    {
        SliderNeededCheck();
    }

    private void SliderNeededCheck() //Do we need a slider at all?
    {
        if (scrollRectTransform.rect.height >= contentRectTransform.rect.height)
        {
            sliderObject.SetActive(false);
        }
        else
        {
            sliderObject.SetActive(true);
        }
    }
}
