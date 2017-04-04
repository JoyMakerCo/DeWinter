using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdvanceTimeButtonTextController : MonoBehaviour {

    private Text myText;

    void Start ()
    {
        myText = this.GetComponentInChildren<Text>();
        OutfitInventory.PartyOutfit = null;
    }

    void Update ()
    {
		if (GameData.tonightsParty == null || GameData.tonightsParty.faction == null || GameData.tonightsParty.RSVP != 1)
        {
            myText.text = "Next Day";
        }
        else
        {
            myText.text = "Go to the Party!";
        }    
	}
}