using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;
using UnityEngine;
using System.Linq;

namespace Ambition
{
    [Saveable]
    public class CharacterModel : ObservableModel<CharacterModel>, Util.IInitializable, IResettable
    {
        // PUBLIC DATA

        [JsonIgnore]
        public int LiaisonChance = 100;

        [JsonProperty("characters")]
        public Dictionary<string, CharacterVO> Characters = new Dictionary<string, CharacterVO>();

        [JsonIgnore]
        public RendezVO Rendezvous => Array.Find(_schedule.GetOccasions(_schedule.Today), r => r.IsAttending);

        [JsonIgnore]
        public RendezVO CreateRendezvous;

        [JsonIgnore]
        public int MissedRendezvousPenalty;

        [JsonIgnore]
        public float FavoredLocationBonus = .6f;

        [JsonIgnore]
        public float LocationBonus = .3f;

        [JsonIgnore]
        public IncidentVO[] ExitIncidents = new IncidentVO[0];

        [JsonIgnore]
        public bool CreateRendezvousMode => CreateRendezvous != null;


        // PRIVATE DATA

        [JsonProperty("calendar")]
        private RendezvousSchedule _schedule = new RendezvousSchedule();


        // PUBLIC METHODS

        public void Initialize()
        {
            AmbitionApp.Calendar.RegisterCalendar<RendezVO>(_schedule);
        }

        public int GetOutfitFavor(string characterID, OutfitVO outfit)
        {
            CharacterVO character = GetCharacter(characterID);
            if (character == null) return 0;

            AmbitionApp.GetModel<FactionModel>().Factions.TryGetValue(character.Faction, out FactionVO faction);
            int score = 200 - Math.Abs(outfit.Modesty - faction.Modesty) - Math.Abs(outfit.Luxury - faction.Luxury);
            return (int)Math.Floor(score * outfit.Novelty * .00333333);
        }

        public CharacterVO GetCharacter(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            Characters.TryGetValue(id, out CharacterVO character);
            if (character != null) return character;

            CharacterConfig config = Resources.Load<CharacterConfig>(Filepath.CHARACTERS + id);
            character = config?.GetCharacter();
            if (character != null)
            {
                Characters[id] = character;
                Broadcast();
            }
            return character;
        }

        public void Reset()
        {
            Characters.Clear();
            CreateRendezvous = null;
            _schedule.Clear();
            Broadcast();
        }

        public override string ToString()
        {
            List<string> result = new List<string>();
            foreach (CharacterVO character in Characters.Values)
                result.Add(character.ToString());
            return "CharacterModel:\n" + string.Join("\n ", result.ToArray());
        }

        public List<RendezVO> GetNewEvents(bool newEvents, bool pastEvents) => AmbitionApp.Calendar.GetNewEvents<RendezVO>(newEvents, pastEvents);
        public bool HasNewEvents(bool futureEvents, bool pastEvents) => AmbitionApp.Calendar.HasNewEvents<RendezVO>(futureEvents, pastEvents);
        public List<RendezVO> GetPendingInvitations(bool future, bool past)
        {
            List<RendezVO> result = new List<RendezVO>();
            foreach (List<RendezVO> list in _schedule.Values)
            {
                foreach(RendezVO rendez in list)
                {
                    if (rendez.IsCaller
                        && (!future || rendez.Day > _schedule.Day)
                        && (!past || rendez.Day < _schedule.Day)
                        && _schedule.Day > rendez.Created
                        && rendez.RSVP == RSVP.New)
                    {
                        result.Add(rendez);
                    }
                }
            }
            return result;
        }
        public bool HasPendingInvitations()
        {
            foreach (List<RendezVO> list in _schedule.Values)
            {
                if (list.Exists(r => r.RSVP == RSVP.New))
                    return true;
            }
            return false;
        }


        // PRIVATE METHODS

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            CalendarModel calendar = AmbitionApp.Calendar;
            calendar.RegisterCalendar<RendezVO>(_schedule);
            foreach (KeyValuePair<int, List<RendezVO>> kvp in _schedule)
            {
                kvp.Value.ForEach(r => r.Day = kvp.Key);
            }
        }

        private class RendezvousSchedule : Calendar<RendezVO> { }
    }
}
