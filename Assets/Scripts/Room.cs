using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {

    public Party party; //What party is this Room part of?
    public int starRating; //How difficult is this Room? 1-5 Stars
    public bool cleared; //Has this Room been Cleared?

    public string name; //Randomly Generated Room Name
    public int xPos; //Where is this room in the Party's Room Grid?
    public int yPos; // Same
    public int entranceDistance = -1; //Distance from the entrance of the Party. Assigned via the flood fill algorithm. Starts at -1 to find stragglers

    public bool revealed; // Has the Player viewed this room yet?
    public bool punchBowl; // Is there a Punch Bowl in this Room?
    public bool hostHere; // Is the Host/Hostess in this Room?
    public bool entrance; // Is this Room the Entrance to the Party?
    public bool playerHere; //Is the Player in this Room?
    public bool playerAdjacent; //Is the Player Next to this room?

    public List<Guest> guestList = new List<Guest>(); //Holds all the Guests in the Room
    public List<Reward> rewardList = new List<Reward>(); //Holds all the Rewards one could win from the Room. Index 4 = 4 Charmed Guests, Index 3 = 3 Charmed Guests, etc...
    public List<Gossip> gossipList = new List<Gossip>();

    public Notable host;

    //Initially used in Party Generation
    public Room(Party p, int x, int y)
    {
        party = p;
        xPos = x;
        yPos = y;
        name = GenerateName();
        GenerateRewards();
        revealed = false;
    }

    string GenerateName()
    {
        if (entrance)
        {
            return "Entrance";
        } else
        {
            int adjectiveInt = Random.Range(0, GameData.roomAdjectiveList.Count);
            int nounInt = Random.Range(0, GameData.roomNounList.Count);
            string adjective = GameData.roomAdjectiveList[adjectiveInt];
            string noun = GameData.roomNounList[nounInt];
            return "The " + adjective + " " + noun;
        }
    }

    public int MoveThroughChance()
    {
        if (!cleared)
        {
            return 100 - ((starRating * 10) + 10);
        } else
        {
            return 90;
        }
    }

    public void SetStarRatingAndGuests(int stars, int guests) //Only should be used at creation
    {
        starRating = stars;
        GenerateGuestsAndHost(guests);
    }
    
    void GenerateGuestsAndHost(int guests)
    {
        guestList.Clear(); // Just in case, to make sure there aren't more than 4 Guests
        //Constructor = Guest(opinion, interest cap)
        switch (starRating)
        {
            case 0:
                //Create No Guests, this is an Event room
                name = "?????";
                break;
            case 1:
                for (int i = 0; i < guests; i++)
                {
                    guestList.Add(new Guest(Random.Range(25, 51), Random.Range(6, 10)));
                }
                break;
            case 2:
                for (int i = 0; i < guests; i++)
                {
                    guestList.Add(new Guest(Random.Range(25, 46), Random.Range(5, 9)));
                }
                break;
            case 3:
                for (int i = 0; i < guests; i++)
                {
                    guestList.Add(new Guest(Random.Range(25, 41), Random.Range(4, 8)));
                }
                break;
            case 4:
                for (int i = 0; i < guests; i++)
                {
                    guestList.Add(new Guest(Random.Range(25, 36), Random.Range(3, 7)));
                }
                break;
            case 5:
                for (int i = 0; i < guests; i++)
                {
                    guestList.Add(new Guest(Random.Range(20, 31), Random.Range(2, 6)));
                }
                break;
            case 6:
                hostHere = true;
                host = new Notable();
                name = "The Host's Quarters";
                break;
        }
    }

    void GenerateRewards()
    {
        rewardList.Clear(); //Just in case, to make sure there isn't a bunch of other Rewards
        for (int i = 0; i <= 6; i++)
        {
            rewardList.Add(new Reward(party, "Random", i));
        }
    }

    public void AddEnemy(Enemy e)
    {
        guestList[Random.Range(0, guestList.Count)] = new Guest(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        for(int i = 0; i < guestList.Count; i++)
        {
            if (guestList[i].name == e.Name() && guestList[i].isEnemy)
            {
                guestList[i] = new Guest();
            }
        }
    }
}
