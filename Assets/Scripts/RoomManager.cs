using UnityEngine;
using System.Collections;
using DeWinter;

public class RoomManager : MonoBehaviour
{

    public PartyManager partyManager;
    public Party party; //What Party are we managing?
    public RoomVO[,] roomButtonGrid; // Holds the Rooms
    public RoomVO entranceRoom; //Whichever Room is the Entrance
    public RoomVO currentPlayerRoom;
    public GameObject roomButtonPrefab;
    public GameObject houseOutlinePrefab; //This is used to Outline the house when it's done
    public Canvas canvas; //New Interface objects need to get parented to this in order to work
    bool mapSetUp = false;
    public GameObject screenFader;

    //Images for the Map Buttons
    public Sprite mapBlock2x2;
    public Sprite mapBlock2x3;
    public Sprite mapBlock2x4;
    public Sprite mapBlock3x3;
    public Sprite mapBlock4x3T;
    public Sprite mapBlock4x4L;

    void Start()
    {
        mapSetUp = false;
        partyManager = this.transform.parent.GetComponent<PartyManager>();
    }

    void Update()
    {
        if (mapSetUp)
        {
            ScanForPlayerAndAdjacents();
        }
    }

    //TODO - Complete Overhaul as per the Multiple Room Shapes Spec
    public void SetUpMap(Party p)
    {
        party = p;
        GameObject roomHolder = new GameObject();
        roomHolder.transform.SetParent(canvas.transform, false);
        roomHolder.transform.localPosition = new Vector3(0, -50, 0);
        roomHolder.transform.SetAsFirstSibling();
        //Make the Room Buttons ----------------------
        //Positioning (Set Up)
        int buttonwidth = (int)roomButtonPrefab.GetComponent<RectTransform>().rect.width;
        int padding = 5; // Space between Rooms  
        int offsetFromCenterX = ((GameData.tonightsParty.roomGrid.GetLength(0) * buttonwidth) + padding) / 2;
        int offsetFromCenterY = ((GameData.tonightsParty.roomGrid.GetLength(1) * buttonwidth) + padding) / 2;
        GameObject[,] mapButtonGrid = new GameObject[GameData.tonightsParty.roomGrid.GetLength(0), GameData.tonightsParty.roomGrid.GetLength(1)];
        for (int i = 0; i < GameData.tonightsParty.roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < GameData.tonightsParty.roomGrid.GetLength(1); j++)
            {
                if (GameData.tonightsParty.roomGrid[i, j] != null)
                {
                    //Parenting
                    GameObject mapButton = Instantiate(roomButtonPrefab, roomButtonPrefab.transform.position, roomButtonPrefab.transform.rotation) as GameObject;
                    mapButton.transform.SetParent(roomHolder.transform, false);
                    RoomButton roomButton = mapButton.GetComponent<RoomButton>();
                    //Positioning (Actual)
                    mapButton.transform.localPosition = new Vector3(i * (buttonwidth+padding) - offsetFromCenterX, j * (buttonwidth + padding) - offsetFromCenterY, 0);
                    mapButtonGrid[i, j] = mapButton; 
                    //Set the Room that this button represents
                    roomButton.myRoom = GameData.tonightsParty.roomGrid[i, j];
                    roomButton.roomManager = this;
                    if (roomButton.myRoom.entrance)
                    {
                        entranceRoom = roomButton.myRoom; //Setting the Entrance Room in the Room Manager
                    }
                    Debug.Log("Room Created at Grid Square: " + i.ToString() + "," + j.ToString());
                }
            }
        }
        //Putting an Outline around the Map/House
        GameObject houseOutline = Instantiate(houseOutlinePrefab, houseOutlinePrefab.transform.position, houseOutlinePrefab.transform.rotation) as GameObject;
        houseOutline.transform.SetParent(roomHolder.transform, false);
        RectTransform houseOutlineRT = (RectTransform)houseOutline.transform;
        houseOutlineRT.sizeDelta = new Vector2((GameData.tonightsParty.roomGrid.GetLength(0) * buttonwidth) + 10, (GameData.tonightsParty.roomGrid.GetLength(1) * buttonwidth) + 10);
        //float outlineXPos = (mapButtonGrid[0, 0].transform.position.x + mapButtonGrid[mapButtonGrid.GetUpperBound(0), mapButtonGrid.GetUpperBound(1)].transform.position.x) / 2;
        //float outlineYPos = (mapButtonGrid[0, 0].transform.position.y + mapButtonGrid[mapButtonGrid.GetUpperBound(0), mapButtonGrid.GetUpperBound(1)].transform.position.y) / 2;
        houseOutline.transform.localPosition = new Vector3(0, 0, 0);
        houseOutline.transform.SetAsFirstSibling();
        //Map Set Up is complete, notify the rest of the game
        mapSetUp = true;
    }

    public void PlayerMovement(int xPos, int yPos)
    {
        if (party.turnsLeft > 0)
        {
            for (int i = 0; i < party.roomGrid.GetLength(0); i++) // Search via the X Axis
            {
                for (int j = 0; j < party.roomGrid.GetLength(1); j++) //Search via the Y Axis
                {
                    if (party.roomGrid[i, j] != null) //If this room is not null
                    {
                        party.roomGrid[i, j].playerHere = false;
                        party.roomGrid[i, j].playerAdjacent = false;
                    }
                }
            }
            party.roomGrid[xPos, yPos].playerHere = true;
            currentPlayerRoom = party.roomGrid[xPos, yPos];
            if (party.roomGrid[xPos, yPos].punchBowl)
            {
                party.currentPlayerDrinkAmount = party.maxPlayerDrinkAmount;
            } else if (!party.roomGrid[xPos, yPos].cleared && party.currentPlayerDrinkAmount != party.maxPlayerDrinkAmount)
            {
                RandomWineCheck(); // Level 4 Faction Benefit
            }
        }
        else
        {
            Debug.Log("Out of turns. Go home!");
        }
    }

    void RandomWineCheck()
    {
        if(GameData.factionList[party.faction].PlayerReputationLevel() >= 4)
        {
            int randomInt = Random.Range(0, 4);
            if(randomInt == 0)
            {
                party.currentPlayerDrinkAmount = party.maxPlayerDrinkAmount;
                screenFader.gameObject.SendMessage("CreateRandomWineModal", party);
            }
        }
    }

    public void MovePlayerToEntrance()
    {
        for (int i = 0; i < party.roomGrid.GetLength(0); i++) // Search via the X Axis
        {
            for (int j = 0; j < party.roomGrid.GetLength(1); j++) //Search via the Y Axis
            {
                if (party.roomGrid[i, j] != null) //If this room is not null
                {
                    party.roomGrid[i, j].playerHere = false;
                    party.roomGrid[i, j].playerAdjacent = false;
                }
            }
        }
        party.roomGrid[entranceRoom.xPos, entranceRoom.yPos].playerHere = true;
        currentPlayerRoom = party.roomGrid[entranceRoom.xPos, entranceRoom.yPos];
    }

    void ScanForPlayerAndAdjacents()
    {
        for (int i = 0; i < party.roomGrid.GetLength(0); i++) // Search via the X Axis
        {
            for (int j = 0; j < party.roomGrid.GetLength(1); j++) //Search via the Y Axis
            {
                if (party.roomGrid[i, j] != null) //If this room is not null
                {
                    if (party.roomGrid[i, j].playerHere)
                    { //Is the Player is in this Room
                        party.roomGrid[i, j].revealed = true;
                        //North
                        if ((j + 1) < party.roomGrid.GetLength(1)) //Is the North Room on the Map?
                        {
                            Room northRoom = party.roomGrid[i, j + 1];
                            if (northRoom != null)
                            {
                                party.roomGrid[northRoom.xPos, northRoom.yPos].playerAdjacent = true;
                                party.roomGrid[northRoom.xPos, northRoom.yPos].revealed = true;
                            }
                        }
                        //South
                        if ((j - 1) >= 0) //Is the South Room on the Map?
                        {
                            Room southRoom = party.roomGrid[i, j - 1];
                            if (southRoom != null)
                            {
                                party.roomGrid[southRoom.xPos, southRoom.yPos].playerAdjacent = true;
                                party.roomGrid[southRoom.xPos, southRoom.yPos].revealed = true;
                            }
                        }
                        //East
                        if ((i + 1) < party.roomGrid.GetLength(0)) //Is the North Room on the Map?
                        {
                            Room eastRoom = party.roomGrid[i + 1, j];
                            if (eastRoom != null)
                            {
                                party.roomGrid[eastRoom.xPos, eastRoom.yPos].playerAdjacent = true;
                                party.roomGrid[eastRoom.xPos, eastRoom.yPos].revealed = true;
                            }
                        }
                        //West
                        if ((i - 1) >= 0) //Is the North Room on the Map?
                        {
                            Room westRoom = party.roomGrid[i - 1, j];
                            if (westRoom != null)
                            {
                                party.roomGrid[westRoom.xPos, westRoom.yPos].playerAdjacent = true;
                                party.roomGrid[westRoom.xPos, westRoom.yPos].revealed = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void ChoiceModal(int xPos, int yPos)
    {
        if (!currentPlayerRoom.entrance) // Players can freely move through the Entrance Tile
        {
            //Work the Room or Move Through
            int[] intStorage = new int[2];
            intStorage[0] = xPos;
            intStorage[1] = yPos;
            screenFader.gameObject.SendMessage("CreateRoomChoiceModal", intStorage);
        }
    }

    public void WorkTheRoomModal(bool isAmbush)
    {
        if (!currentPlayerRoom.cleared)
        {
            party.turnsLeft--;
            party.currentPlayerIntoxication = Mathf.Clamp(party.currentPlayerIntoxication - 5, 0, party.maxPlayerIntoxication);
            //Work the Room!
            object[] objectStorage = new object[3];
            objectStorage[0] = currentPlayerRoom;
            objectStorage[1] = isAmbush;
            objectStorage[2] = this;
            screenFader.gameObject.SendMessage("CreateWorkTheRoomModal", objectStorage);
        }
        else
        {
            Debug.Log("Can't Work a Cleared Room");
        }
    }

    public void WorkTheHostModal()
    {
        if (!currentPlayerRoom.cleared)
        {
            party.turnsLeft--;
            party.currentPlayerIntoxication = Mathf.Clamp(party.currentPlayerIntoxication - 5, 0, party.maxPlayerIntoxication);
            //Work the Host!
            object[] objectStorage = new object[2];
            objectStorage[0] = currentPlayerRoom;
            objectStorage[1] = this;
            screenFader.gameObject.SendMessage("CreateWorkTheHostModal", objectStorage);
        }
        else
        {
            Debug.Log("Can't Work a Cleared Host");
        }
    }

    public void MoveThrough()
    {
        if (!currentPlayerRoom.hostHere)
        {
            int checkValue = Random.Range(0, 100);
            string[] stringStorage = new string[1];
            stringStorage[0] = currentPlayerRoom.name;
            int moveThroughChance = currentPlayerRoom.MoveThroughChance();
            //Is the Player using the Cane Accessory? If so then increase the chance to Move Through by 10%!
            if (GameData.tonightsParty.playerAccessory != null)
            {
                if (GameData.tonightsParty.playerAccessory.Type() == "Cane")
                {
                    moveThroughChance += 10;
                }
            }
            if (checkValue <= moveThroughChance) //The Player Moves through
            {
                screenFader.gameObject.SendMessage("CreateMovedThroughModal", stringStorage);
            }
            else // The Player fails to Move Through and is Ambushed!
            {
                screenFader.gameObject.SendMessage("CreateAmbushedModal", stringStorage);
            }
        }
        else
        {
            Debug.Log("Can't Move Through the Host");
        }
    }

    public void PartyEventModal(int xPos, int yPos)
    {
        if(!party.roomGrid[xPos, yPos].cleared) //If the Room hasn't been cleared already, do all the stuff
        {
            //Standdard Turn Stuff
            party.turnsLeft--;
            party.currentPlayerIntoxication = Mathf.Clamp(party.currentPlayerIntoxication - 20, 0, party.maxPlayerIntoxication);
            //Make the Event Happen
            screenFader.gameObject.SendMessage("CreateEventPopUp", "party");
            //Clear the Room
            party.roomGrid[xPos, yPos].cleared = true;
        }
    }
}