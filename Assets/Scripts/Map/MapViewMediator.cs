using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class MapViewMediator : MonoBehaviour
	{
		private const int PAN_TOLERTANCE = 50;
		private const int TILES_PER_SECOND = 20;

	    public GameObject roomButtonPrefab;

	    private Dictionary<RoomVO, RoomButton> _buttons;

	    private MapModel _model;
		private PartyModel _partyModel;
		private Vector3 _center;

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

			//Make the Room Buttons ----------------------
	        //Positioning (Set Up)
			_buttons = new Dictionary<RoomVO, RoomButton>();

	        //Map Set Up is complete, notify the rest of the game
	        Array.ForEach(Map.Rooms, DrawRoom);
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
			HandleRoom(Map.Entrance);
	    }

	    void Update()
	    {
	    	if (Input.GetKey(KeyCode.Space))
	    		Recenter();
			else {
				Vector3 offset = transform.localPosition;
				float d = _model.MapScale*Time.deltaTime*TILES_PER_SECOND;
				if (Input.GetKey(KeyCode.UpArrow)) offset[1] -= d*.5f;
				else if (Input.GetKey(KeyCode.DownArrow)) offset[1] += d*.5f;
				if (Input.GetKey(KeyCode.LeftArrow)) offset[0] += d*.5f;
				else if (Input.GetKey(KeyCode.RightArrow)) offset[0] -= d*.5f;
				if (Input.mousePosition.x < PAN_TOLERTANCE)
					offset[0] += d;
				else if (Input.mousePosition.x > Screen.width-PAN_TOLERTANCE)
					offset[0] -= d;
				if (Input.mousePosition.y < PAN_TOLERTANCE)
					offset[1] += d;
				else if (Input.mousePosition.y > Screen.height-PAN_TOLERTANCE)
					offset[1] -= d;

				transform.localPosition = offset;
			}
		}

	    private void Recenter()
	    {
			transform.localPosition = _center;
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

			if (room != null)
			{
				int [] bounds = room.Bounds;
				float q = -.5f*_model.MapScale;
				_center.x = (bounds[0]+bounds[2])*q;
				_center.y = (bounds[1]+bounds[3])*q;
				_center.z = 0f;
			}
			else
			{
				_center = Vector3.zero;
			}
			Recenter();
		}
	}
}
