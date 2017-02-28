using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DeWinter;

public class FactionInfoTextController : MonoBehaviour {

    public Text crownInfo;
    public Text churchInfo;
    public Text militaryInfo;
    public Text bourgeoisieInfo;
    public Text revolutionInfo;

    public int testTheWatersCost;

    public bool availableSpymasterTestTheWaters;
    public bool availableTestTheWaters;

    private Dictionary<string, int> _allegianceTimers;
	private Dictionary<string, int> _powerTimers;

	// Use this for initialization
	void Start () {
		ServantModel smod = DeWinterApp.GetModel<ServantModel>();
		availableSpymasterTestTheWaters = (smod.Hired.ContainsKey("Spymaster"));
        availableTestTheWaters = true;

        _allegianceTimers = new Dictionary<string, int>();
		_powerTimers = new Dictionary<string, int>();
		foreach(string faction in DeWinterApp.GetModel<FactionModel>().Factions.Keys)
		{
			_powerTimers.Add(faction, 0);
			_allegianceTimers.Add(faction, 0);
		}

        IncrementKnowledgeTimers();
        UpdateInfo();
	}

    void UpdateInfo()
    {
        //---- Crown Info ----
        if (GameData.factionList["Crown"].ReputationLevel >= 8)
        {
            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
                        "\n- Like Expensive but Modest Clothes" +
				"\n- Power: " + GetPowerString(GameData.factionList["Crown"].Power) + " (Faction Benefit)";
        } else if (GameData.factionList["Crown"].ReputationLevel >= 6)
        {
            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
                        "\n- Like Expensive but Modest Clothes" +
				"\n- Power: " + GetPowerString(GameData.factionList["Crown"].Power) + " (Faction Benefit)";
        } else
        {
            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
                        "\n- Like Expensive but Modest Clothes" +
                        "\n- Power: " + GameData.factionList["Crown"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Crown"]);
        }
        //---- Church Info ----
        if (GameData.factionList["Church"].ReputationLevel >= 8)
        {
            churchInfo.text = "- The Clergy and those alligned with them" +
                        "\n- Like Vintage and Modest Clothes" +
				"\n- Allegiance: " + GetAllegianceString(GameData.factionList["Church"].Allegiance) + " (Faction Benefit)" +
				"\n- Power: " + GetPowerString(GameData.factionList["Church"].Power) + " (Faction Benefit)";
        }
        else if (GameData.factionList["Church"].ReputationLevel >= 6)
        {
            churchInfo.text = "- The Clergy and those alligned with them" +
                         "\n- Like Vintage and Modest Clothes" +
                         "\n- Allegiance: " + GameData.factionList["Church"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Church"]) +
				"\n- Power: " + GetPowerString(GameData.factionList["Church"].Power) + " (Faction Benefit)";
        }
        else
        {
            churchInfo.text = "- The Clergy and those alligned with them" +
                        "\n- Like Vintage and Modest Clothes" +
                        "\n- Allegiance: " + GameData.factionList["Church"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Church"]) +
                        "\n- Power: " + GameData.factionList["Church"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Church"]);
        }
        //---- Military Info ----
        if (GameData.factionList["Military"].ReputationLevel >= 8)
        {
            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
                        "\n- Couldn't care less about your clothes" +
				"\n- Allegiance: " + GetAllegianceString(GameData.factionList["Military"].Allegiance) + " (Faction Benefit)" +
				"\n- Power: " + GetPowerString(GameData.factionList["Military"].Power) + " (Faction Benefit)";
        }
        else if (GameData.factionList["Military"].ReputationLevel >= 6)
        {
            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
                        "\n- Couldn't care less about your clothes" +
                        "\n- Allegiance: " + GameData.factionList["Military"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Military"]) +
				"\n- Power: " + GetPowerString(GameData.factionList["Military"].Power) + " (Faction Benefit)";
        }
        else
        {
            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
                        "\n- Couldn't care less about your clothes" +
                        "\n- Allegiance: " + GameData.factionList["Military"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Military"]) +
                        "\n- Power: " + GameData.factionList["Military"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Military"]);
        }
        //---- Bourgeoisie Info ----
        if (GameData.factionList["Bourgeoisie"].ReputationLevel >= 8)
        {
            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
                        "\n- Like clothes that are Luxurious and Racy" +
				"\n- Allegiance: " + GetAllegianceString(GameData.factionList["Bourgeoisie"].Allegiance) + " (Faction Benefit)" +
				"\n- Power: " + GetPowerString(GameData.factionList["Bourgeoisie"].Power) + " (Faction Benefit)";
        }
        else if (GameData.factionList["Bourgeoisie"].ReputationLevel >= 6)
        {
            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
                        "\n- Like clothes that are Luxurious and Racy" +
                        "\n- Allegiance: " + GameData.factionList["Bourgeoisie"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Bourgeoisie"]) +
                        "\n- Power: " + GetPowerString(GameData.factionList["Bourgeoisie"].Power) + " (Faction Benefit)";
        }
        else
        {
            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
                        "\n- Like clothes that are Luxurious and Racy" +
                        "\n- Allegiance: " + GameData.factionList["Bourgeoisie"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Bourgeoisie"]) +
                        "\n- Power: " + GameData.factionList["Bourgeoisie"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Bourgeoisie"]);
        }
        //---- Revolution Info ----
        if (GameData.factionList["Revolution"].ReputationLevel >= 8)
        {
            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
                        "\n - Like clothes that are Vintage and Racy" +
				"\n- Power: " + GetPowerString(GameData.factionList["Revolution"].Power) + " (Faction Benefit)";
        }
        else if (GameData.factionList["Revolution"].ReputationLevel >= 6)
        {
            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
                        "\n - Like clothes that are Vintage and Racy" +
							"\n- Power: " + GetPowerString(GameData.factionList["Revolution"].Power) + " (Faction Benefit)";
        }
        else
        {
            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
                        "\n - Like clothes that are Vintage and Racy" +
                        "\n- Power: " + GameData.factionList["Revolution"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Revolution"]);
        }
    }

    public void TestTheWatersPower(string faction)
    {
        if (GameData.factionList[faction].ReputationLevel >= 6)
        {
            Debug.Log("Can't Test the Waters for Power, you already know this info due to Faction Reputation Level");
        } else {
            if (availableSpymasterTestTheWaters)
            {
                availableSpymasterTestTheWaters = false;
				_powerTimers[faction] = 0;
				UpdateInfo();
            }
            else if (availableTestTheWaters && GameData.moneyCount > testTheWatersCost)
            {
                availableTestTheWaters = false;
                GameData.moneyCount -= testTheWatersCost;
				_powerTimers[faction] = 0;
                UpdateInfo();
            }
            else
            {
                Debug.Log("You've already used up your charges to Test the Waters for today and/or you don't have enough money :(");
            }
        } 
    }

    public void TestTheWatersAllegiance(string faction)
    {
        if (GameData.factionList[faction].ReputationLevel >= 8)
        {
            Debug.Log("Can't Test the Waters for Allegiance, you already know this info due to Faction Reputation Level");
        }
        else
        {
            if (availableSpymasterTestTheWaters)
            {
                availableSpymasterTestTheWaters = false;
				_powerTimers[faction] = 0;
                UpdateInfo();
            }
            else if (availableTestTheWaters && GameData.moneyCount > testTheWatersCost)
            {
                availableTestTheWaters = false;
                GameData.moneyCount -= testTheWatersCost;
                _powerTimers[faction] = 0;
                UpdateInfo();
            }
            else
            {
                Debug.Log("You've already used up your charges to Test the Waters for today and/or you don't have enough money :(");
            }
        }
    }

    void IncrementKnowledgeTimers()
    {

        //Allegiance Timers for Crown and Revolution aren't used
        foreach (string faction in _allegianceTimers.Keys)
        {
        	_allegianceTimers[faction]++;
			_powerTimers[faction]++;
        }
    }

// TODO: Store them in order
    private string GetPowerString(int power)
    {
    	FactionModel fmod = DeWinterApp.GetModel<FactionModel>();
    	int [] values = new int[fmod.Power.Count];
    	fmod.Power.Keys.CopyTo(values, 0);
    	Array.Sort(values);
    	for (int i=values.Length-1; i>=0; i--)
    	{
    		if (power > values[i])
    			return fmod.Power[values[i]];
    	}
		return fmod.Power[0];
    }

// TODO: Store them in order
	private string GetAllegianceString(int allegiance)
    {
    	FactionModel fmod = DeWinterApp.GetModel<FactionModel>();
    	int [] values = new int[fmod.Allegiance.Count];
		fmod.Allegiance.Keys.CopyTo(values, 0);
    	Array.Sort(values);
    	for (int i=values.Length-1; i>=0; i--)
    	{
			if (allegiance > values[i])
				return fmod.Allegiance[values[i]];
    	}
		return fmod.Allegiance[0];
    }

    string ConvertKnowledgeTimer(int timer)
    {
        switch (timer) {
            case 0:
                return "(As of Today)";
            case 1:
                return "(As of Yesterday)";
            default:
                return "(As of " + timer + " Days Ago)";
        }     
    }
}
