using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PartyTracker : MonoBehaviour {
    private Text myText;
    public Party nextParty;

    void Start()
    {
        myText = GetComponent<Text>();
    }

    void Update()
    {
        updateParty();
    }

    //TODO - Account for Monthly Rollovers. This can't necessarily see into the next month
    //This function controls the text that tells players what the next party is in the Wardrobe screen. Not a necessity but very helpful
    public void updateParty()
    {
        Party nextParty = null;
        int i = 0;
        while (nextParty == null)
        {
            if(GameData.calendar.daysFromNow(i).party1.faction != null && GameData.calendar.daysFromNow(i).party1.invited)
            {
                nextParty = GameData.calendar.daysFromNow(i).party1;
            } else
            {
                i++;
            }        
        }
        if (i == 0)
        {
            myText.text = nextParty.SizeString() + " " + nextParty.faction + " Party (Today)";
        } else if (i == 1)
        {
            myText.text = nextParty.SizeString() + " " + nextParty.faction + " Party (Tomorrow)";
        } else if (i == 2)
        {
            myText.text = nextParty.SizeString() + " " + nextParty.faction + " Party (The Day After Tomorrow)";
        } else
        {
            myText.text = nextParty.SizeString() + " " + nextParty.faction + " Party (" + i + " Days from Now)";
        }
    }
}
