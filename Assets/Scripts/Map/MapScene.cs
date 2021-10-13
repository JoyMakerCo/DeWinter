using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Ambition
{
    public class MapScene : MonoBehaviour
    {
        public AvatarCollection Avatars;
        public PrefabMap[] Maps;

        private RoomView[] _rooms;

        public MapView Map { get; private set; }

        public Sprite GetPortrait(string avatarID) => Avatars?.GetAvatar(avatarID).Portrait;

        private void Awake()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            if (Map == null) Map = model.LoadMap(transform.transform);
            if (Map == null)
            {
                PrefabMap prefab = Array.Find(Maps, m => m.ID.ToLower() == model.Party.Faction.ToString().ToLower());
                if (prefab.Prefab == null) prefab = RNG.TakeRandom(Maps);
                GameObject obj = Instantiate<GameObject>(prefab.Prefab, transform.transform);
                Map = obj.GetComponent<MapView>();
            }
            if (Map != null)
            {
                _rooms = Map.GetComponentsInChildren<RoomView>();
                model.NumRooms = _rooms.Length;
                AmbitionApp.Subscribe<IncidentVO[]>(PartyMessages.SELECT_INCIDENTS, HandleIncidents);
                if (!string.IsNullOrEmpty(Map.Music.Name))
                {
                    AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, Map.Music.Name);
                }
            }
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<IncidentVO[]>(PartyMessages.SELECT_INCIDENTS, HandleIncidents);
        }

        private void HandleIncidents(IncidentVO[] incidents)
        {
            for (int i = _rooms.Length - 1; i >= 0; --i)
            {
                _rooms[i].SetIncident(incidents[i]);
            }
        }
    }
}
