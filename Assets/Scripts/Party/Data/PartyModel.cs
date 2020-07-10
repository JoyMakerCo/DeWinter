using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using UnityEngine;
using Util;

namespace Ambition
{
    [Saveable]
    public class PartyModel : DocumentModel, IInitializable, IConsoleEntity, IResettable
    {
        // CONSTRUCTOR //////////////////////

        public PartyModel() : base("PartyData") { }

        // PUBLIC DATA //////////////////////

        [JsonProperty("turns")]
        public int Turns;

        [JsonProperty("turn")]
        public int Turn = -1;

        [JsonProperty("room")]
        public int Room = -1;

        [JsonIgnore]
        public int NumRooms => Incidents?.Length??0;

        [JsonIgnore]
        public int TurnsLeft => Turns - Turn;

        [JsonProperty("parties")]
        public Dictionary<string, PartyVO> Parties = new Dictionary<string, PartyVO>();

        [JsonProperty("incidents")]
        public string[] Incidents;

        [JsonIgnore]
        public string PartyID => Party?.ID;

        [JsonIgnore]
        public PartyVO Party { get; private set; }

        [JsonProperty("last_outfit")]
        public string LastOutfitID;

        [JsonProperty("charmed_remark_bonus")]
        public int CharmedRemarkBonus;

        [JsonProperty("free_remark_counter")]
        public int FreeRemarkCounter;

        [JsonProperty("boredom_penalty")]
        public int BoredomPenalty;

        [JsonProperty("boredom_remark_penalty")]
        public int BoredomRemarkPenalty;

        [JsonProperty("offended_remark_penalty")]
        public int OffendedRemarkPenalty;

        [JsonProperty("guest_difficulty")]
        public GuestDifficultyVO[] GuestDifficultyStats;

        // PRIVATE DATA //////////////////////

        [JsonProperty("incident_index")]
        private int _incidentIndex = 0;

        [JsonProperty("calendar")]
        private Dictionary<ushort, List<PartyVO>> _calendar = new Dictionary<ushort, List<PartyVO>>();

        [JsonIgnore]
        private Dictionary<string, GameObject> _maps = new Dictionary<string, GameObject>();

        [JsonIgnore]
        private Dictionary<FactionType, List<string>> _genericMaps = new Dictionary<FactionType, List<string>>();

        [JsonIgnore]
        private ushort _day;

        // PUBLIC METHODS //////////////////////

        public void Schedule(PartyVO party) => Schedule(party, party?.Date ?? default);
        public void Schedule(PartyVO party, DateTime date)
        {
            if (party == null || date.Equals(default)) return;
            GameModel game = AmbitionApp.GetModel<GameModel>();
            ushort day = (ushort)(date.Subtract(game.StartDate).Days);
            if (!_calendar.TryGetValue(day, out List<PartyVO> parties))
            {
                _calendar[day] = parties = new List<PartyVO>();
            }
            if (!parties.Contains(party)) parties.Add(party);
            Broadcast();
        }

        public PartyVO SetDay(ushort day)
        {
            _day = day;
            UpdateParty();
            Broadcast();
            return Party;
        }

        public PartyVO UpdateParty()
        {
            Party = GetParty(_day, true);
            Broadcast();
            return Party;
        }

        public PartyVO NextDay() => SetDay((ushort)(_day + 1));

        public string GetRequiredIncident()
        {
            return (_incidentIndex < (Party?.RequiredIncidents?.Length ?? 0))
                ? Party.RequiredIncidents[_incidentIndex]
                : null;
        }

        public PartyVO[] GetParties(ushort day)
        {
            return (_calendar.TryGetValue(day, out List<PartyVO> parties) && parties != null)
                ? parties.ToArray()
                : new PartyVO[0];
        }

        public PartyVO[] GetParties() => GetParties(_day);

        public PartyVO GetParty(ushort day, bool attendingOnly=false)
        {
            PartyVO[] parties = GetParties(day);
            PartyVO result = null;
            foreach (PartyVO party in parties)
            {
                switch (party.RSVP)
                {
                    case RSVP.Required:
                        return party;
                    case RSVP.Accepted:
                        result = party;
                        break;
                }
            }
            return result ?? (!attendingOnly && parties.Length > 0 ? parties[0] : null);
        }

        public void Initialize()
        {
            GameObject[] maps = Resources.LoadAll<GameObject>(Filepath.MAPS);
            MapView mapView;
            foreach(GameObject map in maps)
            {
                mapView = map.GetComponent<MapView>();
                if (mapView != null)
                {
                    if (!_genericMaps.TryGetValue(mapView.Faction, out List<string> objects))
                    {
                        _genericMaps.Add(mapView.Faction, new List<string>());
                    }
                    _genericMaps[mapView.Faction].Add(map.name);
                    _maps[map.name] = map;
                }
            }
            Resources.UnloadUnusedAssets();
            Broadcast();
        }

