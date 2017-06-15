using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class MapViewMediator : MonoBehaviour
	{
		private const int PADDING = 5;

	    public GameObject roomButtonPrefab;
	    public GameObject houseOutlinePrefab; //This is used to Outline the house when it's done

	    private Dictionary<RoomVO, RoomButton> _buttons;

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

		void Awake()
		{
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RoomVO>(HandleRoom);
			_buttons.Clear();
			_buttons = null;
		}

	    void Start()
	    {
			_model = AmbitionApp.GetModel<MapModel>();
			_partyModel = AmbitionApp.GetModel<PartyModel>();
			AmbitionApp.SendMessage<PartyVO>(MapMessage.GENERATE_MAP, _partyModel.Party);
			DrawMap();
			currentPlayerRoom = Map.Entrance;
	    }

		private void HandleRoom(RoomVO room)
		{
			if (_buttons != null)
			{
				foreach (RoomVO mapRoom in Map.Rooms)
				{
					if (mapRoom != null)
						_buttons[mapRoom].SetCurrentRoom(room);
				}
			}
		}

		private void DrawMap()
	    {
	        //Make the Room Buttons ----------------------
	        //Positioning (Set Up)
			int buttonWidth = (int)roomButtonPrefab.GetComponent<RectTransform>().rect.width;
			_buttons = new Dictionary<RoomVO, RoomButton>();

			//Putting an Outline around the Map/House
	        GameObject houseOutline = Instantiate<GameObject>(houseOutlinePrefab);
	        houseOutline.transform.SetParent(gameObject.transform, false);
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
				mapButton.transform.SetParent(gameObject.transform, false);
				mapButton.transform.localPosition = new Vector3((room.Shape[0].x-(Map.Width>>1))*(buttonWidth + PADDING), (room.Shape[0].y - (Map.Depth>>1))*(PADDING + buttonWidth), 0);
				roomButton.Room = room;
				_buttons.Add(room, roomButton);
			}
	    }

	    public void MovePlayerToEntrance()
	    {
			currentPlayerRoom = (_model.Map != null) ? _model.Map.Entrance : null;
	    }

	    public void PartyEventModal(RoomVO room)
	    {
	        if(!room.Cleared) //If the Room hasn't been cleared already, do all the stuff
	        {
	            //Standdard Turn Stuff
				AmbitionApp.AdjustValue<int>(PartyConstants.TURNSLEFT, -1);
				AmbitionApp.AdjustValue<int>(GameConsts.INTOXICATION, -20);
	            // TODO: Make the Event Happen
//	            screenFader.gameObject.SendMessage("CreateEventPopUp", "party");
	            //Clear the Room
	            room.Cleared = true;
	        }
	    }
	}
}
