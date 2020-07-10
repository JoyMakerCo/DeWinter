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
        public PartyModel() : base("PartyData") { }

        [JsonProperty("turns")]
        public int Turns;

        [JsonProperty("turn")]
        public int Turn = -1;

        [JsonProperty("room")]
        public int Room = -1;

        public int NumRooms => Incidents?.Length??0;

        public int TurnsLeft => Turns - Turn;

        [JsonProperty("parties")]
        public Dictionary<string, PartyVO> Parties = new Dictionary<string, PartyVO>();

        [JsonProperty("incidents")]
        public string[] Incidents;

        [JsonProperty("incident_index")]
        private int _incidentIndex = 0;

        [JsonProperty("party")]
        public string PartyID
        {
            get => Party?.ID;
            set
            {
                if (string.IsNullOrEmpty(value)) Party = null;
                else Parties.TryGetValue(value, out Party);
            }
        }

        public string RequiredIncident => (_incidentIndex < (Party?.RequiredIncidents?.Length ?? 0))
            ? Party.RequiredIncidents[_incidentIndex]
            : null;

        [JsonIgnore]
        public PartyVO Party;

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

        [JsonIgnore]
        private Dictionary<string, GameObject> _maps = new Dictionary<string, GameObject>();

        [JsonIgnore]
        private Dictionary<FactionType, List<string>> _genericMaps = new Dictionary<FactionType, List<string>>();

        public PartyVO[] GetParties(DateTime date)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            OccasionVO[] occasions = calendar.GetOccasions(OccasionType.Party);
            List<PartyVO> parties = new List<PartyVO>();
            foreach(OccasionVO occasion in occasions)
            {
                if (Parties.TryGetValue(occasion.ID, out PartyVO party))
                    parties.Add(party);
            }
            return parties.ToArray();
        }

        public PartyVO[] GetParties()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            return GetParties(calendar.Today);
        }

        public PartyVO GetParty(DateTime date, bool attendingOnly=false)
        {
            PartyVO[] parties = GetParties(date);
            if (parties.Length == 0) return null;
            if (attendingOnly) return Array.Find(parties, p => p.Attending);
            return Array.Find(parties, p => p.Attending) ?? parties[0];
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
        }

        public PartyVO GetParty(bool attendingOnly = false) => GetParty(AmbitionApp.GetModel<CalendarModel>().Today, attendingOnly);

        public string NextRequiredIncident()
        {
            _incidentIndex++;
            return RequiredIncident;
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
            return view;
        }

        public PartyVO LoadConfig(PartyConfig config)
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
            for (int i= party.RequiredIncidents.Length-1; i>=0; --i)
            {
                party.RequiredIncidents[i] = config.RequiredIncidents[i]?.name;
            }
            party.SupplementalIncidents = new string[config.SupplementalIncidents?.Length ?? 0];
            for (int i = party.SupplementalIncidents.Length - 1; i >= 0; --i)
            {
                party.SupplementalIncidents[i] = config.SupplementalIncidents[i].name;
            }
            return party;
        }

        public void EndParty()
        {
            Turns = 0;
            Turn = -1;
            _incidentIndex = 0;
            Party = null;
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
