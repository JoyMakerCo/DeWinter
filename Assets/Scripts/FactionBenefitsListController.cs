using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ambition;

public class FactionBenefitsListController : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = this.GetComponent<Text>();
        DisplayBenefits(FactionType.Crown);
    }

    public void DisplayBenefits(FactionType faction)
    {
    	FactionModel model = AmbitionApp.GetModel<FactionModel>();
        string str = "The " + faction.ToString() + "(Level " + model[faction].Level.ToString() + ")\n";
		str += model.GetFactionBenefits(faction);
		text.text = str;
    }
}
