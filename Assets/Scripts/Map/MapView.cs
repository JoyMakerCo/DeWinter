using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
namespace Ambition
{
    public class MapView : MonoBehaviour
    {
        public FactionType Faction = FactionType.Neutral;
        public string[] Tags;
        public PartySize Size;

        private RoomView[] _rooms;

        private void Awake()
        {
            _rooms = transform.GetComponentsInChildren<RoomView>();
            AmbitionApp.Subscribe<IncidentVO[]>(PartyMessages.SELECT_INCIDENTS, HandleIncidents);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<IncidentVO[]>(PartyMessages.SELECT_INCIDENTS, HandleIncidents);
        }

        private void HandleIncidents(IncidentVO[] incidents)
        {
            for (int i=_rooms.Length-1; i>=0; --i)
            {
                _rooms[i].SetIncident(incidents[i]);
            }
        }
    }
}
