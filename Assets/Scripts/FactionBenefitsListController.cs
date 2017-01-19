using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionBenefitsListController : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = this.GetComponent<Text>();
        DisplayBenefits("Crown");
    }

    public void DisplayBenefits(string factionName)
    {
        text.text = "The " + factionName;
        for (int i = 0; i < GameData.factionList[factionName].PlayerReputationLevel()+1; i++)
        {
            text.text += "\n"+GameData.factionList[factionName].benefitsList[i];
            Debug.Log(factionName + "Level " + i + " Faction Benefits!");
        }
    }
}
