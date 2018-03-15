using System;
using System.Collections;
using System.Collections.Generic;
using Ambition;

public class Reward {

    PartyVO party;
    string type; //Reputation, Faction Rep, Faction Power, Livres, Outfit, Accessory, ServantIntro, Random or Nothing (a placeholder, essentially)
    string subtype; //Use in Faction Rep and Faction Power, will need to be used for Outfit, Accessory and ServantIntro
    public int amount;

    public Reward(PierreQuest pQ)
    {
        party = null;
        GenerateRandomQuestReward(pQ);
    }

    public Reward (PartyVO p, string t, int a) {
        party = p;
        type = t;
        amount = a;
        if (type == "Random")
        {
            GenerateRandomPartyReward(a);
        } else if (type == "Gossip")
        {
            subtype = PartyRandomFaction();
        }
    }

    public Reward(PartyVO p, string t, string sT, int a)
    {
        party = p;
        type = t;
        subtype = sT;
        amount = a;
        if (type == "Random")
        {
            GenerateRandomPartyReward(a);
        }
    }

    void GenerateRandomPartyReward(int level)
    {
        int typeRandomInt = 0; //Have to define it here, leave this line alone
        switch (level)
        {
            case 0:
                type = "Nothing";
                amount = 0;
                break;
            case 1:
                if (typeRandomInt == 1 || typeRandomInt == 2)
                {
                    type = "Reputation";
                    amount = 5;
                }
                else if (typeRandomInt == 3 || typeRandomInt == 4)
                {
                    type = "Faction Reputation";
                    subtype = party.Faction;
                    amount = 10;
                }
                else
                {
                    type = "Gossip";
                    subtype = party.Faction;
                    amount = 1;
                }
                break;
            case 2:
                if (typeRandomInt == 1 || typeRandomInt == 2)
                {
                    type = "Reputation";
                    amount = 10;
                }
                else if (typeRandomInt == 3 || typeRandomInt == 4)
                {
                    type = "Faction Reputation";
                    subtype = party.Faction;
                    amount = 20;
                }
                else
                {
                    type = "Gossip";
                    subtype = party.Faction;
                    amount = 1;
                }
                break;
            case 3:
                if (typeRandomInt == 1 || typeRandomInt == 2)
                {
                    type = "Reputation";
                    amount = 15;
                }
                else if (typeRandomInt == 3 || typeRandomInt == 4)
                {
                    type = "Faction Reputation";
                    subtype = party.Faction;
                    amount = 30;
                }
                else
                {
                    type = "Gossip";
                    subtype = party.Faction;
                    amount = 1;
                }
                break;
            case 4:
                if (typeRandomInt == 1 || typeRandomInt == 2)
                {
                    type = "Reputation";
                    amount = 20;
                }
                else if (typeRandomInt == 3 || typeRandomInt == 4)
                {
                    type = "Faction Reputation";
                    subtype = party.Faction;
                    amount = 40;
                }
                else
                {
                    type = "Gossip";
                    subtype = party.Faction;
                    amount = 1;
                }
                break;
            case 5:
                typeRandomInt = Util.RNG.Generate(0,6);
                if (typeRandomInt == 0 || typeRandomInt == 1)
                {
                    type = "Reputation";
                    amount = 30;
                }
                else if (typeRandomInt == 2 || typeRandomInt == 3)
                {
                    type = "Faction Reputation";
                    subtype = party.Faction;
                    amount = 60;
                }
                else
                {
                    type = "Introduction";
                    subtype = "Seamstress";
                    amount = 1;
                }
                break;
        }
    }

    void GenerateRandomQuestReward(PierreQuest pQuest)
    {
        string faction = pQuest.Faction; 
        int typeRandomInt = Util.RNG.Generate(0,4);
        //Amount is determined Inverse to Time Limit
        int multiplier = 12 - pQuest.daysLeft;
        if (typeRandomInt == 0)
        {
            type = "Reputation";
            amount = multiplier * (Util.RNG.Generate(6, 16));
        } else if (typeRandomInt == 1)
        {
            type = "Faction Reputation";
            subtype = RandomExclusiveFaction(faction);
			amount = multiplier * (Util.RNG.Generate(10, 21));
        } else
        {
            type = "Livre";
			amount = multiplier * (Util.RNG.Generate(10, 21));
        }
    }

    public string Name()
    {
        switch (type)
        {
            case "Reputation":
                return amount + " " + type;
            case "Faction Reputation":
                return amount + " " + type + " (" + subtype +")";
            case "Livre":
                return amount + " Livres";
            case "Introduction":
				ServantModel model = AmbitionApp.GetModel<ServantModel>();
				List<ServantVO> servants;
				return (model.Unknown.TryGetValue(subtype, out servants) && servants.Count > 0)
					? servants[Util.RNG.Generate(0,servants.Count)].NameAndTitle
					: type;
            case "Gossip":
                return "A tidbit of " + SubType() + " Gossip";
        }
        return type;
    }

    public string Type()
    {
        return type;
    }

    public string SubType()
    {
        if(subtype != null)
        {
            return subtype;
        } else
        {
            return "Null";
        }
    }

    string PartyRandomFaction()
    {
        //Randomly Choose a faction, weighted towards the Faction hosting the Party
        int factionRandom = Util.RNG.Generate(0,7);
        switch (factionRandom)
        {
            case 0:
                return "Crown";
            case 1:
                return "Church";
            case 2:
                return "Military";
            case 3:
                return "Bourgeoisie";
            case 4:
                return "Third Estate";
            default:
                return party.Faction;
        }
    }

    string RandomExclusiveFaction(string faction)
    {
    	FactionModel model = AmbitionApp.GetModel<FactionModel>();
    	string[] factions = System.Linq.Enumerable.ToArray(model.Factions.Keys);
    	int index = Util.RNG.Generate(1, factions.Length);
		return factions[factions[index] == faction ? 0 : index];
    }
}