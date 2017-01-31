using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

public class Party {

    public string faction;
    public int partySize;
    public bool invited;
    public int invitationDistance; // How many days before does the Player have to be before they get invited (if eligible)?
    public int RSVP = 0; //0 means no RSVP yet, 1 means Attending and -1 means Decline
    public int playerRSVPDistance = -1;
    public int modestyPreference;
    public int luxuryPreference;

    public string description; // Randomly Generated Flavor Description

    public Notable host;

    public MapVO Map;
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
    public int drinkStrength = 50;

    public List<Remark> playerHand = new List<Remark>();
    public string lastTone;

    public List<Reward> wonRewardsList = new List<Reward>(); //Starts Empty, gets rewards added as the Party goes on and Rooms are cleared

    public List<Enemy> enemyList = new List<Enemy>();

    public bool blackOutEnding = false; //Did they Party end normally or via Blacking Out?
    public string blackOutEffect; // This is used for the After Party Report
    public int blackOutEffectAmount; //This is also used for the After Party Report

    public Outfit playerOutfit;
    public Accessory playerAccessory;

    // Default Constructor
    public Party()
    {
        SetFaction();
        partySize = Random.Range(1, 4);
        GenerateRandomDescription();
        turns = (partySize * 5) + 1;
        turnsLeft = turns;
        FillPlayerHand();
        invited = false;
        invitationDistance = Random.Range(1, 8) + Random.Range(1, 9) - 1; //Pseudo Normalized Value
    }

    //Constructor that makes a Party that ISN'T the included faction
    public Party(string notThisFaction)
    {
        SetExclusiveFaction(notThisFaction);
        partySize = Random.Range(1, 4);
        GenerateRandomDescription();
        turns = (partySize * 5) + 1;
        turnsLeft = turns;
        FillPlayerHand();
        invited = false;
        invitationDistance = Random.Range(1, 8) + Random.Range(1, 9) - 1; //Pseudo Normalized Value
    }

    //Constructor that make parties of a particular size, -1 means no Party
    public Party(int size)
    {
        if (size == -1)
        {
            faction = null;
            partySize = 1;
        } else
        {
            SetFactionGuaranteed();
            partySize = size;
        }       
        GenerateRandomDescription();
        turns = (partySize * 5) + 1;
        turnsLeft = turns;
        FillPlayerHand();
        invited = false;
        invitationDistance = Random.Range(1, 8) + Random.Range(1, 9) - 1; //Pseudo Normalized Value
    }

    void SetFactionGuaranteed()
    {
        int partyFaction = Random.Range(0, 5);
        if (partyFaction == 0)
        {
            faction = "Crown";
            modestyPreference = GameData.factionList[faction].modestyLike;
            luxuryPreference = GameData.factionList[faction].luxuryLike;
        }
        if (partyFaction == 1)
        {
            faction = "Church";
            modestyPreference = GameData.factionList[faction].modestyLike;
            luxuryPreference = GameData.factionList[faction].luxuryLike;
        }
        if (partyFaction == 2)
        {
            faction = "Military";
            modestyPreference = GameData.factionList[faction].modestyLike;
            luxuryPreference = GameData.factionList[faction].luxuryLike;
        }
        if (partyFaction == 3)
        {
            faction = "Bourgeoisie";
            modestyPreference = GameData.factionList[faction].modestyLike;
            luxuryPreference = GameData.factionList[faction].luxuryLike;
        }
        if (partyFaction == 4)
        {
            faction = "Revolution";
            modestyPreference = GameData.factionList[faction].modestyLike;
            luxuryPreference = GameData.factionList[faction].luxuryLike;
        }
    }

    void SetFaction()
    {
        int partyFaction = Random.Range(0, 7);
        switch (partyFaction)
        {
            case 0:
                faction = "Crown";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            case 1:
                faction = "Church";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            case 2:
                faction = "Military";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            case 3:
                faction = "Bourgeoisie";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            case 4:
                faction = "Revolution";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            default:
                faction = null;
                break;
        }
    }

    void SetExclusiveFaction(string nTF)
    {
        int partyFaction = Random.Range(0, 7);
        switch (partyFaction)
        {
            case 0:
                faction = "Crown";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            case 1:
                faction = "Church";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            case 2:
                faction = "Military";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            case 3:
                faction = "Bourgeoisie";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            case 4:
                faction = "Revolution";
                modestyPreference = GameData.factionList[faction].modestyLike;
                luxuryPreference = GameData.factionList[faction].luxuryLike;
                break;
            default:
                faction = null;
                break;
        }
        if(nTF == faction)
        {
            SetExclusiveFaction(nTF);
        }
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

    //Updated Version
    public void CompileRewardsAndGossip()
    {
        List<Reward> tempRewardList = new List<Reward>();
        tempRewardList.Add(new Reward(this, "Reputation", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Crown", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Church", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Military", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Bourgeoisie", 0));
        tempRewardList.Add(new Reward(this, "Faction Rep", "Revolution", 0));
        tempRewardList.Add(new Reward(this, "Introduction", "Seamstress", 0));
        tempRewardList.Add(new Reward(this, "Introduction", "Tailor", 0));
        tempRewardList.Add(new Reward(this, "Introduction", "Spymaster", 0));
        tempRewardList.Add(new Reward(this, "Introduction", "Bodyguard", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Crown", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Church", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Military", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Bourgeoisie", 0));
        tempRewardList.Add(new Reward(this, "Gossip", "Revolution", 0));
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
            playerHand.Add(new Remark(lastTone));
            lastTone = playerHand[i].tone;
        }
    }
}