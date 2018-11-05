using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabButtonController : MonoBehaviour {

    public bool Interactable;

    public Sprite ButtonNormalState;
    public Sprite ButtonHoverState;
    public Sprite ButtonClickedState;
    public Sprite ButtonDisabledState;

    public Image ButtonImage; 
    public Text ButtonText;

    public Color DarkTextColor;
    public Color LightTextColor;

	public void MouseHover()
    {
        if (Interactable)
        {
            ButtonImage.sprite = ButtonHoverState;
            ButtonText.color = LightTextColor;
        } else
        {
            ButtonImage.sprite = ButtonDisabledState;
        }
    }

    public void MouseLeave()
    {
        if (Interactable)
        {
            ButtonImage.sprite = ButtonNormalState;
            ButtonText.color = DarkTextColor;
        } else
        {
            ButtonImage.sprite = ButtonDisabledState;
        }
    }

    public void MouseClick()
    {
        if (Interactable) ButtonImage.sprite = ButtonClickedState;
        else ButtonImage.sprite = ButtonDisabledState;
    }

    public void setInteractable(bool set)
    {
        Interactable = set;
    }
}
