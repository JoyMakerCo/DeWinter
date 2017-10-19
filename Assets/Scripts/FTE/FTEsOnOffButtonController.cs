using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FTEsOnOffButtonController : MonoBehaviour {
    public Text buttonText;

	// Use this for initialization
	void Start () {
        GameData.fTEsOn = true;
        buttonText.text = "Gameplay Tips On";
        this.GetComponent<Image>().color = Color.white;
    }

    public void Switch()
    {
        GameData.fTEsOn = !GameData.fTEsOn;
        if (GameData.fTEsOn)
        {
            buttonText.text = "Gameplay Tips On";
            this.GetComponent<Image>().color = Color.white;
        } else
        {
            buttonText.text = "Gameplay Tips Off";
            this.GetComponent<Image>().color = Color.grey;
        }
    }	
}
