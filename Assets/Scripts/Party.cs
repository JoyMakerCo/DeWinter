using System;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

public class Party {

    public string faction;
    public int partySize;
    public bool invited=false;
    public int invitationDistance; // How many days before does the Player have to be before they get invited (if eligible)?
    public int RSVP = 0; //0 means no RSVP yet, 1 means Attending and -1 means Decline
    public int playerRSVPDistance = -1;
    public int modestyPreference;
    public int luxuryPreference;
    public bool tutorial=false;
    public bool notableEvent = false; //Should this event show up in the Notable Events section of the Parties List?
    public System.DateTime Date;

    public string name; // Randomly Generated Party Name (Flavor)
    public string description; // Randomly Generated Flavor Description
    public string rsvpModalText; //The Text used in the RSVP Modal for this Party

    public Notable host;

    public int turns;
    public int turnsLeft;

    public int maxPlayerConfidence = 0;
    public int startingPlayerConfidence = 0;
    public int currentPlayerConfidence = 0;

    //Drinking and Intoxication
    public int maxPlayerIntoxication = 100;
    public int currentPlayerIntoxication = 0;
    public int currentPlayerDrinkAmount = 3;
    public int maxPlayerDrinkAmount = 3;
    public int drinkStrength = 20;

    public List<Remark> playerHand = new List<Remark>();
    public string lastTone;

    public List<Reward> wonRewardsList = new List<Reward>(); //Starts Empty, gets rewards added as the Party goes on and Rooms are cleared

    public List<Enemy> enemyList = new List<Enemy>();

    public bool blackOutEnding = false; //Did they Party end normally or via Blacking Out?
    public string blackOutEffect; // This is used for the After Party Report
    public int blackOutEffectAmount; //This is also used for the After Party Report

    //Constructor that make parties of a particular size, 0 means no Party
    public Party(int size)
    {
		Random rnd = new Random();
        partySize = size;
		SetRandomFaction();
        host = new Notable(faction);
        turns = (partySize * 5) + 1;
        turnsLeft = turns;
        FillPlayerHand();
        invitationDistance = 1 + rnd.Next(8) + rnd.Next(8); //Pseudo Normalized Value
        GenerateRandomFlavorText();
    }

    //Constructor that makes a Party that ISN'T the included faction
    public Party(string faction)
    {
		Random rnd = new Random();
		SetExclusiveFaction(faction);
        partySize = rnd.Next(1, 4);
        host = new Notable(faction);
        turns = (partySize * 5) + 1;
        turnsLeft = turns;
        FillPlayerHand();
		invitationDistance = 1 + rnd.Next(8) + rnd.Next(8); //Pseudo Normalized Value
        GenerateRandomFlavorText();
    }

    void SetRandomFaction()
    {
    	FactionModel fmod = DeWinterApp.GetModel<FactionModel>();
    	int factionIndex = new Random().Next(fmod.Factions.Count);
		List<string> factions = new List<string>(fmod.Factions.Keys);
		faction = factions[factionIndex];
		modestyPreference = fmod.Factions[faction].Modesty;
		luxuryPreference = fmod.Factions[faction].Luxury;
    }

    void SetExclusiveFaction(string excludeFaction)
    {
		FactionModel fmod = DeWinterApp.GetModel<FactionModel>();
    	int factionIndex = new Random().Next(1, fmod.Factions.Count);
		List<string> factions = new List<string>(fmod.Factions.Keys);
    	faction = factions[factionIndex];
    	if (faction == excludeFaction) faction = factions[0];
		modestyPreference = fmod.Factions[faction].Modesty;
		luxuryPreference = fmod.Factions[faction].Luxury;
    }
    
