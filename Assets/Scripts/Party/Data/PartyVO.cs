using System;
using System.Collections;
using System.Collections.Generic;
using Ambition;

public class PartyVO
{
    public string Faction;
    public int partySize;
    public bool invited=false;
    public int invitationDistance; // How many days before does the Player have to be before they get invited (if eligible)?
    public int RSVP = 0; //0 means no RSVP yet, 1 means Attending and -1 means Decline
    public int playerRSVPDistance = -1;
    public int modestyPreference;
    public int luxuryPreference;
    public float MaleToFemaleRatio = 1f; //Default: 1:1
    public System.DateTime Date;

    public string description;	// Randomly Generated Flavor Description
    public string IntroText;	// Localization string triggers intro text for certain parties.
    public string MapID;		// ID for parties with pregenerated maps

    public NotableVO Host;
    public EnemyVO[] Enemies;

    public int Turns;

    //Drinking and Intoxication
    public int maxPlayerIntoxication = 100;
    public int maxPlayerDrinkAmount = 3;
    public int drinkStrength = 20;

    public List<RemarkVO> playerHand = new List<RemarkVO>();
    public string lastTone;

    public bool blackOutEnding = false; //Did they Party end normally or via Blacking Out?
    public string blackOutEffect; // This is used for the After Party Report
    public int blackOutEffectAmount; //This is also used for the After Party Report

	public List<RewardVO> Rewards = new List<RewardVO>();

    public PartyVO()
    {
		Random rnd = new Random();
		Host = new NotableVO();
		GenerateRandomDescription();
		invitationDistance = 1 + rnd.Next(8) + rnd.Next(8); //Pseudo Normalized Value
    }

    //Constructor that make parties of a particular size, 0 means no Party
    public PartyVO(int size) : this()
    {
        partySize = size;
		SetRandomFaction();
        Host.Faction = Faction;
        Turns = (partySize * 5) + 1;
    }

    //Constructor that makes a Party that ISN'T the included faction
    public PartyVO(string faction) : this()
    {
		SetExclusiveFaction(faction);
		Host.Faction = faction;
        partySize = new Random().Next(1, 4);
    }

    void SetRandomFaction()
    {
    	FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
    	int factionIndex = new Random().Next(fmod.Factions.Count);
		List<string> factions = new List<string>(fmod.Factions.Keys);
		Faction = factions[factionIndex];
		modestyPreference = fmod.Factions[Faction].Modesty;
		luxuryPreference = fmod.Factions[Faction].Luxury;
    }

    void SetExclusiveFaction(string excludeFaction)
    {
		FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
    	int factionIndex = new Random().Next(1, fmod.Factions.Count);
		List<string> factions = new List<string>(fmod.Factions.Keys);
    	Faction = factions[factionIndex];
    	if (Faction == excludeFaction) Faction = factions[0];
		modestyPreference = fmod.Factions[Faction].Modesty;
		luxuryPreference = fmod.Factions[Faction].Luxury;
    }
    
    void GenerateRandomDescription()
    {
        description = "This party is being hosted by some dude or dudette. This segment will later have randomly generated Text describing the party. It should be pretty damn funny.";
    }

    public string SizeString()
    {
        switch (partySize)
        {
            case 1:
                return "Trivial";
            case 2:
                return "Decent";
            case 3:
                return "Grand";
            default:
                return "Nothing";
        }
    }

    //TODO - Overhaul this. This is supposed to be feeding data to the After Party Report. I don't even know what this info is for now. All I know is that all of this is wrong.
    public List<int> ReportInfo()
    {
        List<int> info = new List<int>();
        //info[0] = Total Reputation Change
        info.Add(5);
        //info[1] = Outfit Reputation Change
        info.Add(((playerRSVPDistance - 2) * 20));
        //info[2] = RSVP Reptutation Change
        info.Add((playerRSVPDistance - 2) * 20);
        return info;
    }

    public string Name()
    {
        string name;
        name = SizeString() + " " + Faction + " Party";
        return name;
    }

    public string Description()
    {
        return description;
    }

    public string Objective1()
    {
        return "- Charm the Host";
    }

    public string Objective2()
    {
        return "- Eat ALL the Hors D'oeuvres";
    }

    public string Objective3()
    {
        return "- Don't get too trashed";
    }

    public string Guest1()
    {
        return "- Vis-Prince Christophe Sagnier";
    }

    public string Guest2()
    {
        return "- Prince Emile Fauconier";
    }

    public string Guest3()
    {
        return "- Lady Volteza ";
    }

    public void InvitePlayer()
    {
        invited = true;
    }
}
