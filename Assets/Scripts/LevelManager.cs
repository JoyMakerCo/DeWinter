using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public SceneFadeInOut screenFader;
	public string victoriousPower;
    public string victoryDegree;
    public string playerVictoryStatus;
    public string playerAllegiance;

    public void LoadLevel(string sceneName)
    {
        Debug.Log ("New Level load: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void NextDayEventChance(string sceneName)
    {
        int randomRangeMax = 100;
        if (Random.Range(1, randomRangeMax+1) > GameData.eventChance) //Random.Range with ints is NOT maximally inclusive
        {
            LoadLevel(sceneName);
        }
        else
        {
            LoadLevel("Game_Event");
        }
    }

    public void AdvanceTimeLoadLevel() 
    {
        //If there's no Party tonight
        if (GameData.tonightsParty.faction == null || GameData.tonightsParty.RSVP == -1 || GameData.tonightsParty.RSVP == 0)  // If there's no party tonight OR if it's been negatively RSVP'd to
        {
            AdvanceTime();
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
                AdvanceTime();
                LoadLevel("Game_Estate");
            }
           
        }
    }

    public void StartPartyLevel()
    {
        if (GameData.partyOutfitID != -1)
        {
            LoadLevel("Game_Party");
        } else
        {
            Debug.Log("No Outfit selected :(");
        }
    }

    //TODO - Count for rolling over years (Is this necessary?)
    void AdvanceTime()
    {
        GameData.currentDay += 1;
        if(GameData.currentDay > GameData.calendar.monthList[GameData.currentMonth].days)
        {
            GameData.currentDay = 0;
            GameData.currentMonth += 1;
        }
       if (GameData.currentMonth == GameData.uprisingMonth && GameData.currentDay == GameData.uprisingDay)
        {
            CalculateUprising();
        }
    }

    void CalculateUprising()
    {
        //Establish each Faction's final Power
        float crownFinalPower = GameData.factionList["Crown"].Power() * 100;
        float revolutionFinalPower = GameData.factionList["Revolution"].Power() * 100;
        if(GameData.factionList["Church"].Allegiance() > 0)
        {
            crownFinalPower += (Mathf.Abs((float)(GameData.factionList["Church"].Allegiance() / 2)) * GameData.factionList["Church"].Power());
        } else if (GameData.factionList["Church"].Allegiance() < 0)
        {
            revolutionFinalPower += (Mathf.Abs((float)(GameData.factionList["Church"].Allegiance() / 2)) * GameData.factionList["Church"].Power());
        }
        if (GameData.factionList["Military"].Allegiance() > 0)
        {
            crownFinalPower += (Mathf.Abs((float)(GameData.factionList["Military"].Allegiance() / 2)) * GameData.factionList["Military"].Power());
        } else if (GameData.factionList["Military"].Allegiance() < 0)
        {
            revolutionFinalPower += (Mathf.Abs((float)(GameData.factionList["Military"].Allegiance() / 2)) * GameData.factionList["Military"].Power());
        }
        if (GameData.factionList["Bourgeoisie"].Allegiance() > 0)
        {
            crownFinalPower += (Mathf.Abs((float)(GameData.factionList["Bourgeoisie"].Allegiance() / 2)) * GameData.factionList["Bourgeoisie"].Power());
        }
        else if (GameData.factionList["Bourgeoisie"].Allegiance() < 0)
        {
            revolutionFinalPower += (Mathf.Abs((float)(GameData.factionList["Bourgeoisie"].Allegiance() / 2)) * GameData.factionList["Bourgeoisie"].Power());
        }


        //Compare Final Powers (who won and by what degree?)
        if (crownFinalPower > revolutionFinalPower)
        {
            GameData.victoriousPower = "Crown";
            if (crownFinalPower - revolutionFinalPower > 50)
            {
                GameData.victoryDegree = "Decisive";
            } else
            {
                GameData.victoryDegree = "Partial";
            }
        } else
        {
            GameData.victoriousPower = "Revolution";
            if (revolutionFinalPower - crownFinalPower > 50)
            {
                GameData.victoryDegree = "Decisive";
            }
            else
            {
                GameData.victoryDegree = "Partial";
            }
        }

        //Calculate Player Allegiance
        if(GameData.factionList["Crown"].playerReputation > GameData.factionList["Revolution"].playerReputation)
        {
            GameData.playerAllegiance = "Crown";
        } else if (GameData.factionList["Revolution"].playerReputation > GameData.factionList["Crown"].playerReputation)
        {
            GameData.playerAllegiance = "Revolution";
        } else // If it's equal then you get shuffled onto the losing team of History
        {
            GameData.playerAllegiance = "Unknown";
        }

        //Go to the End Screen
        if(GameData.playerAllegiance == GameData.victoriousPower)
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
