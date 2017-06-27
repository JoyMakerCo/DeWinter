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
        DisplayBenefits("Crown");
    }

    public void DisplayBenefits(string factionName)
    {
    	FactionModel model = AmbitionApp.GetModel<FactionModel>();
        string str = "The " + factionName + "(Level " + model[factionName].ReputationLevel.ToString() + ")\n";
		str += model[factionName].FactionBenefitsList;
		text.text = str;
    }
}
