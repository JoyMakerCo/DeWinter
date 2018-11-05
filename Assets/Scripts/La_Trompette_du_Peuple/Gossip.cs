using System.Collections.Generic;
using System.Linq;
using Ambition;
using UnityEngine;

public class Gossip {

    public string Faction; // The Faction this Gossip is relevant to
    private int _tier; //1 for Cheap, 2 for Shocking, 3 for Outrageous
    private bool _powerShiftEffect; //Used when peddling influence, if true then use the 'Shift Power' effect. If false then use the 'Shift Allegiance' effect 
    private int _effectAmount; //Also used when peddling influence
    public float Freshness; //Starts at 15, goes down by one every day
    public string DescriptionText;

    private Dictionary<string, Dictionary<int, Dictionary<bool, string>>> _descriptionDictionary; //This absurd thing is so that we can easily feed all the variables in to get descriptions without a giant if statement tree

	public Gossip(PartyVO party) : this(party.Faction) {
    }

    public Gossip(string faction=null)
    {
        Faction = AmbitionApp.GetModel<FactionModel>().GetRandomFactionNoNeutral().Name;
        _tier = Util.RNG.Generate(1, 4);
        Freshness = 16; // Starts at 16 because they lose 1 on the day they start
        GeneratePoliticalEffect();
        SetUpDescriptionTextDictionary();
        Debug.Log("Constructor 2 being used. Faction = " + Faction);
        GenerateDescriptionText();
    }

    public Gossip() //Used mainly in testing
    {
        Faction = AmbitionApp.GetModel<FactionModel>().GetRandomFactionNoNeutral().Name;
        _tier = Util.RNG.Generate(1, 4);
        Freshness = 16; // Starts at 16 because they lose 1 on the day they start
        GeneratePoliticalEffect();
        SetUpDescriptionTextDictionary();
        GenerateDescriptionText();
    }

    string RandomFaction(string faction)
    {
        //Randomly Choose a faction, weighted towards the Faction hosting the Party
		if (faction != null && Util.RNG.Generate(0,2) == 0)
			return faction;
		
		FactionVO[] factions = AmbitionApp.GetModel<FactionModel>().Factions.Keys.Cast<FactionVO>().ToArray();
		return factions[Util.RNG.Generate(0,factions.Length)].Name;
	}

    public string TierString()
    {
        if (_tier == 3)
        {
            return "Outrageous";
        } else if (_tier == 2)
        {
            return "Shocking";
        } else if (_tier == 1)
        {
            return "Cheap";
        } else
        {
            return "Error!";
        }
    }

    public int Tier()
    {
        return _tier;
    }

    public bool PowerShiftEffect()
    {
        return _powerShiftEffect;
    }

    public string FreshnessString()
    {
        if(Freshness >= 11)
        {
            return "Fresh";
        } else if (Freshness >= 6)
        {
            return "Relevant";
        } else if (Freshness >= 1)
        {
            return "Old";
        } else
        {
            return "Irrelevant";
        }
    }

    void GenerateDescriptionText()
    {
        //Faction
        Dictionary<int, Dictionary<bool, string>> factionDictionary;
        _descriptionDictionary.TryGetValue(Faction, out factionDictionary);
        //Tier
        Dictionary<bool, string> factionTierDictionary;
        factionDictionary.TryGetValue(_tier, out factionTierDictionary);
        //Political Effect
        string descriptionText;
        factionTierDictionary.TryGetValue(_powerShiftEffect, out descriptionText);
        //Result
        DescriptionText = descriptionText;
    }

    void GeneratePoliticalEffect()
    {
        if(Faction == "Crown" || Faction == "Revolution")
        {
            _powerShiftEffect = true; //The Crown and the Revolution can only have their power shifted, not their allegiance
        } else
        {
            if(UnityEngine.Random.Range(0,2) == 0) //A coin-flip
            {
                _powerShiftEffect = true;
            } else
            {
                _powerShiftEffect = false;
            }
        }
    }

    public string PoliticalEffectDescriptionText()
    {
        if (_powerShiftEffect) //It'll effect the power of the relevant faction?
        {
            return "Depending on how we spin this, this could improve or damage the standings of the " + Faction + " by " + PoliticalEffectValue();
        } else //It'll effect the allegiance of the relevnat faction
        {
            return "With some creative prose, this could be used to push the " + Faction + " towards either the Crown or the Revolution by " + PoliticalEffectValue();
        }
    }

    public string Name()
    {
        string _name = TierString() + " " + Faction + " Gossip";
        return _name;
    }

    public int LivreValue()
    {
        int _value = _tier * 50;
        string _freshnessString = FreshnessString();
        if (_freshnessString == "Fresh")
        {
            _value = _value*1; //Nothing, it remains unchanged
        } else if (_freshnessString == "Relevant")
        {
            _value = (int)UnityEngine.Mathf.Clamp(_value * 0.5f, 1, 150);
        } else if (_freshnessString == "Old")
        {
            _value = (int)UnityEngine.Mathf.Clamp(_value * 0.1f, 1, 150);
        } else
        {
            _value = 1; //Irrelevant Gossip is always only worth 1 Livre
        }
        return _value;
    }