        public PartyVO GetParty(bool attendingOnly = false) => GetParty(_day, attendingOnly);

        public string NextRequiredIncident()
        {
            _incidentIndex++;
            return GetRequiredIncident();
        }

        public MapView LoadMap(Transform parent)
        {
            if (string.IsNullOrEmpty(Party?.Map)) return null;
            GameObject map;
            MapView view;
            RoomView[] rooms;
            if (!_maps.TryGetValue(Party.Map, out map))
            {
                if (!_genericMaps.TryGetValue(Party.Faction, out List<string> maps))
                    _genericMaps.TryGetValue(FactionType.Neutral, out maps);
                map = _maps[RNG.Choice(maps)];
            }
            map = GameObject.Instantiate<GameObject>(map, parent);
            view = map.GetComponent<MapView>();
            rooms = view.GetComponentsInChildren<RoomView>();
            if (Incidents?.Length != rooms.Length)
                Incidents = new string[rooms.Length];
            Broadcast();
            return view;
        }

        public PartyVO LoadParty(string partyID)
        {
            if (!Parties.TryGetValue(partyID, out PartyVO party))
            {
                PartyConfig config = Resources.Load<PartyConfig>(Filepath.PARTIES + partyID);
                IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
                party = LoadConfig(config);
                model.AddDependency(config.IntroIncident, partyID, IncidentType.Party);
                model.AddDependency(config.ExitIncident, partyID, IncidentType.Party);
                Array.ForEach(config.SupplementalIncidents, i => model.AddDependency(i, partyID, IncidentType.Party));
                Array.ForEach(config.RequiredIncidents, i => model.AddDependency(i, partyID, IncidentType.Party));
            }
            if (party != null)
            {
                AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
                AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
            }
            Broadcast();
            return party;
        }

        public void EndParty()
        {
            Turns = 0;
            Turn = -1;
            _incidentIndex = 0;
            Party = null;
            Broadcast();
        }

        public void Reset()
        {
            EndParty();
        }

        public string[] Dump()
		{
			var lines = new List<string>();
			lines.Add( "PartyModel:");
			lines.Add(string.Format("Turn: {0}/{1}", Turn,Turns ));
			lines.Add("Incidents: ");

			lines.Add( "Party: " + Party.ToString() );
			var outfit = "null";
			if (LastOutfitID != null)
			{
				outfit = LastOutfitID.ToString();
			}
			lines.Add( "Last Outfit: " + outfit );
			
			return lines.ToArray();
		}

		public void Invoke( string[] args )
		{
    		ConsoleModel.warn("PartyModel has no invocation.");
		}

        private PartySize GetSize(PartyConfig config)
        {
            int count = config?.RequiredIncidents?.Length ?? 0;
            return (count <= (int)(PartySize.Trivial)) ? PartySize.Trivial
                : (count >= (int)(PartySize.Grand)) ? PartySize.Grand
                : PartySize.Decent;
        }

        private PartyVO LoadConfig(PartyConfig config)
        {
            if (config == null) return null;
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO party = model.Parties[config.name] = new PartyVO()
            {
                Name = config.name,
                ID = config.name,
                Date = new DateTime(config.Date),
                LocalizationKey = config.LocalizationKey,
                Description = config.Description,
                Invitation = config.Invitation,
                Faction = config.Faction,
                RSVP = config.RSVP,
                Size = GetSize(config),
                IntroIncident = config.IntroIncident.name,
                ExitIncident = config.ExitIncident.name,
                Host = config.Host,
                Requirements = config.Requirements,
                Rewards = config.Rewards != null ? new List<CommodityVO>(config.Rewards) : new List<CommodityVO>(),
                Map = config.Map?.name
            };
            party.RequiredIncidents = new string[config.RequiredIncidents?.Length ?? 0];
            for (int i = party.RequiredIncidents.Length - 1; i >= 0; --i)
            {
                party.RequiredIncidents[i] = config.RequiredIncidents[i]?.name;
            }
            party.SupplementalIncidents = new string[config.SupplementalIncidents?.Length ?? 0];
            for (int i = party.SupplementalIncidents.Length - 1; i >= 0; --i)
            {
                party.SupplementalIncidents[i] = config.SupplementalIncidents[i].name;
            }
            Broadcast();
            return party;
        }
    }

    public struct RemarkResult
    {
        [JsonProperty("opinion_min")]
        public int OpinionMin;

        [JsonProperty("opinion_max")]
        public int OpinionMax;

        [JsonProperty("remarks")]
        public int Remarks;

        [JsonProperty("reset")]
        public bool ResetInvolvement;
    }
}
