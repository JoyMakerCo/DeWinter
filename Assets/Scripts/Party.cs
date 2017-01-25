using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public Room[,] roomGrid;
    public Room entranceRoom;
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
        GenerateRooms();
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
        GenerateRooms();
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
        GenerateRooms();
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

    void GenerateRooms()
    {
        int gridDimensionX = 0;
        int gridDimensionY = 0;
        int roomCount = 0; //The Total Amount of Rooms at the Party
        int roomDeletionAmount = 0;
        switch (partySize)
        {
            case 1:
                gridDimensionX = 3;
                gridDimensionY = 3;
                roomDeletionAmount = 2;
                break;
            case 2:
                gridDimensionX = 4;
                gridDimensionY = 4;
                roomDeletionAmount = Random.Range(3, 5);
                break;
            case 3:
                gridDimensionX = 5;
                gridDimensionY = 5;
                roomDeletionAmount = Random.Range(5, 7);
                break;
        }
        //Set the Party's Map Size along with some Faction Specific Changes
        if (faction == "Crown")
        {
            gridDimensionX++;
            roomDeletionAmount++;
        } else if (faction == "Revolution"){
            gridDimensionY++;
            roomDeletionAmount++;
        }
        roomGrid = new Room [gridDimensionX, gridDimensionY];
        roomCount = gridDimensionX * gridDimensionY;
        //Fill the Grid with Randomized Rooms
        for(int i = 0; i < gridDimensionX; i++)
        {
            for (int j = 0; j < gridDimensionY; j++)
            {
                roomGrid[i, j] = new Room(this, i, j);
            }
        }
        //Delete a few Random Rooms (Party Size amount)
        for (int i = 0; i < roomDeletionAmount; i++)
        {
            if(roomGrid[Random.Range(0, gridDimensionX), Random.Range(0, gridDimensionY)] != null)
            {
                roomGrid[Random.Range(0, gridDimensionX), Random.Range(0, gridDimensionY)] = null;
                roomCount--;
            }    
        }
        //Set the Entrance (Random, Southern-most Room)
        Room selectedRoom = null;
        while (selectedRoom == null){ //Just in case it selects a null cell
            selectedRoom = roomGrid[Random.Range(0, gridDimensionX), 0];
        }
        roomGrid[selectedRoom.xPos, selectedRoom.yPos].entrance = true;
        roomGrid[selectedRoom.xPos, selectedRoom.yPos].entranceDistance = 0;
        roomGrid[selectedRoom.xPos, selectedRoom.yPos].starRating = 1;

        roomGrid[selectedRoom.xPos, selectedRoom.yPos].name = "The Entrance";
        entranceRoom = roomGrid[selectedRoom.xPos, selectedRoom.yPos]; // This is for the Party Manager Later
        //Flood fill to set Room Entrance Distance 
        FloodFill(entranceRoom);
        //FloodFill(roomGrid, selectedRoom.xPos, selectedRoom.yPos, 0);

        Room furthestRoom = selectedRoom; // Lowest possible distance Room, the entrance
        for (int i = 0; i < gridDimensionX; i++)
        {
            for (int j = 0; j < gridDimensionY; j++)
            {
                if (roomGrid[i, j] != null) // Safety Measure
                {
                    if (roomGrid[i, j].entranceDistance == -1) //Is it untouched by Flood Fill?
                    {
                        roomGrid[i, j] = null; // Kill the bastard Room if there are no connectors
                        roomCount--;
                    } else
                    {
                        if (roomGrid[i,j].entranceDistance > furthestRoom.entranceDistance)
                        {
                            furthestRoom = roomGrid[i, j];
                        }
                        if (!roomGrid[i, j].entrance && !roomGrid[i, j].hostHere)
                        {
                            roomGrid[i, j].SetStarRating((Random.Range(1, 4)+ Random.Range(1, 5))-2); //Set the Star Rating, using a basically normalized curve
                        }
                    }
                }
            }
        }
        //Put in a minimum amount of Punch Bowls (None in the Entrance)
        int punchBowlAmount = Mathf.Clamp(roomCount / 3, 1, roomCount-1); //-1 Because you can't place Punch Bowls in the Entrance
        int punchCounter = 0;
        int randomRoomX;
        int randomRoomY;
        while(punchCounter < punchBowlAmount)
        {
            randomRoomX = Random.Range(0, gridDimensionX);
            randomRoomY = Random.Range(0, gridDimensionY);
            if (roomGrid[randomRoomX,randomRoomY] != null && !roomGrid[randomRoomX, randomRoomY].entrance)
            {
                    roomGrid[randomRoomX, randomRoomY].punchBowl = true;
                    
            }
            punchCounter++;
        }
        //Host Stuff
        roomGrid[furthestRoom.xPos, furthestRoom.yPos].SetStarRating(6); //The Furthest Room is the Host, represented internally by a Star Rating of 6
        host = roomGrid[furthestRoom.xPos, furthestRoom.yPos].host; //Setting the Host for the Party, this is used elsewhere
        //If the Party makes it so there's no other Rooms other than the Entrance (too many deletions) then just redo it
        if (furthestRoom.entranceDistance == 0)
        {
            GenerateRooms();
        }
    }

    void FloodFill(Room entranceRoom) //The Russell recommended version
    {
        List <Room> roomList = new List<Room>();
        roomList.Add(entranceRoom);
        entranceRoom.entranceDistance = 0;
        int xPosInt;
        int yPosInt;
        while (roomList.Count > 0)
        {
            //Remove the First Point from the List
            xPosInt = roomList[0].xPos;
            yPosInt = roomList[0].yPos;
            roomList.RemoveAt(0);
            //Add it's elgible neighbors to the List and Fill their Points
            if ((yPosInt + 1) < roomGrid.GetLength(1))
            {
                Room northRoom = roomGrid[xPosInt, yPosInt + 1];
                if (northRoom != null)
                {
                    if (roomGrid[northRoom.xPos, northRoom.yPos].entranceDistance == -1)
                    {
                        roomList.Add(northRoom);
                        northRoom.entranceDistance = roomGrid[xPosInt, yPosInt].entranceDistance + 1;
                    }
                }
            }
            if ((yPosInt - 1) >= 0)
            {
                Room southRoom = roomGrid[xPosInt, yPosInt - 1];
                if (southRoom != null)
                {
                    if (roomGrid[southRoom.xPos, southRoom.yPos].entranceDistance == -1)
                    {
                        roomList.Add(southRoom);
                        southRoom.entranceDistance = roomGrid[xPosInt, yPosInt].entranceDistance + 1;
                    }
                }
            }
            if ((xPosInt + 1) < roomGrid.GetLength(0))
            {
                Room eastRoom = roomGrid[xPosInt + 1, yPosInt];
                if (eastRoom != null)
                {
                    if (roomGrid[eastRoom.xPos, eastRoom.yPos].entranceDistance == -1)
                    {
                        roomList.Add(eastRoom);
                        eastRoom.entranceDistance = roomGrid[xPosInt, yPosInt].entranceDistance + 1;
                    }
                }
            }
            if ((xPosInt - 1) >= 0)
            {
                Room westRoom = roomGrid[xPosInt - 1, yPosInt];
                if (westRoom != null)
                {
                    if (roomGrid[westRoom.xPos, westRoom.yPos].entranceDistance == -1)
                    {
                        roomList.Add(westRoom);
                        westRoom.entranceDistance = roomGrid[xPosInt, yPosInt].entranceDistance + 1;
                    }
                }
            }
        }
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

    //Adds an Enemy to the Party, used by the Enemy Inventory Script
    public void AddEnemy(Enemy enemy)
    {
        //Randomly Select a Room, add the Enemy
        int xPos = Random.Range(0, roomGrid.GetLength(0));
        int yPos = Random.Range(0, roomGrid.GetLength(1));
        if (roomGrid[xPos,yPos] != null){
            Room enemyRoom = roomGrid[xPos, yPos];
            if (!enemyRoom.hostHere && !enemyRoom.entrance)
            {
                enemyRoom.AddEnemy(enemy);
                //Put the Enemy in the Enemy List, just in case we need that Data
                enemyList.Add(enemy);
            } else
            {
                AddEnemy(enemy);
            }
        } else
        {
            AddEnemy(enemy);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemyList.Contains(enemy))
        {
            //Scroll through all of the Rooms, Remove the Enemy Enemy
            for(int i = 0; i < roomGrid.GetLength(0); i++)
            {
                for(int j = 0; j < roomGrid.GetLength(1); j++)
                {
                    if (roomGrid[i, j] != null)
                    {
                        roomGrid[i, j].RemoveEnemy(enemy); 
                    }
                }
            }
        }
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
            playerHand.Add(new Remark(lastTone));
            lastTone = playerHand[i].tone;
        }
    }
   
}
