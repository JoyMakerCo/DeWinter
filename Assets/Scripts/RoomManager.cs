using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

public class RoomManager : MonoBehaviour
{
// TODO: Refine room drawing
private const int PADDING = 5;

    public PartyManager partyManager;
    public MapVO Map = null; //Whichever Room is the Entrance
    public GameObject roomButtonPrefab;
    public GameObject houseOutlinePrefab; //This is used to Outline the house when it's done
    public Canvas canvas; //New Interface objects need to get parented to this in order to work
    public GameObject screenFader;
    public GameObject roomHolder;

    private int _buttonWidth;
    private Dictionary<RoomVO, RoomButton> _buttons;

    //Images for the Map Buttons
    public Sprite mapBlock2x2;
    public Sprite mapBlock2x3;
    public Sprite mapBlock2x4;
    public Sprite mapBlock3x3;
    public Sprite mapBlock4x3T;
    public Sprite mapBlock4x4L;

    private MapModel _model;
	private PartyModel _partyModel;

	public RoomVO currentPlayerRoom
	{
		get { return _model.Room; }
		set { _model.Room = value; }
	}

    void Start()
    {
		_model = DeWinterApp.GetModel<MapModel>();
		_partyModel = DeWinterApp.GetModel<PartyModel>();
        partyManager = this.transform.parent.GetComponent<PartyManager>();
        DeWinterApp.Subscribe<MapVO>(HandleMap);
		DeWinterApp.Subscribe<Party>(PartyConstants.SHOW_DRINK_MODAL, handleDrinkModal);
    }

	private void HandleMap(MapVO map)
    {
		GameObject roomHolder = new GameObject();
		roomHolder.transform.SetParent(canvas.transform, false);
        roomHolder.transform.SetAsFirstSibling();
		Map = map;

        //Make the Room Buttons ----------------------
        //Positioning (Set Up)
		_buttonWidth = (int)roomButtonPrefab.GetComponent<RectTransform>().rect.width;
		_buttons = new Dictionary<RoomVO, RoomButton>();
		foreach (RoomVO rm in map.Rooms)
		{
			if (rm != null)
				DrawRoom(rm);
		}
	}

	private void DrawRoom(RoomVO room)
	{
		if (room != null) return;
		if (room.Shape != null)
		{
// TODO: Make room drawing more sophisticated
			GameObject mapButton = Instantiate(roomButtonPrefab, roomButtonPrefab.transform.position, roomButtonPrefab.transform.rotation) as GameObject;
			RoomButton roomButton = mapButton.GetComponent<RoomButton>();
			roomButton.Room = room;
			mapButton.transform.SetParent(roomHolder.transform, false);
			mapButton.transform.localPosition = new Vector3((room.Shape[0].x + PADDING)*_buttonWidth, (room.Shape[0].y + PADDING)*_buttonWidth, 0);
			_buttons.Add(room, roomButton);
		}

        //Putting an Outline around the Map/House
        GameObject houseOutline = Instantiate(houseOutlinePrefab, houseOutlinePrefab.transform.position, houseOutlinePrefab.transform.rotation) as GameObject;
        houseOutline.transform.SetParent(roomHolder.transform, false);
        RectTransform houseOutlineRT = (RectTransform)houseOutline.transform;
        houseOutlineRT.sizeDelta = new Vector2((_model.Map.Width * _buttonWidth) + 10, (_model.Map.Depth * _buttonWidth) + 10);
        //float outlineXPos = (mapButtonGrid[0, 0].transform.position.x + mapButtonGrid[mapButtonGrid.GetUpperBound(0), mapButtonGrid.GetUpperBound(1)].transform.position.x) / 2;
        //float outlineYPos = (mapButtonGrid[0, 0].transform.position.y + mapButtonGrid[mapButtonGrid.GetUpperBound(0), mapButtonGrid.GetUpperBound(1)].transform.position.y) / 2;
        houseOutline.transform.localPosition = new Vector3(0, 0, 0);
        houseOutline.transform.SetAsFirstSibling();
        //Map Set Up is complete, notify the rest of the game
    }

