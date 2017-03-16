using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DeWinter;

public class LevelManager : MonoBehaviour
{
    public SceneFadeInOut screenFader;

//TODO: Half of this already exists in StartPartyCmd. Commandify the rest.
    public void AdvanceTimeLoadLevel()
    {
        //If there's no Party tonight
		if (GameData.tonightsParty == null || GameData.tonightsParty.faction == null || GameData.tonightsParty.RSVP < 1)  // If there's no party tonight OR if it's been negatively RSVP'd to
        {
        	DeWinterApp.SendMessage(CalendarMessages.ADVANCE_DAY);
	        DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_Estate");
        } else // If there is a Party tonight
        {
            if(GameData.tonightsParty.turnsLeft != 0) // Has the party happened yet?
            {
                if (OutfitInventory.personalInventory.Count > 0)
                //If the Player actually has an Outfit to attend the Party with
                {
                    DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_PartyLoadOut");
                }
                else
                {
                    //You ain't got no clothes to attend the party! 
                    screenFader.gameObject.SendMessage("CreateNoOutfitModal");
                }
            } else
            {
				DeWinterApp.SendMessage(CalendarMessages.ADVANCE_DAY);
                DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_Estate");
            }
        }
    }
}