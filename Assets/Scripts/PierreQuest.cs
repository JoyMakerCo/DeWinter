using UnityEngine;
using System.Collections;
using DeWinter;

public class PierreQuest {

    private FactionVO faction; // The Faction this Quest is relevant to
    //Character character //The Character this Quest is relevant to (requires the Character system to be in place)
    public int daysTimeLimit; //How long the Player has to complete this Quest
    public int daysLeft;
    private string name;
    public Reward reward;

    public PierreQuest()
    {
        faction = RandomFaction();
        GenerateDeadline();
        daysLeft = daysTimeLimit;
        reward = new Reward(this);
        name = GenerateName();
    }

    public bool GossipMatch(Gossip gossip)
    {
        return gossip.Faction == faction;
    }

    FactionVO RandomFaction()
    {
        //Randomly Choose a faction, weighted towards the Faction hosting the Party
        int factionRandom = Random.Range(0, 5);
        switch (factionRandom)
        {
            case 0:
                return GameData.factionList["Crown"];
            case 1:
                return GameData.factionList["Church"];
            case 2:
                return GameData.factionList["Military"];
            case 3:
                return GameData.factionList["Bourgeoisie"];
            default:
                return GameData.factionList["Revolution"];
        }
    }

    string GenerateName()
    {
        return "Get " + faction.Name() + " Gossip";
    }

    void GenerateDeadline()
    {
        Party nextParty = null;
        int i = 0;
        while (nextParty == null)
        {
            if (GameData.calendar.daysFromNow(i).party1.faction != null && GameData.calendar.daysFromNow(i).party1.invited)
            {
                nextParty = GameData.calendar.daysFromNow(i).party1;
            }
            else
            {
                i++;
            }
        }
        daysTimeLimit = Random.Range(i, i + 3);
    }

    public FactionVO Faction()
    {
        return faction;
    }

    public string Name()
    {
        return name;
    }

    public string FlavorText()
    {
        return "We need Gossip concerning the " + faction.Name() + ". Get it to me and I can get you " + reward.Name() + " for it.";
    }


}