    void GenerateRandomFlavorText()
    {
        //----- Name Text -----
        int randomNameInt = new Random().Next(1, 10);
        string partyName;
        switch (randomNameInt)
        {
            case 1:
                partyName = "Party";
                break;
            case 2:
                partyName = "Gala";
                break;
            case 3:
                partyName = "Celebration";
                break;
            case 4:
                partyName = "Ball";
                break;
            case 5:
                partyName = "Banquet";
                break;
            case 6:
                partyName = "Festivity";
                break;
            case 7:
                partyName = "Reception";
                break;
            case 8:
                partyName = "Salon";
                break;
            default:
                partyName = "Soiree";
                break;
        }
        name = host.Name + "'s " + SizeString() + " " + partyName;
        //----- Description Text -----
        string descriptionNoun1;
        string descriptionAdjective;
        string descriptionNoun2;
        int descNoun1Int =  new Random().Next(1, 10);
        switch (descNoun1Int)
        {
            case 1:
                descriptionNoun1 = "circumstance";
                break;
            case 2:
                descriptionNoun1 = "showing";
                break;
            case 3:
                descriptionNoun1 = "time";
                break;
            case 4:
                descriptionNoun1 = "presentation";
                break;
            case 5:
                descriptionNoun1 = "proceeding";
                break;
            case 6:
                descriptionNoun1 = "happening";
                break;
            case 7:
                descriptionNoun1 = "affair";
                break;
            case 8:
                descriptionNoun1 = "circumstance";
                break;
            default:
                descriptionNoun1 = "gathering";
                break;
        }
        int descAdjectiveInt = new Random().Next(1, 10);
        switch (descAdjectiveInt)
        {
            case 1:
                descriptionAdjective = "great";
                break;
            case 2:
                descriptionAdjective = "certain";
                break;
            case 3:
                descriptionAdjective = "fascinating";
                break;
            case 4:
                descriptionAdjective = "unquestionable";
                break;
            case 5:
                descriptionAdjective = "elegant";
                break;
            case 6:
                descriptionAdjective = "gentile";
                break;
            case 7:
                descriptionAdjective = "refined";
                break;
            case 8:
                descriptionAdjective = "drunken";
                break;
            default:
                descriptionAdjective = "magnificent";
                break;
        }

        int descNoun2Int = new Random().Next(1, 10);
        switch (descNoun2Int)
        {
            case 1:
                descriptionNoun2 = "splendor";
                break;
            case 2:
                descriptionNoun2 = "gentility";
                break;
            case 3:
                descriptionNoun2 = "elegance";
                break;
            case 4:
                descriptionNoun2 = "merriment";
                break;
            case 5:
                descriptionNoun2 = "mirth";
                break;
            case 6:
                descriptionNoun2 = "gaiety";
                break;
            case 7:
                descriptionNoun2 = "exuberance";
                break;
            case 8:
                descriptionNoun2 = "elation";
                break;
            default:
                descriptionNoun2 = "joi de vivre";
                break;
        }
        description = descriptionNoun1 + " of " + descriptionAdjective + " " + descriptionNoun2;

        //----- RSVP Modal Text -----
        string dateString = DeWinterApp.GetModel<CalendarModel>().GetMonthString(Date) + " " + Date.Day + " " + Date.Year;
        rsvpModalText = "Requests the attendance of Yvette to " + name + " on " + dateString + ". This " + SizeString() + " " + partyName + " is sure to be a " + description + ".";
    }

    void GenerateTutorialDescription()
    {
        description = "The Orphan's Feast is a small social gathering spot for people of every class of society, though almost everyone who comes here does so alone. ";
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
        name = SizeString() + " " + faction + " Party";
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
        return "- Prince Christophe Sagnier";
    }

    public string Guest2()
    {
        return "- Prince Emile Fauconier";
    }

    public string Guest3()
    {
        return "- Lady Volteza ";
    }

    //Updated Version
    public void CompileRewardsAndGossip()
    {
        List<Reward> tempRewardList = new List<Reward>();
        tempRewardList.Add(new Reward(this, "Reputation", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Crown", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Church", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Military", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Bourgeoisie", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Third Estate", 0));
        tempRewardList.Add(new Reward(this, "Introduction", "Seamstress", 0));
        tempRewardList.Add(new Reward(this, "Introduction", "Tailor", 0));
        tempRewardList.Add(new Reward(this, "Introduction", "Spymaster", 0));
        tempRewardList.Add(new Reward(this, "Introduction", "Bodyguard", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Crown", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Church", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Military", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Bourgeoisie", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Third Estate", 0));
        for (int i = 0; i < wonRewardsList.Count; i++)
        {
            for(int j = 0; j < tempRewardList.Count; j++)
            {
                if(wonRewardsList[i].Type() == tempRewardList[j].Type()) //If their Types match
                {
                    if (wonRewardsList[i].Type() == "Faction Rep" || wonRewardsList[i].Type() == "Faction Power" || wonRewardsList[i].Type() == "Introduction" || wonRewardsList[i].Type() == "Gossip") //These Rewards have SubTypes, not all Rewards do
                    {
                        if(wonRewardsList[i].SubType() == tempRewardList[j].SubType())
                        {
                            tempRewardList[j].amount += wonRewardsList[i].amount;
                        }
                    } else //If it's not a Reward with a SubType then just total the amounts
                    {
                        tempRewardList[j].amount += wonRewardsList[i].amount;
                    }
                }
            }
        }
        wonRewardsList.Clear(); //Now that Temp Rewards List has the totalled version of the List, we can clear out the old list
        wonRewardsList = tempRewardList; //The new, totaled list, replaces the old one 
    }

    public void RemoveEnemy(Enemy enemy)
    {
        //Remove the Enemy from the Enemy List
        enemyList.Remove(enemy);
    }

    public void InvitePlayer()
    {
        invited = true;
    }

    void FillPlayerHand()
    {
        playerHand.Add(new Remark());
        lastTone = playerHand[0].tone;
        for (int i = 1; i < 5; i++)
        {
            playerHand.Add(new Remark(lastTone, 2));
            lastTone = playerHand[i].tone;
        }
    }  
}