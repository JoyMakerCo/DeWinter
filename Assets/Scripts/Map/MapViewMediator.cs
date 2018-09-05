using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class MapViewMediator : MonoBehaviour
	{
		private const int PAN_TOLERTANCE = 50;
		private const int TILES_PER_SECOND=10;
	    public GameObject roomButtonPrefab;

	    private Dictionary<RoomVO, RoomButton> _buttons;

	    private MapModel _model;
		private PartyModel _partyModel;
        private Vector4 _bounds;

		public MapVO Map
		{
			get { return _model.Map; }
		}

		void Awake()
		{
			_model = AmbitionApp.GetModel<MapModel>();
			_partyModel = AmbitionApp.GetModel<PartyModel>();
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
            //Make the Room Buttons ----------------------
            _bounds = Vector4.zero;
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
                float d = Time.deltaTime * _model.MapScale * TILES_PER_SECOND;
                if (Input.mousePosition.x < PAN_TOLERTANCE)
                    offset.x = d;
                else if (Input.mousePosition.x > Screen.width - PAN_TOLERTANCE)
                    offset.x = -d;
                else if (Input.GetKey(KeyCode.LeftArrow)) offset.x = d * .5f;
                else if (Input.GetKey(KeyCode.RightArrow)) offset.x = -d * .5f;

                if (Input.mousePosition.y < PAN_TOLERTANCE)
                    offset.y = d;
                else if (Input.mousePosition.y > Screen.height - PAN_TOLERTANCE)
                    offset.y = -d;
                else if (Input.GetKey(KeyCode.UpArrow)) offset.y = -d * .5f;
                else if (Input.GetKey(KeyCode.DownArrow)) offset.y = d * .5f;

                offset += transform.localPosition;

                if (offset.x < _bounds[0] || offset.x > _bounds[2])
                    offset.x = transform.localPosition.x;
                if (offset.y < _bounds[1] || offset.y > _bounds[3])
                    offset.y = transform.localPosition.y;

                transform.localPosition = offset;
            }
        }

	    private void Recenter()
	    {
            if (Map != null)
            {
                RoomVO room = _model.Room ?? Map.Entrance;
                if (room != null)
                {
                    Vector3 center;
                    int[] bounds = room.Bounds;
                    float q = -.5f * _model.MapScale;
                    center.x = (bounds[0] + bounds[2]) * q;
                    center.y = (bounds[1] + bounds[3]) * q;
                    center.z = 0f;
                    transform.localPosition = center;
                }
            }
	    }

		private void DrawRoom(RoomVO room)
		{
			if (room != null)
			{
				GameObject mapButton = Instantiate<GameObject>(roomButtonPrefab, gameObject.transform) as GameObject;
				RoomButton roomButton = mapButton.GetComponent<RoomButton>();
                Vector4 bounds = new Vector4()
                {
                    w = room.Bounds[0] * _model.MapScale,
                    x = room.Bounds[1] * _model.MapScale,
                    y = room.Bounds[2] * _model.MapScale,
                    z = room.Bounds[3] * _model.MapScale
                };
                mapButton.transform.SetAsFirstSibling();
				roomButton.Room = room;
				if (bounds[0] < _bounds[0]) _bounds[0] = bounds[0];
				if (bounds[1] < _bounds[1]) _bounds[1] = bounds[1];
				if (bounds[2] > _bounds[2]) _bounds[2] = bounds[2];
				if (bounds[3] > _bounds[3]) _bounds[3] = bounds[3];
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