    public int PoliticalEffectValue()
    {
        int _value = _tier * 5;
        string _freshnessString = FreshnessString();
        if (_freshnessString == "Fresh")
        {
            _value = _value * 1; //Nothing, it remains unchanged
        }
        else if (_freshnessString == "Relevant")
        {
            _value = (int)UnityEngine.Mathf.Clamp(_value * 0.5f, 1, 15);
        }
        else if (_freshnessString == "Old")
        {
            _value = (int)UnityEngine.Mathf.Clamp(_value * 0.1f, 1, 15);
        }
        else
        {
            _value = 1; //Irrelevant Gossip always has an effect of 1
        }
        return _value;
    }

    void SetUpDescriptionTextDictionary()
    {
        _descriptionDictionary = new Dictionary<string, Dictionary<int, Dictionary<bool, string>>>();
        //The Crown Descriptions
        string crownTier1True = "The Queen was recently seen bedecked in a spectacular new diamond necklace. Is it a sign of royal power, or unnecessary excess?";
        string crownTier2True = "A group of aristocrats have been trying to secure foreign sources of grain. Is it, as they claim, to distribute to starving farmers, or to hoard it for themselves?";
        string crownTier3True = "The Queen plans to grant a title to one of her closest female friends. Is the Queen recognizing merit or merely giving honors to her personal favorites?";
        Dictionary<int, Dictionary<bool, string>> crownDictionary = new Dictionary<int, Dictionary<bool, string>>();
        Dictionary<bool, string> crownTier1Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> crownTier2Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> crownTier3Dictionary = new Dictionary<bool, string>();
        crownTier1Dictionary.Add(true, crownTier1True);
        crownTier2Dictionary.Add(true, crownTier2True);
        crownTier3Dictionary.Add(true, crownTier3True);
        crownDictionary.Add(1, crownTier1Dictionary);
        crownDictionary.Add(2, crownTier2Dictionary);
        crownDictionary.Add(3, crownTier3Dictionary);
        _descriptionDictionary.Add("Crown", crownDictionary);
        
        //The Church Descriptions
        string churchTier1True = "A country priest, in Paris for the Estates General, has been granting asylum to accused criminals. Is he intervening on behalf of the people, or obstructing justice?";
        string churchTier2True = "A bishop has recently intervened on behalf of a group of accused prostitutes. Is he protecting the virtue of innocent women, or trying to cover his own vices?";
        string churchTier3True = "A group of priests are raising money to buy large parcels of farmland from destitute farmers. Is it an act of of relief or just expanding the Church's ownership of France?";
        string churchTier1False = "A bishop has called for the censorship an opera critical of the Church. The king has signalled his assent, but has taken weeks to make a pronouncement. Is the king taking the request seriously, or ignoring it?";
        string churchTier2False = "Representatives from the Third Estate have been calling on priests to rally in favor of land reforms. Is this a recognition of the Church's authority, or crude sabre rattling?";
        string churchTier3False = "The King has been sending personal representatives to Rome to negotiate with the Pope. Is he moving closer to the holy Father, or snubbing the clergy of France by ignoring them?";
        Dictionary<int, Dictionary<bool, string>> churchDictionary = new Dictionary<int, Dictionary<bool, string>>();
        Dictionary<bool, string> churchTier1Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> churchTier2Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> churchTier3Dictionary = new Dictionary<bool, string>();
        churchTier1Dictionary.Add(true, churchTier1True);
        churchTier1Dictionary.Add(false, churchTier1False);
        churchTier2Dictionary.Add(true, churchTier2True);
        churchTier2Dictionary.Add(false, churchTier2False);
        churchTier3Dictionary.Add(true, churchTier3True);
        churchTier3Dictionary.Add(false, churchTier3False);
        churchDictionary.Add(1, churchTier1Dictionary);
        churchDictionary.Add(2, churchTier2Dictionary);
        churchDictionary.Add(3, churchTier3Dictionary);
        _descriptionDictionary.Add("Church", churchDictionary);

        //The Military Descriptions
        string militaryTier1True = "An older veteran, who stands accused of murder, has been discovered to be recieving care packages delivered by soldiers to his jail cell. Is this a sign of loyalty, or corruption?";
        string militaryTier2True = "A Captain in the Garde Royale was recently caught engaging in an utterly staggering number of extra-marital affairs. Is he a vulgar cad, or a romantic rogue?";
        string militaryTier3True = "There's been a sudden increase in soldiers taking second jobs throughout the city. Is this a dereliction of duty, or a sign that they need to be paid more?";
        string militaryTier1False = "The King plans to weigh in on a legal case where an enlisted man struck an officer during an argument at a local theater. Is this attentiveness from the King, or an infringement on military discipline?";
        string militaryTier2False = "They say that the King has been discussing changes to how the army pays its wages. Is the new pay schedule a necessary reform, or a way to pay troops even less?";
        string militaryTier3False = "The King's ministers are begging him to supplement the army's presence around Paris with more foreign mercenaries. Is this meant to relieve exhausted troops, or is the nobility unsure of their loyalty?";
        Dictionary<int, Dictionary<bool, string>> militaryDictionary = new Dictionary<int, Dictionary<bool, string>>();
        Dictionary<bool, string> militaryTier1Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> militaryTier2Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> militaryTier3Dictionary = new Dictionary<bool, string>();
        militaryTier1Dictionary.Add(true, militaryTier1True);
        militaryTier1Dictionary.Add(false, militaryTier1False);
        militaryTier2Dictionary.Add(true, militaryTier2True);
        militaryTier2Dictionary.Add(false, militaryTier2False);
        militaryTier3Dictionary.Add(true, militaryTier3True);
        militaryTier3Dictionary.Add(false, militaryTier3False);
        militaryDictionary.Add(1, militaryTier1Dictionary);
        militaryDictionary.Add(2, militaryTier2Dictionary);
        militaryDictionary.Add(3, militaryTier3Dictionary);
        _descriptionDictionary.Add("Military", militaryDictionary);

        //The Bourgeoisie Descriptions
        string bourgeoisieTier1True = "A coalition of members of the Bourgeoisie have been investing large amount of money in hot air balloons. Are they visionaries, or just wasting money on ridiculous fancies?";
        string bourgeoisieTier2True = "A lawyer recently got in a fist fight with an aristocrat, who tried to claim his seat at the opera. Is this a sign of abuse by the nobility, or the lawyer's unnecessary impudence?";
        string bourgeoisieTier3True = "Some of the Paris's foremost industrialists have been trying to deregulate the price of bread. Is to allow bakers to have the money to make more bread, or is to profiteer off of hunger?";
        string bourgeoisieTier1False = "Some representatives from the Third Estate have gotten free rooms to stay in from a prominent hotelier. However, they've been staying for a long time. Is this a sign of friendship, or are the representatives taking advantage of their host?";
        string bourgeoisieTier2False =  "A group of vinters are preparing a petition to reduce taxes on wine. Are they trying to make things affordable for the common man, or just increasing their profit margins?";
        string bourgeoisieTier3False = "A wealth financier thinks the revolutionaries need to focus their efforts on drafting a constitution. Is he a pragmatic friend of the revolution, or a dismisive boor?";
        Dictionary<int, Dictionary<bool, string>> bourgeoisieDictionary = new Dictionary<int, Dictionary<bool, string>>();
        Dictionary<bool, string> bourgeoisieTier1Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> bourgeoisieTier2Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> bourgeoisieTier3Dictionary = new Dictionary<bool, string>();
        bourgeoisieTier1Dictionary.Add(true, bourgeoisieTier1True);
        bourgeoisieTier1Dictionary.Add(false, bourgeoisieTier1False);
        bourgeoisieTier2Dictionary.Add(true, bourgeoisieTier2True);
        bourgeoisieTier2Dictionary.Add(false, bourgeoisieTier2False);
        bourgeoisieTier3Dictionary.Add(true, bourgeoisieTier3True);
        bourgeoisieTier3Dictionary.Add(false, bourgeoisieTier3False);
        bourgeoisieDictionary.Add(1, bourgeoisieTier1Dictionary);
        bourgeoisieDictionary.Add(2, bourgeoisieTier2Dictionary);
        bourgeoisieDictionary.Add(3, bourgeoisieTier3Dictionary);
        _descriptionDictionary.Add("Bourgeoisie", bourgeoisieDictionary);

        //The Revolution Descriptions
        string revolutionTier1True = "A book salesman was recently arrested for printing profane criticism of the royal family. Is it punishment for vulgar slander, or repression of necessary criticism?";
        string revolutionTier2True = "A trio of Third Estate delegates, in Paris for the Esates General, were detained by the Guet Royale. Were the Guet harassing honorable men, or forced to act against boorish provacateurs?";
        string revolutionTier3True = "An aristocrat challenged a prominent revolutionary thinker to a duel of honor, who dismissed the challenge as barbaric. Is the thinker a enlightened man discarding old, unlawful traditions, or simply a coward?";
        Dictionary<int, Dictionary<bool, string>> revolutionDictionary = new Dictionary<int, Dictionary<bool, string>>();
        Dictionary<bool, string> revolutionTier1Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> revolutionTier2Dictionary = new Dictionary<bool, string>();
        Dictionary<bool, string> revolutionTier3Dictionary = new Dictionary<bool, string>();
        revolutionTier1Dictionary.Add(true, revolutionTier1True);
        revolutionTier2Dictionary.Add(true, revolutionTier2True);
        revolutionTier3Dictionary.Add(true, revolutionTier3True);
        revolutionDictionary.Add(1, revolutionTier1Dictionary);
        revolutionDictionary.Add(2, revolutionTier2Dictionary);
        revolutionDictionary.Add(3, revolutionTier3Dictionary);
        _descriptionDictionary.Add("Revolution", revolutionDictionary);
    }
}