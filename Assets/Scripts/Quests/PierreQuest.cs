using System;
using System.Linq;
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
    public CommodityVO reward;

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
        		reward = new CommodityVO(CommodityType.Reputation, multiplier*rnd.Next(6,16));
        		break;
        	case 1:
        		string faction = FACTIONS[rnd.Next(1, FACTIONS.Length)];
        		if (faction == Faction) faction = FACTIONS[0];
                reward = new CommodityVO(CommodityType.Reputation, faction, multiplier * (rnd.Next(10, 21)));
        		break;
        	case 2:
				reward = new CommodityVO(CommodityType.Livre, multiplier*rnd.Next(10,21));
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
        IEnumerable <DateTime> dates = model.Timeline.Keys.Where(k => k.Date > model.Today && k.CompareTo(model.Today.AddDays(MAX_DEADLINE)) <= 0).OrderBy(k=>k.Ticks);
        foreach(DateTime date in dates)
        {
            if (model.Timeline[date].Exists(e => e is PartyVO))
            {
                daysTimeLimit = date.Subtract(model.Today).Days + Util.RNG.Generate(3);
                return;
            }
        }
        daysTimeLimit = MAX_DEADLINE;
    }

    public string FlavorText()
    {
        return "We need Gossip concerning the " + Faction + ". Get it to me and I can get you " + reward.ID + " for it.";
    }
}