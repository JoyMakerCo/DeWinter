using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

public class PierreQuest
{
	private static readonly string[] FACTIONS = new string[]{"Crown", "Church", "Military", "Bourgoisie", "Revolution"};
	private const int MAX_DEADLINE = 15;

    public string Faction; // The Faction this Quest is relevant to
    //Character character //The Character this Quest is relevant to (requires the Character system to be in place)
    public int daysTimeLimit; //How long the Player has to complete this Quest
    public int daysLeft;
    public string Name;
    public Reward reward;

    public PierreQuest()
    {
		Faction = FACTIONS[new System.Random().Next(5)];
        GenerateDeadline();
        daysLeft = daysTimeLimit;
        reward = new Reward(this);
        Name = GenerateName();
    }

    public bool GossipMatch(Gossip gossip)
    {
        return gossip.Faction == Faction;
    }

    string GenerateName()
    {
        return "Get " + Faction + " Gossip";
    }

    void GenerateDeadline()
    {
    	CalendarModel model = DeWinterApp.GetModel<CalendarModel>();
    	List<Party> parties;
    	DateTime day = model.Today;
		for (int i=0; i<MAX_DEADLINE; i++)
		{
			if (model.Parties.TryGetValue(day.AddDays(i), out parties) && parties.Count > 0)
			{
				daysTimeLimit = i + new System.Random().Next(3);
				return;
			}
		}
    }

    public string FlavorText()
    {
        return "We need Gossip concerning the " + Faction + ". Get it to me and I can get you " + reward.Name() + " for it.";
    }
}