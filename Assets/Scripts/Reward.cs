using System;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

public class Reward {

    Party party;
    string type; //Reputation, Faction Rep, Faction Power, Livres, Outfit, Accessory, ServantIntro, Random or Nothing (a placeholder, essentially)
    string subtype; //Use in Faction Rep and Faction Power, will need to be used for Outfit, Accessory and ServantIntro
    public int amount;

    public Reward(PierreQuest pQ)
    {
        party = null;
        GenerateRandomQuestReward(pQ);
    }

    public Reward (Party p, string t, int a) {
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

    public Reward(Party p, string t, string sT, int a)
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
                    subtype = party.faction;
                    amount = 10;
                }
                else
                {
                    type = "Gossip";
                    subtype = party.faction;
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
                    subtype = party.faction;
                    amount = 20;
                }
                else
                {
                    type = "Gossip";
                    subtype = party.faction;
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
                    subtype = party.faction;
                    amount = 30;
                }
                else
                {
                    type = "Gossip";
                    subtype = party.faction;
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
                    subtype = party.faction;
                    amount = 40;
                }
                else
                {
                    type = "Gossip";
                    subtype = party.faction;
                    amount = 1;
                }
                break;
            case 5:
                typeRandomInt = (new Random()).Next(6);
                if (typeRandomInt == 0 || typeRandomInt == 1)
                {
                    type = "Reputation";
                    amount = 30;
                }
                else if (typeRandomInt == 2 || typeRandomInt == 3)
                {
                    type = "Faction Reputation";
                    subtype = party.faction;
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
		Random rnd = new Random();
        int typeRandomInt = rnd.Next(4);
        //Amount is determined Inverse to Time Limit
        int multiplier = 12 - pQuest.daysLeft;
        if (typeRandomInt == 0)
        {
            type = "Reputation";
            amount = multiplier * (rnd.Next(6, 16));
        } else if (typeRandomInt == 1)
        {
            type = "Faction Reputation";
            subtype = RandomExclusiveFaction(faction);
			amount = multiplier * (rnd.Next(10, 21));
        } else
        {
            type = "Livre";
			amount = multiplier * (rnd.Next(10, 21));
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
				ServantModel smod = DeWinterApp.GetModel<ServantModel>();
				ServantVO[] servants = smod.GetServants(subtype);
				ServantVO servant = Array.Find(servants, s => !s.Hired && !s.Introduced);
                return servant != null
                	? "An Introduction to Hire " + servant.NameAndTitle
                	: null;
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
        int factionRandom = new Random().Next(7);
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
                return "Revolution";
            default:
                return party.faction;
        }
    }

    string RandomExclusiveFaction(string faction)
    {
    	FactionModel model = DeWinterApp.GetModel<FactionModel>();
    	List<string> factions = new List<string>(model.Factions.Keys);
    	string val = factions[new Random().Next(factions.Count-1)];
    	return (val != faction) ? val : factions[factions.Count-1];
    }
}