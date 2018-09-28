using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class MapViewMediator : MonoBehaviour
	{
		private const int PAN_TOLERTANCE = 50;
		private const int TILES_PER_SECOND = 50;
	    public GameObject roomButtonPrefab;

	    private Dictionary<RoomVO, RoomButton> _buttons;

	    private MapModel _model;
        private Vector4 _bounds;
        private Vector3 _offset;

		public MapVO Map
		{
			get { return _model.Map; }
		}

		void Awake()
		{
			_model = AmbitionApp.GetModel<MapModel>();
			_buttons = new Dictionary<RoomVO, RoomButton>();
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, Recenter);
            AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, Lock);
            AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, Unlock);
		}

 		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RoomVO>(HandleRoom);
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, Recenter);
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, Lock);
            AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_CLOSED, Unlock);
			_buttons.Clear();
			_buttons = null;
		}

        void Start()
        {
            _bounds = Vector4.zero;
            foreach (RoomVO room in Map.Rooms)
            {
                if (room.Bounds[0] < _bounds[0]) _bounds[0] = room.Bounds[0];
                if (room.Bounds[1] < _bounds[1]) _bounds[1] = room.Bounds[1];
                if (room.Bounds[2] > _bounds[2]) _bounds[2] = room.Bounds[2];
                if (room.Bounds[3] > _bounds[3]) _bounds[3] = room.Bounds[3];
            }

            //Make the Room Buttons ----------------------
            Array.ForEach(Map.Rooms, DrawRoom);
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
			HandleRoom(Map.Entrance);
	    }

        private void Lock()
        {
            enabled = false;
        }
		
        private void Unlock(string DialogID)
        {
            enabled = DialogID == "END_CONVERSATION" || DialogID == "DEFEAT";
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Recenter();
            }
            else if (!float.IsNaN(transform.position.sqrMagnitude))
            {
                Vector3 offset = Vector3.zero;
                float d = Time.deltaTime * TILES_PER_SECOND;
                if (Input.mousePosition.x < PAN_TOLERTANCE)
                    offset.x = d;
                else if (Input.mousePosition.x > Screen.width - PAN_TOLERTANCE)
                    offset.x = -d;
                else if (Input.GetKey(KeyCode.LeftArrow)) offset.x = d;
                else if (Input.GetKey(KeyCode.RightArrow)) offset.x = -d;

                if (Input.mousePosition.y < PAN_TOLERTANCE)
                    offset.y = d;
                else if (Input.mousePosition.y > Screen.height - PAN_TOLERTANCE)
                    offset.y = -d;
                else if (Input.GetKey(KeyCode.UpArrow)) offset.y = -d;
                else if (Input.GetKey(KeyCode.DownArrow)) offset.y = d;

                _offset += offset;

                if (_offset.x < -_bounds[2] || _offset.x > -_bounds[0])
                    _offset.x -= offset.x;
                if (_offset.y < -_bounds[3] || _offset.y > -_bounds[1])
                    _offset.y -= offset.y;

                transform.localPosition = _offset * _model.MapScale;
                transform.localPosition = _offset*_model.MapScale;
            }
        }

	    private void Recenter()
	    {
            if (Map != null)
            {
                RoomVO room = _model.Room ?? Map.Entrance;
                if (room != null)
                {
                    int[] bounds = room.Bounds;
                    _offset.x = (bounds[0] + bounds[2]) * -.5f;
                    _offset.y = (bounds[1] + bounds[3]) * -.5f;
                    _offset.z = 0f;
                    transform.localPosition = _offset * _model.MapScale;
                }
            }
	    }

	    private void DrawRoom(RoomVO room)
		{
			if (room != null)
			{
				GameObject mapButton = Instantiate(roomButtonPrefab, gameObject.transform);
				RoomButton roomButton = mapButton.GetComponent<RoomButton>();
                mapButton.transform.SetAsFirstSibling();
				roomButton.Room = room;
				_buttons.Add(room, roomButton);
			}

	    }

		private void HandleRoom(RoomVO room)
		{
			foreach(KeyValuePair<RoomVO, RoomButton> kvp in _buttons)
			{
				kvp.Value.UpdatePlayerRoom(room);
			}
		}
	}
}
