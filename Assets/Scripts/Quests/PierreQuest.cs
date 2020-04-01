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
    public CommodityVO Reward;
    public string RewardKey;    // UGH

    public PierreQuest()
    {
        // avoid neutrals
        var validFactions = 
        Faction = Util.RNG.TakeRandom( new FactionType[] { 
            FactionType.Crown, FactionType.Church, FactionType.Military, FactionType.Bourgeoisie, FactionType.Revolution 
            } );

        GenerateDeadline();
        daysLeft = daysTimeLimit;
		Name = GenerateName();

        CommodityType rewardType = CommodityType.Livre;
        int amount = 15;

        RewardKey = "quest.reward.livre.0";

        switch(Util.RNG.Generate(18))
        {
        	case 0: 
        	case 1: 
        	case 2: 
                rewardType = CommodityType.Livre;           amount = 15;        RewardKey = "quest.reward.livre.0";   break;

            case 3:
            case 4:
                rewardType = CommodityType.Livre;           amount = 30;        RewardKey = "quest.reward.livre.1";   break;

            case 5:
                rewardType = CommodityType.Livre;           amount = 50;        RewardKey = "quest.reward.livre.2";   break;

            case 6:
            case 7:
            case 8:
                rewardType = CommodityType.Credibility;     amount = 5;        RewardKey = "quest.reward.credibility.0";    break;

            case 9:
            case 10:
                rewardType = CommodityType.Credibility;     amount = 10;        RewardKey = "quest.reward.credibility.1";   break;

            case 11:
                rewardType = CommodityType.Credibility;     amount = 15;        RewardKey = "quest.reward.credibility.2";   break;

            case 12:
            case 13:
            case 14:
                rewardType = CommodityType.Peril;           amount = -5;        RewardKey = "quest.reward.peril.0";   break;

            case 15:
            case 16:
                rewardType = CommodityType.Peril;           amount = -10;        RewardKey = "quest.reward.peril.1";   break;
            
            case 17:
                rewardType = CommodityType.Peril;           amount = -15;        RewardKey = "quest.reward.peril.2";   break;

        }

        Reward = new CommodityVO( rewardType, amount );
    }

    public bool GossipMatch(ItemVO gossip) => gossip.ID == Faction.ToString();

    string GenerateName()
    {
        return "Get " + Faction + " Gossip";
    }

    void GenerateDeadline()
    {
    	CalendarModel model = AmbitionApp.GetModel<CalendarModel>();

        // rough 5-15 bell curve here
        daysTimeLimit = Util.RNG.Generate(5) + Util.RNG.Generate(4) + Util.RNG.Generate(4) + 5;
    }

    public string FlavorText()
    {
        return "We need Gossip concerning the " + Faction + ". Get it to me and I can get you " + Reward.ID + " for it.";
    }

    public override string ToString()
    {
        return string.Format("{0} for {1}", Name, Reward.ToString() );
    }
}