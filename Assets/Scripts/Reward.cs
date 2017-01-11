using UnityEngine;
using System.Collections;

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
                typeRandomInt = Random.Range(1, 6);
                if (typeRandomInt == 1 || typeRandomInt == 2)
                {
                    type = "Reputation";
                    amount = 30;
                }
                else if (typeRandomInt == 3 || typeRandomInt == 4)
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
        Faction faction = pQuest.Faction(); 
        int typeRandomInt = Random.Range(1, 4);
        //Amount is determined Inverse to Time Limit
        int multiplier = 12 - pQuest.daysLeft;
        if (typeRandomInt == 1)
        {
            type = "Reputation";
            amount = multiplier * (Random.Range(6, 16));
        } else if (typeRandomInt == 2)
        {
            type = "Faction Reputation";
            subtype = RandomExclusiveFaction(faction);
            amount = multiplier * (Random.Range(10, 21));
        } else
        {
            type = "Livre";
            amount = multiplier * (Random.Range(10, 21));
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
                return "An Introduction to Hire " + GameData.servantDictionary[subtype].NameAndTitle();
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
        int factionRandom = Random.Range(0, 7);
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

    string RandomExclusiveFaction(Faction faction)
    {
        int factionRandom = Random.Range(0, 5);
        Faction selectedFaction;
        switch (factionRandom)
        {
            case 0:
                selectedFaction = GameData.factionList["Crown"];
                break;
            case 1:
                selectedFaction = GameData.factionList["Church"];
                break;
            case 2:
                selectedFaction = GameData.factionList["Military"];
                break;
            case 3:
                selectedFaction = GameData.factionList["Bourgeoisie"];
                break;
            default:
                selectedFaction = GameData.factionList["Revolution"];
                break;
        }
        if(selectedFaction != faction)
        {
            return selectedFaction.Name();
        } else
        {
            return RandomExclusiveFaction(faction);
        }
    }
}
