﻿using System;
using System.Collections;
using System.Collections.Generic;
using Ambition;

public class PierreQuest
{
	private static readonly string[] FACTIONS = new string[]{"Crown", "Church", "Military", "Bourgoisie", "Third Estate"};
	private const int MAX_DEADLINE = 15;

    public string Faction; // The Faction this Quest is relevant to
    //Character character //The Character this Quest is relevant to (requires the Character system to be in place)
    public int daysTimeLimit; //How long the Player has to complete this Quest
    public int daysLeft;
    public string Name;
    public RewardVO reward;

    public PierreQuest()
    {
    	System.Random rnd = new System.Random();
		Faction = FACTIONS[new System.Random().Next(5)];
        GenerateDeadline();
        daysLeft = daysTimeLimit;
		Name = GenerateName();

		int multiplier = 12-daysLeft;
        switch(rnd.Next(3))
        {
        	case 0:
        		reward = new RewardVO(RewardConsts.VALUE, GameConsts.REPUTATION, multiplier*rnd.Next(6,16));
        		break;
        	case 1:
        		string faction = FACTIONS[rnd.Next(1, FACTIONS.Length)];
        		if (faction == Faction) faction = FACTIONS[0];
				reward = new RewardVO(RewardConsts.FACTION, faction, multiplier * (rnd.Next(10, 21)));
        		break;
        	case 2:
				reward = new RewardVO(RewardConsts.VALUE, GameConsts.LIVRE, multiplier*rnd.Next(10,21));
        		break;
        }
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
    	CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
    	List<PartyVO> parties;
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
        return "We need Gossip concerning the " + Faction + ". Get it to me and I can get you " + reward.Name + " for it.";
    }
}