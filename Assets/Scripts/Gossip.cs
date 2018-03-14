using System;
using System.Collections;
using System.Linq;
using Ambition;

public class Gossip {

    public string Faction; // The Faction this Gossip is relevant to
    //Character character The Character this Gossip is relevant to (requires the Character system to be in place)
    int livreValue;
    int factionPowerShift;
    int factionAllegianceShift;
    public float freshness;
    public string flavorText;

	public Gossip(PartyVO party) : this(party.Faction) {}
    public Gossip(string faction=null)
    {
        Faction = faction;
        //character = SomethingSomething <- If the Gossip affects a specific character
        livreValue = RandomLivreValue();
        factionPowerShift = 100;
        factionAllegianceShift = RandomFactionAllegianceShift();
        freshness = 11; // Starts at 11 because they lose 1 on the day they start
        flavorText = RandomFlavorText();
    }

    string RandomFaction(string faction)
    {
        //Randomly Choose a faction, weighted towards the Faction hosting the Party
		if (faction != null && UnityEngine.Random.Range(0,2) == 0)
			return faction;
		
		FactionVO[] factions = AmbitionApp.GetModel<FactionModel>().Factions.Keys.Cast<FactionVO>().ToArray();
		return factions[UnityEngine.Random.Range(0,factions.Length)].Name;
	}

    //Character RandomCharacter (Picks a Random active character from the Faction)
    
    int RandomLivreValue()
    {
        return UnityEngine.Random.Range(25, 101);
    }

    int RandomFactionPowerShift()
    {
    	return UnityEngine.Random.Range(0,4) == 0 ? UnityEngine.Random.Range(10, 51) : -UnityEngine.Random.Range(10, 51);
    }

    int RandomFactionAllegianceShift()
    {
		if (Faction == "Third Estate" || Faction == "Crown")
			return 0;
    	return UnityEngine.Random.Range(0, 2) == 0 ? UnityEngine.Random.Range(10, 51) : -UnityEngine.Random.Range(10, 51);
    }

    string RandomFlavorText()
    {
        return "Rumor has it that...";
    }

    public string Name()
    {
        return "A tidbit of " + Faction + " Gossip";
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
        return freshness * 0.1f;
    }
}