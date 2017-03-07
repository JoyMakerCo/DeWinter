using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DeWinter;

public class LevelManager : MonoBehaviour
{
    public SceneFadeInOut screenFader;
    public string playerVictoryStatus;
    public string playerAllegiance;

    public void LoadLevel(string sceneName)
    {
        DeWinterApp.Subscribe(CalendarConsts.UPRISING_DAY, HandleUprisingDay);
        SceneManager.LoadScene(sceneName);
    }

    // TODO: Commandify
    public void NextDayEventChance(string sceneName)
    {
		int chance = DeWinterApp.GetModel<EventModel>().EventChance;
		LoadLevel(Random.Range(0, 100) < chance ? "Game_Event" : sceneName);
    }

    public void AdvanceTimeLoadLevel() 
    {
        //If there's no Party tonight
		if (GameData.tonightsParty == null || GameData.tonightsParty.faction == null || GameData.tonightsParty.RSVP == -1 || GameData.tonightsParty.RSVP == 0)  // If there's no party tonight OR if it's been negatively RSVP'd to
        {
        	DeWinterApp.SendMessage(CalendarConsts.ADVANCE_DAY);
	        LoadLevel("Game_Estate");
        } else // If there is a Party tonight
        {
            if(GameData.tonightsParty.turnsLeft != 0) // Has the party happened yet?
            {
                if (OutfitInventory.personalInventory.Count > 0)
                //If the Player actually has an Outfit to attend the Party with
                {
                    LoadLevel("Game_PartyLoadOut");
                }
                else
                {
                    //You ain't got no clothes to attend the party! 
                    screenFader.gameObject.SendMessage("CreateNoOutfitModal");
                }
            } else
            {
				DeWinterApp.SendMessage(CalendarConsts.ADVANCE_DAY);
                LoadLevel("Game_Estate");
            }
           
        }
    }

    public void StartPartyLevel()
    {
        if (OutfitInventory.PartyOutfit != null)
        {
            LoadLevel("Game_Party");
        } else
        {
            Debug.Log("No Outfit selected :(");
        }
    }

	private void HandleUprisingDay()
    {
		string victoriousPower;
//    	bool isDecisive;

        //Establish each Faction's final Power
        float crownFinalPower = GameData.factionList["Crown"].Power * 100;
        float revolutionFinalPower = GameData.factionList["Revolution"].Power * 100;
        if(GameData.factionList["Church"].Allegiance > 0)
        {
            crownFinalPower += (Mathf.Abs((float)(GameData.factionList["Church"].Allegiance / 2)) * GameData.factionList["Church"].Power);
        } else if (GameData.factionList["Church"].Allegiance < 0)
        {
            revolutionFinalPower += (Mathf.Abs((float)(GameData.factionList["Church"].Allegiance / 2)) * GameData.factionList["Church"].Power);
        }
        if (GameData.factionList["Military"].Allegiance > 0)
        {
            crownFinalPower += (Mathf.Abs((float)(GameData.factionList["Military"].Allegiance / 2)) * GameData.factionList["Military"].Power);
        } else if (GameData.factionList["Military"].Allegiance < 0)
        {
            revolutionFinalPower += (Mathf.Abs((float)(GameData.factionList["Military"].Allegiance / 2)) * GameData.factionList["Military"].Power);
        }
        if (GameData.factionList["Bourgeoisie"].Allegiance > 0)
        {
            crownFinalPower += (Mathf.Abs((float)(GameData.factionList["Bourgeoisie"].Allegiance / 2)) * GameData.factionList["Bourgeoisie"].Power);
        }
        else if (GameData.factionList["Bourgeoisie"].Allegiance < 0)
        {
            revolutionFinalPower += (Mathf.Abs((float)(GameData.factionList["Bourgeoisie"].Allegiance / 2)) * GameData.factionList["Bourgeoisie"].Power);
        }


        //Compare Final Powers (who won and by what degree?)
		victoriousPower = crownFinalPower >= revolutionFinalPower ? "Crown" : "Revolution";
//		isDecisive = Mathf.Abs(crownFinalPower - revolutionFinalPower) > 50;
		
        //Calculate Player Allegiance
        if(GameData.factionList["Crown"].playerReputation > GameData.factionList["Revolution"].playerReputation)
        {
			GameData.Allegiance = "Crown";
        } else if (GameData.factionList["Revolution"].playerReputation > GameData.factionList["Crown"].playerReputation)
        {
            GameData.Allegiance = "Revolution";
        } else // If it's equal then you get shuffled onto the losing team of History
        {
			GameData.Allegiance = "Unknown";
        }

        //Go to the End Screen

		if(GameData.Allegiance == victoriousPower)
        {
			GameData.playerVictoryStatus = "Political Victory";
        } else
        {
            GameData.playerVictoryStatus = "Political Loss";
        }
        LoadLevel("Game_EndScreen");
    }

    public void QuitRequest()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }

}
