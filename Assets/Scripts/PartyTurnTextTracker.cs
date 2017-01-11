using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PartyTurnTextTracker : MonoBehaviour {

    Text myText;

	// Use this for initialization
	void Start () {
        myText = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        myText.text = "Turns: " + GameData.tonightsParty.turnsLeft + "/" + GameData.tonightsParty.turns;
        if (GameData.tonightsParty.turnsLeft > 0)
        {
            myText.color = Color.white;
        } else
        {
            myText.color = Color.red;
        }
	}
}
