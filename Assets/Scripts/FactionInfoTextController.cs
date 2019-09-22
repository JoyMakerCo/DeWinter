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

	    private Dictionary<FactionType, int> _allegianceTimers;
		private Dictionary<FactionType, int> _powerTimers;
        private GameModel _model;

		// Use this for initialization
		void Start ()
		{
			FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
			ServantModel servants = AmbitionApp.GetModel<ServantModel>();
            _model = AmbitionApp.GetModel<GameModel>();
            ServantVO spymaster;

			availableSpymasterTestTheWaters = servants.Servants.TryGetValue(ServantConsts.SPYMASTER, out spymaster);
	        availableTestTheWaters = true;

            _allegianceTimers = new Dictionary<FactionType, int>();
            _powerTimers = new Dictionary<FactionType, int>();
			foreach(FactionType faction in fmod.Factions.Keys)
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
	        if (model[FactionType.Crown].Level >= 8)
	        {
	            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
	                        "\n- Like Expensive but Modest Clothes" +
					"\n- Power: " + GetPowerString(model[FactionType.Crown].Power) + " (Faction Benefit)";
	        } else if (model[FactionType.Crown].Level >= 6)
	        {
	            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
	                        "\n- Like Expensive but Modest Clothes" +
					"\n- Power: " + GetPowerString(model[FactionType.Crown].Power) + " (Faction Benefit)";
	        } else
	        {
	            crownInfo.text = "- The Royalty and members of high ranking nobility alligned with them" +
	                        "\n- Like Expensive but Modest Clothes" +
	                        "\n- Power: " + model[FactionType.Crown].knownPower + " " + ConvertKnowledgeTimer(_powerTimers[FactionType.Crown]);
	        }
	        //---- Church Info ----
	        if (model[FactionType.Church].Level >= 8)
	        {
	            churchInfo.text = "- The Clergy and those alligned with them" +
	                        "\n- Like Vintage and Modest Clothes" +
					"\n- Allegiance: " + GetAllegianceString(model[FactionType.Church].Allegiance) + " (Faction Benefit)" +
					"\n- Power: " + GetPowerString(model[FactionType.Church].Power) + " (Faction Benefit)";
	        }
	        else if (model[FactionType.Church].Level >= 6)
	        {
	            churchInfo.text = "- The Clergy and those alligned with them" +
	                         "\n- Like Vintage and Modest Clothes" +
	                         "\n- Allegiance: " + model[FactionType.Church].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers[FactionType.Church]) +
					"\n- Power: " + GetPowerString(model[FactionType.Church].Power) + " (Faction Benefit)";
	        }
	        else
	        {
	            churchInfo.text = "- The Clergy and those alligned with them" +
	                        "\n- Like Vintage and Modest Clothes" +
	                        "\n- Allegiance: " + model[FactionType.Church].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers[FactionType.Church]) +
	                        "\n- Power: " + model[FactionType.Church].knownPower + " " + ConvertKnowledgeTimer(_powerTimers[FactionType.Church]);
	        }
	        //---- Military Info ----
	        if (model[FactionType.Military].Level >= 8)
	        {
	            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
	                        "\n- Couldn't care less about your clothes" +
					"\n- Allegiance: " + GetAllegianceString(model[FactionType.Military].Allegiance) + " (Faction Benefit)" +
					"\n- Power: " + GetPowerString(model[FactionType.Military].Power) + " (Faction Benefit)";
	        }
	        else if (model[FactionType.Military].Level >= 6)
	        {
	            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
	                        "\n- Couldn't care less about your clothes" +
	                        "\n- Allegiance: " + model[FactionType.Military].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers[FactionType.Military]) +
					"\n- Power: " + GetPowerString(model[FactionType.Military].Power) + " (Faction Benefit)";
	        }
	        else
	        {
	            militaryInfo.text = "- The Generals and Troops of the armed forces and those alligned with them" +
	                        "\n- Couldn't care less about your clothes" +
	                        "\n- Allegiance: " + model[FactionType.Military].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers[FactionType.Military]) +
	                        "\n- Power: " + model[FactionType.Military].knownPower + " " + ConvertKnowledgeTimer(_powerTimers[FactionType.Military]);
	        }
	        //---- Bourgeoisie Info ----
	        if (model[FactionType.Bourgeoisie].Level >= 8)
	        {
	            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
	                        "\n- Like clothes that are Luxurious and Racy" +
					"\n- Allegiance: " + GetAllegianceString(model[FactionType.Bourgeoisie].Allegiance) + " (Faction Benefit)" +
					"\n- Power: " + GetPowerString(model[FactionType.Bourgeoisie].Power) + " (Faction Benefit)";
	        }
	        else if (model[FactionType.Bourgeoisie].Level >= 6)
	        {
	            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
	                        "\n- Like clothes that are Luxurious and Racy" +
	                        "\n- Allegiance: " + model[FactionType.Bourgeoisie].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers[FactionType.Bourgeoisie]) +
	                        "\n- Power: " + GetPowerString(model[FactionType.Bourgeoisie].Power) + " (Faction Benefit)";
	        }
	        else
	        {
	            bourgeoisieInfo.text = "- The newly wealthy Mercantile class" +
	                        "\n- Like clothes that are Luxurious and Racy" +
	                        "\n- Allegiance: " + model[FactionType.Bourgeoisie].knownAllegiance + " " + ConvertKnowledgeTimer(_allegianceTimers[FactionType.Bourgeoisie]) +
	                        "\n- Power: " + model[FactionType.Bourgeoisie].knownPower + " " + ConvertKnowledgeTimer(_powerTimers[FactionType.Bourgeoisie]);
	        }
	        //---- Third Estate Info ----
	        if (model[FactionType.Revolution].Level >= 8)
	        {
	            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
	                        "\n - Like clothes that are Vintage and Racy" +
					"\n- Power: " + GetPowerString(model[FactionType.Revolution].Power) + " (Faction Benefit)";
	        }
	        else if (model[FactionType.Revolution].Level >= 6)
	        {
	            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
	                        "\n - Like clothes that are Vintage and Racy" +
								"\n- Power: " + GetPowerString(model[FactionType.Revolution].Power) + " (Faction Benefit)";
	        }
	        else
	        {
	            revolutionInfo.text = "- The unhappy Common Man along with the Academics and Artists alligned with them" +
	                        "\n - Like clothes that are Vintage and Racy" +
	                        "\n- Power: " + model[FactionType.Revolution].knownPower + " " + ConvertKnowledgeTimer(_powerTimers[FactionType.Revolution]);
	        }
	    }

	    public void TestTheWatersPower(FactionType faction)
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
	            else if (availableTestTheWaters && _model.Livre.Value > testTheWatersCost)
	            {
	                availableTestTheWaters = false;
	                _model.Livre.Value -= testTheWatersCost;
					_powerTimers[faction] = 0;
	                UpdateInfo();
	            }
	            else
	            {
	                Debug.Log("You've already used up your charges to Test the Waters for today and/or you don't have enough money :(");
	            }
	        } 
	    }

	    public void TestTheWatersAllegiance(FactionType faction)
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
	            else if (availableTestTheWaters && _model.Livre.Value > testTheWatersCost)
	            {
	                availableTestTheWaters = false;
	                _model.Livre.Value -= testTheWatersCost;
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
			List<FactionType> factions = new List<FactionType>(_allegianceTimers.Keys);
	        foreach (FactionType faction in factions)
	        {
	        	_allegianceTimers[faction]++;
				_powerTimers[faction]++;
			}
	    }

	// TODO: Store them in order
	    private string GetPowerString(int power)
	    {
			return AmbitionApp.GetString("power." + power.ToString());
	    }

	// TODO: Store them in order
		private string GetAllegianceString(int allegiance)
	    {
	    	FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
			int index = fmod.Allegiance.Length-1;
			while (index >= 0 && allegiance >= fmod.Allegiance[index])
				index--;
			return AmbitionApp.GetString("allegiance." + index.ToString());
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
