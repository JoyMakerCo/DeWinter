using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class MapViewMediator : MonoBehaviour
	{
		private const int PAN_TOLERTANCE = 50;
		private const float PAN_VELOCITY = .005f;

	    public GameObject roomButtonPrefab;

	    private Dictionary<RoomVO, RoomButton> _buttons;

	    private MapModel _model;
		private PartyModel _partyModel;
		private RectTransform _rect;

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
			_rect = GetComponent<RectTransform>();
			_model = AmbitionApp.GetModel<MapModel>();
			_partyModel = AmbitionApp.GetModel<PartyModel>();
			AmbitionApp.SendMessage<PartyVO>(MapMessage.GENERATE_MAP, _partyModel.Party);

			//Make the Room Buttons ----------------------
	        //Positioning (Set Up)
			_buttons = new Dictionary<RoomVO, RoomButton>();

	        //Map Set Up is complete, notify the rest of the game
	        Array.ForEach(Map.Rooms, DrawRoom);
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
			currentPlayerRoom = Map.Entrance;
			Recenter();
	    }

	    void Update()
	    {
	    	if (Input.GetKey(KeyCode.Space))
	    		Recenter();
	    	else
	    	{
		    	Vector2 offset = _rect.pivot;

		    	if (Input.mousePosition.x < PAN_TOLERTANCE)
					offset[0] -= _model.MapScale*PAN_VELOCITY;
				else if (Input.mousePosition.x > Screen.width-PAN_TOLERTANCE)
					offset[0] += _model.MapScale*PAN_VELOCITY;

				if (Input.mousePosition.y < PAN_TOLERTANCE)
					offset[1] -= _model.MapScale*PAN_VELOCITY;
				else if (Input.mousePosition.y > Screen.height-PAN_TOLERTANCE)
					offset[1] += _model.MapScale*PAN_VELOCITY;

				_rect.pivot = offset;
			}
	    }

	    private void Recenter()
	    {
// TODO: Figure Out why logical recenter doesn't work
			// RoomButton btn;
			// if (currentPlayerRoom != null && _buttons.TryGetValue(currentPlayerRoom, out btn))
			// {
			// 	_rect.pivot = -btn.GetComponent<RectTransform>().rect.center;
	    	// }
	 		// else
			 {
				_rect.pivot = new Vector2();
			 }
	    }

		private void DrawRoom(RoomVO room)
		{
			if (room != null)
			{
				GameObject mapButton = Instantiate<GameObject>(roomButtonPrefab, gameObject.transform) as GameObject;
				RoomButton roomButton = mapButton.GetComponent<RoomButton>();
				mapButton.transform.SetAsFirstSibling();
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
				AmbitionApp.GetModel<PartyModel>().TurnsLeft--;
				AmbitionApp.GetModel<PartyModel>().Intoxication-=20;

	            // TODO: Make the Event Happen
//	            screenFader.gameObject.SendMessage("CreateEventPopUp", "party");
	            //Clear the Room
	            room.Cleared = true;
	        }
	    }

		private void HandleRoom(RoomVO room)
		{
			if (_buttons != null)
			{
				foreach(KeyValuePair<RoomVO, RoomButton> kvp in _buttons)
				{
					kvp.Value.UpdatePlayerRoom(room);
				}
			}
		}
	}
}
