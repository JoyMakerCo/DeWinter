using UnityEngine;
using System.Collections;

public class Gossip {

    FactionVO faction; // The Faction this Gossip is relevant to
    //Character character The Character this Gossip is relevant to (requires the Character system to be in place)
    int livreValue;
    int factionPowerShift;
    int factionAllegianceShift;
    public float freshness;
    public string flavorText;

	public Gossip(Party party)
    {
        faction = RandomFaction(party.faction);
        //character = SomethingSomething <- If the Gossip affects a specific character
        livreValue = RandomLivreValue();
        factionPowerShift = 100;
        factionAllegianceShift = RandomFactionAllegianceShift();
        freshness = 11; // Starts at 11 because they lose 1 on the day they start
        flavorText = RandomFlavorText();
    }

    public Gossip(string factionString)
    {
        faction = GameData.factionList[factionString];
        //character = SomethingSomething <- If the Gossip affects a specific character
        livreValue = RandomLivreValue();
        factionPowerShift = 100;
        factionAllegianceShift = RandomFactionAllegianceShift();
        freshness = 11; // Starts at 11 because they lose 1 on the day they start
        flavorText = RandomFlavorText();
    }

    FactionVO RandomFaction(string partyFaction)
    {
        //Randomly Choose a faction, weighted towards the Faction hosting the Party
        int factionRandom = Random.Range(0, 7);
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
            case 4:
                return GameData.factionList["Revolution"];
            default:
                return GameData.factionList[partyFaction];
         }
     }

    //Character RandomCharacter (Picks a Random active character from the Faction)
    
    int RandomLivreValue()
    {
        return Random.Range(25, 101);
    }

    int RandomFactionPowerShift()
    {
        int positiveOrNegative = Random.Range(0, 4);
        if(positiveOrNegative == 0) //Low chance, but still possible that the Gossip will make the Faction more powerful, not less
        {
            return Random.Range(10, 51);
        } else
        {
            return (Random.Range(10, 51) * -1);
        }
    }

    int RandomFactionAllegianceShift()
    {
        if(faction.Name == "Revolution" || faction.Name == "Crown")
        {
            return 0;
        } else
        {
            int positiveOrNegative = Random.Range(0, 2);
            if (positiveOrNegative == 0) // A 50/50 Chance
            {
                return Random.Range(10, 51);
            }
            else
            {
                return (Random.Range(10, 51) * -1);
            }
        }   
    }

    string RandomFlavorText()
    {
        return "Rumor has it that...";
    }

    public string Name()
    {
        return "A tidbit of " + faction.Name + " Gossip";
    }

    public int LivreValue()
    {
        return (int)(livreValue * (freshness / 10));
    }

    public int PowerShiftValue()
    {
        return (int)(factionPowerShift * (freshness / 10));
    }

    public int AllegianceShiftValue()
    {
        return (int)(factionAllegianceShift * (freshness / 10));
    }

    public float FreshnessValue()
    {
        return freshness / 10;
    }

    public FactionVO Faction()
    {
        return faction;
    }
}
