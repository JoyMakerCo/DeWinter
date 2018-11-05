using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Core;

namespace Ambition
{
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
		void Start ()
		{
			FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
			ServantModel servants = AmbitionApp.GetModel<ServantModel>();
			ServantVO spymaster;

			availableSpymasterTestTheWaters = servants.Servants.TryGetValue(ServantConsts.SPYMASTER, out spymaster);
	        availableTestTheWaters = true;

	        _allegianceTimers = new Dictionary<string, int>();
			_powerTimers = new Dictionary<string, int>();
			foreach(string faction in fmod.Factions.Keys)
			{
				_powerTimers.Add(faction, 0);
				_allegianceTimers.Add(faction, 0);
			}

	        IncrementKnowledgeTimers();
	        UpdateInfo();
		}

	    void UpdateInfo()
	    {
			FactionModel model = AmbitionApp.GetModel<FactionModel>();

	        //---- Crown Info ----
	        if (model["Crown"].Level >= 8)
	        {
	            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
	                        "\n- Like Expensive but Modest Clothes" +
					"\n- Power: " + GetPowerString(model["Crown"].Power) + " (Faction Benefit)";
	        } else if (model["Crown"].Level >= 6)
	        {
	            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
	                        "\n- Like Expensive but Modest Clothes" +
					"\n- Power: " + GetPowerString(model["Crown"].Power) + " (Faction Benefit)";
	        } else
	        {
	            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
	                        "\n- Like Expensive but Modest Clothes" +
	                        "\n- Power: " + model["Crown"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Crown"]);
	        }
	        //---- Church Info ----
	        if (model["Church"].Level >= 8)
	        {
	            churchInfo.text = "- The Clergy and those alligned with them" +
	                        "\n- Like Vintage and Modest Clothes" +
					"\n- Allegiance: " + GetAllegianceString(model["Church"].Allegiance) + " (Faction Benefit)" +
					"\n- Power: " + GetPowerString(model["Church"].Power) + " (Faction Benefit)";
	        }
	        else if (model["Church"].Level >= 6)
	        {
	            churchInfo.text = "- The Clergy and those alligned with them" +
	                         "\n- Like Vintage and Modest Clothes" +
	                         "\n- Allegiance: " + model["Church"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Church"]) +
					"\n- Power: " + GetPowerString(model["Church"].Power) + " (Faction Benefit)";
	        }
	        else
	        {
	            churchInfo.text = "- The Clergy and those alligned with them" +
	                        "\n- Like Vintage and Modest Clothes" +
	                        "\n- Allegiance: " + model["Church"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Church"]) +
	                        "\n- Power: " + model["Church"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Church"]);
	        }
	        //---- Military Info ----
	        if (model["Military"].Level >= 8)
	        {
	            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
	                        "\n- Couldn't care less about your clothes" +
					"\n- Allegiance: " + GetAllegianceString(model["Military"].Allegiance) + " (Faction Benefit)" +
					"\n- Power: " + GetPowerString(model["Military"].Power) + " (Faction Benefit)";
	        }
	        else if (model["Military"].Level >= 6)
	        {
	            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
	                        "\n- Couldn't care less about your clothes" +
	                        "\n- Allegiance: " + model["Military"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Military"]) +
					"\n- Power: " + GetPowerString(model["Military"].Power) + " (Faction Benefit)";
	        }
	        else
	        {
	            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
	                        "\n- Couldn't care less about your clothes" +
	                        "\n- Allegiance: " + model["Military"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Military"]) +
	                        "\n- Power: " + model["Military"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Military"]);
	        }
	        //---- Bourgeoisie Info ----
	        if (model["Bourgeoisie"].Level >= 8)
	        {
	            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
	                        "\n- Like clothes that are Luxurious and Racy" +
					"\n- Allegiance: " + GetAllegianceString(model["Bourgeoisie"].Allegiance) + " (Faction Benefit)" +
					"\n- Power: " + GetPowerString(model["Bourgeoisie"].Power) + " (Faction Benefit)";
	        }
	        else if (model["Bourgeoisie"].Level >= 6)
	        {
	            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
	                        "\n- Like clothes that are Luxurious and Racy" +
	                        "\n- Allegiance: " + model["Bourgeoisie"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Bourgeoisie"]) +
	                        "\n- Power: " + GetPowerString(model["Bourgeoisie"].Power) + " (Faction Benefit)";
	        }
	        else
	        {
	            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
	                        "\n- Like clothes that are Luxurious and Racy" +
	                        "\n- Allegiance: " + model["Bourgeoisie"].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers["Bourgeoisie"]) +
	                        "\n- Power: " + model["Bourgeoisie"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Bourgeoisie"]);
	        }
	        //---- Third Estate Info ----
	        if (model["Third Estate"].Level >= 8)
	        {
	            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
	                        "\n - Like clothes that are Vintage and Racy" +
					"\n- Power: " + GetPowerString(model["Third Estate"].Power) + " (Faction Benefit)";
	        }
	        else if (model["Third Estate"].Level >= 6)
	        {
	            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
	                        "\n - Like clothes that are Vintage and Racy" +
								"\n- Power: " + GetPowerString(model["Third Estate"].Power) + " (Faction Benefit)";
	        }
	        else
	        {
	            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
	                        "\n - Like clothes that are Vintage and Racy" +
	                        "\n- Power: " + model["Third Estate"].knownPower + " " + ConvertKnowledgeTimer(_powerTimers["Third Estate"]);
	        }
	    }

	    public void TestTheWatersPower(string faction)
	    {
			if (AmbitionApp.GetModel<FactionModel>()[faction].Level >= 6)
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
			if (AmbitionApp.GetModel<FactionModel>()[faction].Level >= 8)
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
	        //Allegiance Timers for Crown and Third Estate aren't used
			List<string> factions = new List<string>(_allegianceTimers.Keys);
	        foreach (string faction in factions)
	        {
	        	_allegianceTimers[faction]++;
				_powerTimers[faction]++;
			}
	    }

	// TODO: Store them in order
	    private string GetPowerString(int power)
	    {
			return AmbitionApp.GetModel<LocalizationModel>().GetString("power." + power.ToString());
	    }

	// TODO: Store them in order
		private string GetAllegianceString(int allegiance)
	    {
	    	FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
			int index = fmod.Allegiance.Length-1;
			while (index >= 0 && allegiance >= fmod.Allegiance[index])
				index--;
			return AmbitionApp.GetModel<LocalizationModel>().GetString("allegiance." + index.ToString());
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
}
