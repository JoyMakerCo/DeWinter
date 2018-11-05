using System;
using UnityEngine;

namespace Ambition
{
    public class TutorialRoomButtonMediator : MonoBehaviour
    {
        public string RoomName;
        private RoomVO _room;

        private void Awake()
        {
            MapModel map = AmbitionApp.GetModel<MapModel>();
            _room = Array.Find(map.Map.Rooms, r => r.Name == RoomName);
            if (_room != null)
            {
                gameObject.SetActive(false);
                AmbitionApp.Subscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
                HandleRoom(map.Room);
            }
            else Destroy(gameObject);
        }

        private void HandleRoom(RoomVO room)
        {
            if (room == _room) Destroy(gameObject);
            else
            {
                bool active = _room.IsAdjacentTo(room);
                if (active) AmbitionApp.Subscribe(GameMessages.FADE_IN, HandleReveal);
                else
                {
                    gameObject.SetActive(false);
                    AmbitionApp.Unsubscribe(GameMessages.FADE_IN, HandleReveal);
                }
            }
        }

        private void HandleReveal()
        {
            gameObject.SetActive(true);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN, HandleReveal);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN, HandleReveal);
        }
    }
}
