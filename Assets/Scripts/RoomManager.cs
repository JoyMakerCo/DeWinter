﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

public class RoomManager : MonoBehaviour
{
	private const int PADDING = 5;

    public PartyManager partyManager;
    public GameObject roomButtonPrefab;
    public GameObject houseOutlinePrefab; //This is used to Outline the house when it's done
    public Canvas canvas; //New Interface objects need to get parented to this in order to work
    public GameObject screenFader;
    public GameObject roomHolder;

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

	public MapVO Map
	{
		get { return _model.Map; }
	}

    void Start()
    {
		_model = DeWinterApp.GetModel<MapModel>();
		_partyModel = DeWinterApp.GetModel<PartyModel>();
        partyManager = this.transform.parent.GetComponent<PartyManager>();
		DeWinterApp.SendMessage<Party>(MapMessage.GENERATE_MAP, _partyModel.Party);
		DeWinterApp.Subscribe<Party>(PartyConstants.SHOW_DRINK_MODAL, handleDrinkModal);
		DeWinterApp.Subscribe<RoomVO>(HandleRoom);
		DrawMap();
		currentPlayerRoom = Map.Entrance;
    }

	private void HandleRoom(RoomVO room)
	{
		foreach (RoomVO mapRoom in Map.Rooms)
		{
			if (mapRoom != null)
				_buttons[mapRoom].SetCurrentRoom(room);
		}

		if (room.HostHere)
			WorkTheHostModal();
		else
			WorkTheRoomModal(false);
	}

	private void DrawMap()
    {
        //Make the Room Buttons ----------------------
        //Positioning (Set Up)
		int buttonWidth = (int)roomButtonPrefab.GetComponent<RectTransform>().rect.width;
		_buttons = new Dictionary<RoomVO, RoomButton>();

		//Putting an Outline around the Map/House
        GameObject houseOutline = Instantiate<GameObject>(houseOutlinePrefab);
        houseOutline.transform.SetParent(roomHolder.transform, false);
		((RectTransform)houseOutline.transform).sizeDelta = new Vector2(((_model.Map.Width + PADDING)*buttonWidth), ((_model.Map.Depth + PADDING)*buttonWidth));

        //Map Set Up is complete, notify the rest of the game
		foreach (RoomVO rm in Map.Rooms)
		{
			DrawRoom(rm, buttonWidth);
		}
	}

	private void DrawRoom(RoomVO room, int buttonWidth)
	{
		if (room != null && room.Shape != null)
		{
			GameObject mapButton = Instantiate(roomButtonPrefab) as GameObject;
			RoomButton roomButton = mapButton.GetComponent<RoomButton>();
			mapButton.transform.SetParent(roomHolder.transform, false);
			mapButton.transform.localPosition = new Vector3((room.Shape[0].x-(Map.Width>>1))*(buttonWidth + PADDING), (room.Shape[0].y - (Map.Depth>>1))*(PADDING + buttonWidth), 0);
			roomButton.Room = room;
			_buttons.Add(room, roomButton);
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
            if (GameData.partyAccessory != null)
            {
				if (GameData.partyAccessory.Type == "Cane")
                {
                    moveThroughChance += 10;
                }
            }
            if (checkValue <= moveThroughChance) //The Player Moves through
            {
            	Dictionary<string, string> subs = new Dictionary<string, string>()
					{{"$ROOMNAME",currentPlayerRoom.Name}};
            	DeWinterApp.OpenMessageDialog(DialogConsts.MOVED_THROUGH_DIALOG, subs);
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
    	Dictionary<string, string> subs = new Dictionary<string, string>(){
    		{"$HOSTNAME",p.host.Name}};
    	DeWinterApp.OpenMessageDialog(DialogConsts.REFILL_WINE_DIALOG, subs);
    }
}