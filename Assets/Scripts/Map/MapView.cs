using System;
using System.Linq;
using UnityEngine;
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

        public MapVO CreateMapVO()
        {
            return new MapVO()
            {
                Name = gameObject.name,
                AssetID = gameObject.name,
                Faction = this.Faction,
                Tags = Tags?.ToArray(),
                Size = this.Size,
            };
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<IncidentVO[]>(PartyMessages.SELECT_INCIDENTS, HandleIncidents);
        }

        private void HandleIncidents(IncidentVO[] incidents)
        {
            // Shuffle
            int index;
            IncidentVO[] results = new IncidentVO[_rooms.Length];
            Array.Copy(incidents, results, incidents.Length);
            int count = results.Length;
            for (int i = 0; i < count; i++)
            {
                index = Util.RNG.Generate(i, count);
                _rooms[i].SetIncident(results[index]);
                results[index] = results[i];
            }
        }
    }
}
