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
        private const float RECENTER_TIME = .5f;
        public GameObject roomButtonPrefab;

	    private Dictionary<RoomVO, RoomButtonDeprecated> _buttons;

        private Vector4 _bounds;
        private Vector3 _offset;

		void Awake()
		{
			_buttons = new Dictionary<RoomVO, RoomButtonDeprecated>();
            //AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, Recenter);
            AmbitionApp.Subscribe(GameMessages.FADE_IN_COMPLETE, Unlock);
		}

 		void OnDestroy()
		{
            //AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, Recenter);
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT, Lock);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN_COMPLETE, Unlock);
            _buttons.Clear();
			_buttons = null;
		}

        void Start()
        {
            _bounds = Vector4.zero;
/*            foreach (RoomVO room in Map.Rooms)
            {
                if (room.Bounds[0] < _bounds[0]) _bounds[0] = room.Bounds[0];
                if (room.Bounds[1] < _bounds[1]) _bounds[1] = room.Bounds[1];
                if (room.Bounds[2] > _bounds[2]) _bounds[2] = room.Bounds[2];
                if (room.Bounds[3] > _bounds[3]) _bounds[3] = room.Bounds[3];
            }
*/
            //Make the Room Buttons ----------------------
            //Array.ForEach(Map.Rooms, DrawRoom);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, Lock);
            Lock();
        }

        private void Lock()
        {
            enabled = false;
        }
		
        private void Unlock()
        {
            enabled = gameObject.activeInHierarchy;
        }

/*DEPRECATED        void Update()
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
                int[] bounds = room.Bounds;
                Vector3 vec = transform.localPosition;
                _offset = new Vector3(
                    (bounds[0] + bounds[2]) * -.5f,
                    (bounds[1] + bounds[3]) * -.5f,
                    0f);
                transform.localPosition = _offset* _model.MapScale;
                //StartCoroutine(RecenterMap(_model.Room ?? Map.Entrance));
            }
         }

        */
	    private void DrawRoom(RoomVO room)
		{
			if (room != null)
			{
				GameObject mapButton = Instantiate(roomButtonPrefab, gameObject.transform);
                RoomButtonDeprecated roomButton = mapButton.GetComponent<RoomButtonDeprecated>();
                mapButton.transform.SetAsFirstSibling();
				roomButton.Room = room;
				_buttons.Add(room, roomButton);
			}
	    }
/*
        IEnumerator RecenterMap(RoomVO room)
        {
            int[] bounds = room.Bounds;
            Vector3 vec = transform.localPosition;
            _offset = new Vector3(
                (bounds[0] + bounds[2]) * -.5f,
                (bounds[1] + bounds[3]) * -.5f,
                0f);
            for (float t = 0f; t < RECENTER_TIME; t+=Time.deltaTime)
            {
                vec = (vec * _model.MapScale + transform.localPosition) * .5f;
                transform.localPosition = vec;
                yield return null;
            }
            transform.localPosition = _offset* _model.MapScale;
        }
*/
    }
}
