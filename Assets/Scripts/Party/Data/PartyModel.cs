using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using UnityEngine;
using Util;

namespace Ambition
{
    [Saveable]
    public class PartyModel : Model, IInitializable, IResettable
    {
        // PUBLIC DATA //////////////

        [JsonProperty("turns")]
        public int Turns;

        [JsonProperty("turn")]
        public int Turn = -1;

        [JsonIgnore]
        public int NumRooms;

        [JsonIgnore]
        public int[] NumTurnsByPartySize;

        [JsonIgnore]
        public int TurnsLeft => Turns - Turn;

        [JsonProperty("incidents")]
        public string[] Incidents;

        [JsonIgnore]
        public string PartyID => Party?.ID;

        [JsonProperty("rewards")]
        public List<CommodityVO> Rewards = new List<CommodityVO>(); // Rewards for the current party

        [JsonIgnore]
        public int BaseNoveltyLoss;

        [JsonIgnore]
        public int CumulativeNoveltyLoss;

        [JsonIgnore]
        public string RequiredIncident => (_incidentIndex < (Party?.RequiredIncidents?.Length ?? 0))
            ? Party.RequiredIncidents[_incidentIndex]
            : null;

        [JsonIgnore]
        public PartyVO Party
        {
            get
            {
                PartyVO[] parties = _calendar.GetOccasions(_calendar.Today);
                return Array.Find(parties, p => p.IsAttending);
            }
        }
        [JsonIgnore]
        public int IgnoreInvitationPenalty;

        [JsonIgnore]
        public int AcceptInvitationBonus;

        // PRIVATE DATA //////////////
        [JsonProperty("incident_index")]
        private int _incidentIndex = 0;

        [JsonProperty("calendar")]
        private PartyCalendar _calendar = new PartyCalendar();

        [JsonIgnore]
        private Dictionary<string, GameObject> _maps = new Dictionary<string, GameObject>();

        // PUBLIC METHODS //////////////

        public void Initialize()
        {
            GameObject[] maps = Resources.LoadAll<GameObject>(Filepath.MAPS);
            foreach (GameObject map in maps)
            {
                _maps[map.name] = map;
            }
            AmbitionApp.GetModel<CalendarModel>().RegisterCalendar<PartyVO>(_calendar);
            Resources.UnloadUnusedAssets();
        }

        public string NextRequiredIncident()
        {
            _incidentIndex++;
            return RequiredIncident;
        }

        public MapView LoadMap(Transform parent)
        {
            if (string.IsNullOrEmpty(Party?.Map)) return null;
            _maps.TryGetValue(Party.Map, out GameObject map);
            if (map == null) return null;

            map = GameObject.Instantiate<GameObject>(map, parent);
            return map.GetComponent<MapView>();
        }

        public PartyVO LoadParty(string partyID)
        {
            LoadParty(partyID, out PartyVO party);
            return Party;
        }

        public bool LoadParty(string partyID, out PartyVO party)
        {
            PartyConfig config = Resources.Load<PartyConfig>(Filepath.PARTIES + partyID);
            if (config == null)
            {
                party = null;
                Resources.UnloadUnusedAssets();
                return false;
            }

            IncidentModel story = AmbitionApp.Story;
            List<string> incidents = new List<string>();
            IncidentVO incident = config.IntroIncident?.GetIncident();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            party = new PartyVO(config.name);
            party.Size = config.Size == PartySize.None ? PartySize.Trivial : config.Size;
            party.IntroIncident = incident?.ID;
            story.AddDependency(incident, config.name, IncidentType.Party);

            incident = config.ExitIncident?.GetIncident();
            party.ExitIncident = incident?.ID;
            story.AddDependency(incident, config.name, IncidentType.Party);

            if (config.RequiredIncidents != null)
            {
                string[] names = Enum.GetNames(typeof(PartySize));
                int index = Array.IndexOf(names, party.Size.ToString());
                int numTurns = index < model.NumTurnsByPartySize.Length
                    ? model.NumTurnsByPartySize[index]
                    : model.NumTurnsByPartySize[model.NumTurnsByPartySize.Length - 1];
                if (config.RequiredIncidents.Length > numTurns)
                {
                    numTurns = config.RequiredIncidents.Length;
                    for (int i = model.NumTurnsByPartySize.Length - 1; i>0; --i)
                    {
                        if (numTurns >= model.NumTurnsByPartySize[i] && i<names.Length)
                        {
                            party.Size = (PartySize)i;
                            break;
                        }
                    }
                }
                foreach (IncidentConfig iconfig in config.RequiredIncidents)
                {
                    incident = iconfig?.GetIncident();
                    incidents.Add(incident?.ID);
                    story.AddDependency(incident, config.name, IncidentType.Party);
                }
            }
            party.RequiredIncidents = incidents.ToArray();

            if (config.SupplementalIncidents != null)
            {
                incidents.Clear();
                foreach (IncidentConfig iconfig in config.SupplementalIncidents)
                {
                    incident = iconfig?.GetIncident();
                    if (incident != null)
                    {
                        incidents.Add(incident?.ID);
                        story.AddDependency(incident, config.name, IncidentType.Party);
                    }
                }
            }
            party.SupplementalIncidents = incidents.ToArray();

            if (config.Date > 0) party.Day = new DateTime(config.Date).Subtract(AmbitionApp.Calendar.StartDate).Days;
            party.Faction = config.Faction == FactionType.None ? party.Faction : config.Faction;
            party.RSVP = config.RSVP == RSVP.Required || party.RSVP == RSVP.Required
                ? RSVP.Required
                : party.RSVP == RSVP.New
                ? config.RSVP
                : config.RSVP == RSVP.New
                ? party.RSVP
                : config.RSVP;
            if (!string.IsNullOrWhiteSpace(config.Host))
                party.Host = config.Host;

            party.Requirements = config.Requirements;
            party.Map = config.Map?.name;
            config = null;
            Resources.UnloadUnusedAssets();
            return true;
        }

        public void ResetParty()
        {
            Turns = 0;
            Turn = -1;
            _incidentIndex = 0;
            Rewards.Clear();
        }

        public void Reset()
        {
            ResetParty();
            _calendar.Clear();
            AmbitionApp.Calendar.RegisterCalendar<PartyVO>(_calendar);
        }

        public List<PartyVO> GetNewInvitations(bool futureEvents, bool pastEvents) => AmbitionApp.Calendar.GetNewEvents<PartyVO>(futureEvents, pastEvents);
        public bool HasNewEvents(bool futureEvents, bool pastEvents) => AmbitionApp.Calendar.HasNewEvents<PartyVO>(futureEvents, pastEvents);

        public override string ToString()
        {
            return "PartyModel:" +
                "\n Party: " + Party.ToString() +
                "\n " + string.Format("Turn: {0}/{1}", Turn, Turns);
		}

        [Serializable]
        public class PartyCalendar:Calendar<PartyVO> { }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            AmbitionApp.Calendar.RegisterCalendar<PartyVO>(_calendar);
            int day = _calendar.Today.Subtract(_calendar.StartDate).Days;
            PartyVO party;
            foreach(KeyValuePair<int, List<PartyVO>> kvp in _calendar)
            {
                if (kvp.Value != null && kvp.Key >= day)
                {
                    for (int i = kvp.Value.Count - 1; i >= 0; --i)
                    {
                        LoadParty(kvp.Value[i].ID, out party);
                    }
                }
            }
        }
    }
}
