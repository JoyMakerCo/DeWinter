using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoozeGlassTextController : MonoBehaviour {

    Text myText;

    // Use this for initialization
    void Start () {
        myText = this.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        myText.text = "Booze Glass: " + GameData.tonightsParty.currentPlayerDrinkAmount + "/" + GameData.tonightsParty.maxPlayerDrinkAmount;
    }
}
