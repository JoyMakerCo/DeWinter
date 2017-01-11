using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdvanceTimeButtonTextController : MonoBehaviour {

    private Text myText;
    private Outline myOutline; // This is for highlighting buttons

    void Start ()
    {
        myText = this.GetComponentInChildren<Text>();
        myOutline = this.GetComponent<Outline>();
        GameData.partyOutfitID = -1;
    }

    void Update ()
    {
        if (GameData.tonightsParty.faction == null || GameData.tonightsParty.RSVP == -1 || GameData.tonightsParty.RSVP == 0)
        {
            myOutline.effectColor = Color.clear;
            myText.text = "Next Day";
        } else
        {
            myOutline.effectColor = Color.yellow;
            myText.text = "Go to the Party!";
        }
	}
}