    public void PlayerMovement(int xPos, int yPos)
    {
        if (_partyModel.Party.turnsLeft > 0)
        {
            currentPlayerRoom = _model.Map.Rooms[xPos, yPos];
			if (Array.IndexOf(currentPlayerRoom.Features, PartyConstants.PUNCHBOWL) >= 0)
            {
				_partyModel.Party.currentPlayerDrinkAmount = _partyModel.Party.maxPlayerDrinkAmount;
			} else if (!_model.Map.Rooms[xPos, yPos].Cleared && _partyModel.Party.currentPlayerDrinkAmount < _partyModel.Party.maxPlayerDrinkAmount)
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
		if(GameData.factionList[_partyModel.Party.faction].ReputationLevel >= 5)
        {
			if(UnityEngine.Random.Range(0, 4) == 0)
            {
				_partyModel.Party.currentPlayerDrinkAmount = _partyModel.Party.maxPlayerDrinkAmount;
				screenFader.gameObject.SendMessage("CreateRandomWineModal", _partyModel.Party);
            }
        }
    }

    public void MovePlayerToEntrance()
    {
		currentPlayerRoom = (_model.Map != null) ? _model.Map.Entrance : null;
    }

    public void ChoiceModal(int xPos, int yPos)
    {
        if (!_model.Room.IsEntrance) // Players can freely move through the Entrance Tile
	    {
	        Debug.Log("No Move Through:" + currentPlayerRoom.IsImpassible);
	        Debug.Log("Cleared:" + currentPlayerRoom.Cleared);
	        if (!currentPlayerRoom.IsImpassible || !currentPlayerRoom.Cleared) 
	        {   
	            //Work the Room or Move Through
	            int[] intStorage = new int[2]{ xPos, yPos };
	            screenFader.gameObject.SendMessage("CreateRoomChoiceModal", intStorage);
	        }
	    }
   	}

    public void WorkTheRoomModal(bool isAmbush)
    {
		if (!currentPlayerRoom.Cleared)
        {
			_partyModel.Party.turnsLeft--;
			SoberUp(5);
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
        if (!currentPlayerRoom.Cleared)
        {
			_partyModel.Party.turnsLeft--;
			SoberUp(5);
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
        if (!currentPlayerRoom.HostHere && !currentPlayerRoom.IsImpassible) //If this is not a Host Room and the Player is allowed to Move Through this Room
        {
            int checkValue = UnityEngine.Random.Range(0, 100);
            string[] stringStorage = new string[1];
            stringStorage[0] = currentPlayerRoom.Name;
            int moveThroughChance = currentPlayerRoom.MoveThroughChance;
            //Is the Player using the Cane Accessory? If so then increase the chance to Move Through by 10%!
            if (GameData.tonightsParty.playerAccessory != null)
            {
                if (GameData.tonightsParty.playerAccessory.Type == "Cane")
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

    public void PartyEventModal(RoomVO room)
    {
        if(!room.Cleared) //If the Room hasn't been cleared already, do all the stuff
        {
            //Standdard Turn Stuff
			_partyModel.Party.turnsLeft--;
            SoberUp(20);
            //Make the Event Happen
            screenFader.gameObject.SendMessage("CreateEventPopUp", "party");
            //Clear the Room
            room.Cleared = true;
        }
    }

    private void SoberUp(int amount)
    {
		_partyModel.Party.currentPlayerIntoxication -= amount;
		if (_partyModel.Party.currentPlayerIntoxication<0)
			_partyModel.Party.currentPlayerIntoxication=0;
    }

    private void handleDrinkModal(Party p)
    {
		screenFader.gameObject.SendMessage("CreateRandomWineModal", p);
    }
}