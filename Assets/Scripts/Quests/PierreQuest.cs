using System;
using System.Linq;
using System.Collections.Generic;
using Ambition;

public class PierreQuest
{
	private const int MAX_DEADLINE = 15;

    public FactionType Faction; // The Faction this Quest is relevant to
    //Character character //The Character this Quest is relevant to (requires the Character system to be in place)
    public int daysTimeLimit; //How long the Player has to complete this Quest
    public int daysLeft;
    public string Name;
    public CommodityVO reward;

    public PierreQuest()
    {
    	System.Random rnd = new System.Random();
        Faction = Util.RNG.TakeRandom(AmbitionApp.GetModel<FactionModel>().Factions.Keys.ToArray());
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
                FactionType[] factions = AmbitionApp.GetModel<FactionModel>().Factions.Keys.Where(f => f != FactionType.Neutral && f != Faction).ToArray();
                FactionType faction = Util.RNG.TakeRandom(factions);
                reward = new CommodityVO(CommodityType.Reputation, faction.ToString(), multiplier * (rnd.Next(10, 21)));
        		break;
        	case 2:
				reward = new CommodityVO(CommodityType.Livre, multiplier*rnd.Next(10,21));
        		break;
        }
    }

    public bool GossipMatch(ItemVO gossip) => gossip.ID == Faction.ToString();

    string GenerateName()
    {
        return "Get " + Faction + " Gossip";
    }

    void GenerateDeadline()
    {
    	CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
        daysTimeLimit = (model.GetEvent<PartyVO>() != null
            ? Util.RNG.Generate(3)
            : MAX_DEADLINE);
    }

    public string FlavorText()
    {
        return "We need Gossip concerning the " + Faction + ". Get it to me and I can get you " + reward.ID + " for it.";
    }
}