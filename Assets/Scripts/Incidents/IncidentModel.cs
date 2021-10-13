using Core;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    [Saveable]
    public class IncidentModel : Model, Util.IInitializable, IResettable
    {
        // PUBLIC DATA /////////////

        [JsonIgnore] // Loaded upon init
        public readonly Dictionary<string, IncidentVO> Incidents = new Dictionary<string, IncidentVO>();

        [JsonIgnore]
        public readonly Dictionary<IncidentType, string[]> Types = new Dictionary<IncidentType, string[]>();

        [JsonIgnore]
        public IncidentVO Incident { get; private set; }

        [JsonIgnore]
        public MomentVO Moment;

        [JsonProperty("dependencies")]
        public Dictionary<string, LoadedIncident> Dependencies = new Dictionary<string, LoadedIncident>();

        // PRIVATE / PROTECTED DATA /////////////

        [JsonProperty("completed")]
        private List<string> _completed = new List<string>();

        [JsonProperty("visited")]
        private List<string> _visited = new List<string>();

        [JsonProperty("schedule")]
        private IncidentCalendar _schedule = new IncidentCalendar();

        [JsonProperty("index")]
        private int _index;

        [JsonProperty("savedState", Order = 0)]
        private IncidentSaveState _saveState = null;

        // PUBLIC METHODS /////////////

        public bool Schedule(string incidentID) => Schedule(GetIncident(incidentID));
        public bool Schedule(IncidentVO incident)
        {
            if (incident == null) return false;
            DateTime date = incident.Date.Equals(default) ? _schedule.Today : incident.Date;
            string[] occasions = _schedule.GetOccasions(date);
            if (_index > 0 && date.Equals(_schedule.Today))
            {
                return Array.LastIndexOf(occasions, incident.ID) < _index && Schedule(incident, date);
            }
            else
            {
                return Array.IndexOf(occasions, incident.ID) < 0 && Schedule(incident, date);
            }
        }

        public bool Schedule(string incidentID, DateTime date)
        {
            if (string.IsNullOrEmpty(incidentID)) return false;
            _schedule.Schedule(incidentID, date);
            if (date == _schedule._model.Today) UpdateIncident();
            return true;
        }

        public bool Schedule(IncidentVO incident, DateTime date)
        {
            if (!Schedule(incident?.ID, date)) return false;
            Incidents[incident.ID] = incident;
            return true;
        }

        public IncidentVO Update(bool reset = true)
        {
            if (reset) _index = 0;
            return UpdateIncident();
        }

        public IncidentVO NextIncident()
        {
            ++_index;
            Moment = (Incident?.Nodes?.Length ?? 0) > 1 ? Incident.Nodes[0] : null;
            _saveState = null;
            return UpdateIncident();
        }

        public IncidentVO UpdateIncident()
        {
            string[] occasions = _schedule.GetOccasions(_schedule.Today);
            if (_index >= 0 && _index < occasions.Length)
            {
                return Incident = GetIncident(occasions[_index]);
            }
            return Incident = null;
        }

        public void Initialize()
        {
            ResetIncidents();
            InitSchedule(AmbitionApp.Calendar);
        }

        public IncidentVO[] GetIncidents(IncidentType type)
        {
            if (!Types.TryGetValue(type, out string[] ids)) return new IncidentVO[0];
            List<IncidentVO> result = new List<IncidentVO>();
            IncidentVO incident;
            foreach (string id in ids)
            {
                if (Incidents.TryGetValue(id, out incident) && incident != null)
                {
                    result.Add(incident);
                }
            }
            return result.ToArray();
        }

        public bool IsComplete(string incidentId, bool isOneShot)
        {
            if (string.IsNullOrEmpty(incidentId) || _completed.Contains(incidentId)) return true;
            return !isOneShot && _visited.Contains(incidentId);
        }

        public void Reset()
        {
            Moment = null;
            _saveState = null;
            _index = 0;
            _completed.Clear();
            _visited.Clear();
            ResetIncidents();
            InitSchedule(AmbitionApp.Calendar);
            Incident = null;
        }

        public bool SetTestIncident(string incidentID)
        {
#if DEBUG
            Moment = null;
            _saveState = null;
            _index = 0;
            Incident = string.IsNullOrEmpty(incidentID) ? null : GetIncident(incidentID);
            return Incident != null;
#else
            return false;
#endif
        }

        public bool AddDependency(IncidentVO incident, string filepath, IncidentType type)
        {
            string id = incident?.ID;
            if (string.IsNullOrEmpty(id) || _completed.Contains(id))
            {
                incident?.Dispose();
                return false;
            }
            Dependencies[id] = new LoadedIncident(filepath, type);
            Incidents[id] = incident;
            return true;
        }

        public void InitSchedule(CalendarModel model)
        {
            IncidentVO incident;
            _schedule.Clear();
            model.RegisterCalendar(_schedule);
            foreach (string[] incidents in Types.Values)
            {
                foreach (string id in incidents)
                {
                    Incidents.TryGetValue(id, out incident);
                    if (incident != null && incident.Date != default)
                    {
                        _schedule.Schedule(id, incident.Date);
                    }
                }
            }
        }

        public void CompleteCurrentIncident()
        {
            CompleteIncident(Incident?.ID);
            NextIncident();
        }

        public void CompleteIncident(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (Incident.OneShot)
                {
                    if (!_completed.Contains(id))
                        _completed.Add(id);
                    Incidents.Remove(id);
                    Incident.Dispose();
                }
                else if (!_visited.Contains(id))
                {
                    _visited.Add(id);
                }
            }
        }

        public IncidentVO GetIncident(string id)
        {
            return !string.IsNullOrEmpty(id) && Incidents.TryGetValue(id, out IncidentVO incident)
                 ? incident
                 : null;
        }

        public IncidentVO LoadIncident(string id, IncidentType type)
        {
            IncidentVO result = GetIncident(id);
            if (result != null) return result;

            string path;
            IncidentConfig config;
            switch (type)
            {
                case IncidentType.Reward:
                    path = Filepath.INCIDENTS_REWARD;
                    break;
                case IncidentType.Party:
                    path = Filepath.INCIDENTS_PARTY;
                    break;
                case IncidentType.Peril:
                    path = Filepath.INCIDENTS_PERIL;
                    break;
                case IncidentType.Caught:
                    path = Filepath.INCIDENTS_CAUGHT;
                    break;
                case IncidentType.PartyIntro:
                    path = Filepath.INCIDENTS_PARTY_INTRO;
                    break;
                case IncidentType.Political:
                    path = Filepath.INCIDENTS_POLITICAL;
                    break;
                default:
                    path = Filepath.INCIDENTS_TIMELINE;
                    break;
            }

            config = Resources.Load<IncidentConfig>(path + id);
            result = config?.GetIncident();
            Resources.UnloadUnusedAssets();
            return result;
        }

        public MomentVO UpdateMoment()
        {
            if (Moment != null)
            {
                if (_saveState == null)
                    _saveState = new IncidentSaveState();
                _saveState.Moment = Array.IndexOf(Incident.Nodes, Moment);
                if (!string.IsNullOrEmpty(Moment.AmbientSFX.Name))
                    _saveState.Ambient = Moment.AmbientSFX;
                if (!string.IsNullOrEmpty(Moment.Music.Name))
                    _saveState.Music = Moment.Music;
                if (Moment.Background != null)
                    _saveState.Background = _saveState.Moment;
            }
            else _saveState = null;
            return Moment;
        }

        public void RestoreIncident()
        {
            UpdateIncident();
            if (Incident?.Nodes != null && _saveState != null && _saveState.Moment >= 0 && _saveState.Moment < Incident.Nodes.Length)
            {
                Moment = Incident.Nodes[_saveState.Moment];
            }
        }

        public Sprite RestoreSavedState()
        {
            if (_saveState == null) return null;

            if (string.IsNullOrEmpty(Moment?.Music.Name) && !string.IsNullOrEmpty(_saveState.Music.Name))
                AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, _saveState.Music);

            if (string.IsNullOrEmpty(Moment?.AmbientSFX.Name) && !string.IsNullOrEmpty(_saveState.Ambient.Name))
                AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENT, _saveState.Ambient);

            return (Moment?.Background == null
                && Incident?.Nodes != null
                && _saveState.Background >= 0
                && _saveState.Background < Incident.Nodes.Length)
                ? Incident.Nodes[_saveState.Background].Background
                : null;
        }

        // PRIVATE / PROTECTED METHODS /////////////

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            AmbitionApp.Calendar.RegisterCalendar<string>(_schedule);
            List<string> parties = new List<string>();
            List<string> locations = new List<string>();
            PartyModel party = AmbitionApp.GetModel<PartyModel>();
            foreach(KeyValuePair<string, LoadedIncident> kvp in Dependencies)
            {
                switch (kvp.Value.Type)
                {
                    case IncidentType.Party:
                        parties.Add(kvp.Value.Filepath);
                        break;
                    case IncidentType.Location:
                        locations.Add(kvp.Value.Filepath);
                        break;
                }
            }
            parties.ForEach(s => party.LoadParty(s, out PartyVO p));
            locations.ForEach(l=>AmbitionApp.Paris.GetLocation(l));
        }

        private void ResetIncidents()
        {
            Dictionary<IncidentType, string> types = new Dictionary<IncidentType, string>()
            {
                { IncidentType.Timeline, Filepath.INCIDENTS_TIMELINE },
                { IncidentType.Caught, Filepath.INCIDENTS_CAUGHT },
                { IncidentType.Party, Filepath.INCIDENTS_PARTY },
                { IncidentType.Peril, Filepath.INCIDENTS_PERIL },
                { IncidentType.Political, Filepath.INCIDENTS_POLITICAL },
                { IncidentType.Reward, Filepath.INCIDENTS_REWARD },
                { IncidentType.PartyIntro, Filepath.INCIDENTS_PARTY_INTRO }
            };
            List<string> values = new List<string>();
            IncidentConfig[] incidents;
            IncidentVO incident;

            Incidents.Clear();
            Types.Clear();

            foreach (KeyValuePair<IncidentType, string> kvp in types)
            {
                incidents = Resources.LoadAll<IncidentConfig>(kvp.Value);
                values.Clear();
                foreach (IncidentConfig config in incidents)
                {
                    incident = config.GetIncident();
                    if (incident != null)
                    {
                        incident.Type = kvp.Key;
                        Incidents[incident.ID] = incident;
                        values.Add(incident.ID);
                    }
                }
                Types[kvp.Key] = values.ToArray();
            }
            Resources.UnloadUnusedAssets();
        }

        [Serializable]
        private class IncidentCalendar : Calendar<string> { }
    }

    [Serializable]
    public struct LoadedIncident
    {
        [JsonProperty("path")]
        public string Filepath;

        [JsonProperty("type")]
        public IncidentType Type;

        public LoadedIncident(string filepath, IncidentType type)
        {
            Filepath = filepath;
            Type = type;
        }
    }

    [Serializable]
    public class IncidentSaveState
    {
        [JsonProperty("moment")]
        public int Moment = -1;

        [JsonProperty("music")]
        public FMODEvent Music;

        [JsonProperty("ambient")]
        public FMODEvent Ambient;

        [JsonProperty("background")]
        public int Background = -1;
    }
}
