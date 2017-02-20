using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeWinter;

public class FactionBenefitsListController : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = this.GetComponent<Text>();
        DisplayBenefits("Crown");
    }

    public void DisplayBenefits(string factionName)
    {
		int repLevel = GameData.factionList[factionName].ReputationLevel;
        text.text = "The " + factionName + "(Level " + repLevel.ToString() + ")";
		if(repLevel != -1)
        {
			for (int i = 0; i < repLevel; i++)
            {
				text.text += "\n" + GameData.factionList[factionName].FactionBenefits(i);
                Debug.Log(factionName + "Level " + i.ToString() + " Faction Benefits!");
            }
        } else
        {
            text.text += "\nLevel -1: Persona Non Grata at all " + factionName + " Parties";
        }        
    }
}
