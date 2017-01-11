﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PartyManager : MonoBehaviour {
    public GameObject screenFader; // It's for the Confidence Tally pop-up
    public RoomManager roomManager;

    void Start()
    {
        ConfidenceTally();
        EnemyCheck();
        EngageParty();
        GameData.tonightsParty.roomGrid[GameData.tonightsParty.entranceRoom.xPos, GameData.tonightsParty.entranceRoom.yPos].playerHere = true;
    }

    //Checks all the relevant Enemies and sees if they're going to attend the Party
    void EnemyCheck()
    {
        foreach (Enemy e in GameData.enemyList)
        {
            if(e.faction == GameData.factionList[GameData.tonightsParty.faction])
            {
                if(Random.Range(0,2) == 0)
                {
                    GameData.tonightsParty.AddEnemy(e);
                }
            }
        }
    }

    void EngageParty()
    {
        //Instantiate a the room Holder Parent Object
        roomManager.SetUpMap(GameData.tonightsParty);
        //Damage the Outfit's Novelty, how that the Confidence has already been Tallied
        GameData.tonightsParty.playerOutfit = OutfitInventory.personalInventory[GameData.partyOutfitID];
        OutfitDegradation();

        //Is the Player using the Garter Flack Accessory? If so then increase the amount of Booze they can carry!
        if (GameData.partyAccessoryID != -1)
        {
            GameData.tonightsParty.playerAccessory = AccessoryInventory.personalInventory[GameData.partyAccessoryID];
            if (AccessoryInventory.personalInventory[GameData.partyAccessoryID].Type() == "Garter Flask")
            {
                GameData.tonightsParty.maxPlayerDrinkAmount++;
            }
        }      
    }

    void ConfidenceTally()
    {
        GameData.tonightsParty.maxPlayerConfidence = 0;
        GameData.tonightsParty.currentPlayerConfidence = 0;
        //Calculate Confidence Values Here------------
        //Faction Outfit Likes (Military doesn't know anything, so they use the Average Value)
        float outfitReaction;
        if (GameData.tonightsParty.faction != "Military")
        {
            float modestyLike = GameData.factionList[GameData.tonightsParty.faction].modestyLike;
            float luxuryLike = GameData.factionList[GameData.tonightsParty.faction].luxuryLike;
            float outfitModesty = OutfitInventory.personalInventory[GameData.partyOutfitID].modesty;
            float outfitLuxury = OutfitInventory.personalInventory[GameData.partyOutfitID].luxury;
            float outfitNovelty = (float)OutfitInventory.personalInventory[GameData.partyOutfitID].novelty / 100;
            Debug.Log("Outfit Novelty: " + outfitNovelty);
            //Fix this formula
            outfitReaction = Mathf.Round((((400 - (Mathf.Abs(modestyLike - outfitModesty) + Mathf.Abs(luxuryLike - outfitLuxury))))/2)* outfitNovelty);
            Debug.Log("Rounded Outfit Reaction: " + outfitReaction);
            GameData.tonightsParty.maxPlayerConfidence = (int)outfitReaction;
        } else {
            outfitReaction = 100;
            GameData.tonightsParty.maxPlayerConfidence = (int)outfitReaction;
        }
        //Is it in Style?
        int outfitStyleReaction;
        if (GameData.currentStyle == OutfitInventory.personalInventory[GameData.partyOutfitID].style)
        {
            outfitStyleReaction = 50;
        } else
        {
            outfitStyleReaction = 0;
        }
        GameData.tonightsParty.maxPlayerConfidence += outfitStyleReaction;
        //Is the Accessory in Style and is there a Match?
        int accessoryStyleReaction = 0;
        int outfitAccessoryStyleMatch = 0;
        if (GameData.partyAccessoryID != -1) //Has an Accessory been worn at all?
        {
            if (GameData.currentStyle == AccessoryInventory.personalInventory[GameData.partyAccessoryID].Style())
            {
                accessoryStyleReaction = 50;
            }
            else
            {
                accessoryStyleReaction = 0;
            }
            GameData.tonightsParty.maxPlayerConfidence += accessoryStyleReaction;
            if (OutfitInventory.personalInventory[GameData.partyOutfitID].style == AccessoryInventory.personalInventory[GameData.partyAccessoryID].Style())
            {
                outfitAccessoryStyleMatch = 50;
            }
            else
            {
                outfitAccessoryStyleMatch = 0;
            }
            GameData.tonightsParty.maxPlayerConfidence += outfitAccessoryStyleMatch;
        }
        //Faction Rep
        int factionReaction = GameData.factionList[GameData.tonightsParty.faction].playerReputation / 10;
        GameData.tonightsParty.maxPlayerConfidence += factionReaction;
        //General Rep
        int generalRepReaction = GameData.reputationCount / 20;
        GameData.tonightsParty.maxPlayerConfidence += generalRepReaction;
        //Set Starting Confidence (Needs penalties for multiple Parties in a Row)
        GameData.tonightsParty.currentPlayerConfidence = GameData.tonightsParty.maxPlayerConfidence;
        GameData.tonightsParty.startingPlayerConfidence = GameData.tonightsParty.currentPlayerConfidence; 
        //Put Results in the Pop-Up Here
        object[] objectStorage = new object[11];
        objectStorage[0] = GameData.partyOutfitID;
        objectStorage[1] = GameData.partyAccessoryID;
        objectStorage[2] = GameData.tonightsParty.faction;
        objectStorage[3] = (int)outfitReaction;
        objectStorage[4] = outfitStyleReaction;
        objectStorage[5] = accessoryStyleReaction;
        objectStorage[6] = outfitAccessoryStyleMatch;
        objectStorage[7] = factionReaction;
        objectStorage[8] = generalRepReaction;
        objectStorage[9] = GameData.tonightsParty.maxPlayerConfidence;
        objectStorage[10] = GameData.tonightsParty.currentPlayerConfidence;
        screenFader.gameObject.SendMessage("CreateConfidenceTallyModal", objectStorage);
    }

    //This is currently called by the 'Leave the Party' Button in the Party Scene. May need to automate this in some fashion?
    public void FinishTheParty()
    {
        GameData.tonightsParty.turnsLeft = 0; // This makes this easier for the After Party Report to Advance Time properly
        GameData.tonightsParty.CompileRewardsAndGossip();
        //Distribute the Rewards into the Player's 'Accounts' in Game Data and the appropriate Inventories
        foreach (Reward t in GameData.tonightsParty.wonRewardsList)
        {
            switch (t.Type())
            {
                case "Reputation":
                    GameData.reputationCount += t.amount;
                    break;
                case "Faction Rep":
                    GameData.factionList[t.SubType()].playerReputation += t.amount;
                    break;
                case "Introduction":
                    if(t.amount > 0)
                    {
                        if(!GameData.servantDictionary[t.SubType()].Introduced() && !GameData.servantDictionary[t.SubType()].Hired())
                        {
                            GameData.servantDictionary[t.SubType()].Introduce();
                        }
                    }
                    break;
                case "Gossip":
                    if (t.amount > 0)
                    {
                        for(int i = 0; i < t.amount; i++)
                        {
                            GameData.gossipInventory.Add(new Gossip(t.SubType()));
                        }
                    }
                    break;
                default:
                    Debug.Log("Something went wrong with issuing this Reward of " + t.amount + " " + t.Name());
                    break;
            }
        }
        GameData.partyOutfitID = -1;
        GameData.partyAccessoryID = -1;
    }

    void OutfitDegradation()
    {
        //Reduce Novelty of Outfit. If Outfit has been used twice in a row then it's lowered double.
        if (GameData.partyOutfitID != GameData.lastPartyOutfitID)
        {
            OutfitInventory.outfitInventories["personal"][GameData.partyOutfitID].novelty += GameData.noveltyDamage;
            GameData.woreSameOutfitTwice = false;
        }
        else
        {
            OutfitInventory.outfitInventories["personal"][GameData.partyOutfitID].novelty += GameData.noveltyDamage * 2;
            GameData.woreSameOutfitTwice = true;
        }
        //Now that the calculations are finished, the outfit now becomes the last used outfit.
        GameData.lastPartyOutfitID = GameData.partyOutfitID;
    }
}
