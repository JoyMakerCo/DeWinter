using Core;
using System;
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

        [JsonProperty("dependencies")]
        public Dictionary<string, LoadedIncident> Dependencies = new Dictionary<string, LoadedIncident>();

        [JsonIgnore]
        public IncidentVO Incident { get; private set; }

        [JsonIgnore]
        public MomentVO Moment
        {
            get => Incident?.Nodes != null && _moment >= 0 && _moment < Incident.Nodes.Length
                    ? Incident.Nodes[_moment]
                    : null;
            set => _moment = Incident?.Nodes == null ? -1 : Array.IndexOf(Incident.Nodes, value);
        }

        // PRIVATE / PROTECTED DATA /////////////

        [JsonProperty("completed")]
        private Dictionary<string, ushort> _completed = new Dictionary<string, ushort>();

        [JsonProperty("moment")]
        private int _moment = -1;

        [JsonIgnore]
        private ushort _day = 0;

        [JsonProperty("schedule")]
        private Dictionary<ushort, List<string>> _schedule = new Dictionary<ushort, List<string>>();

        [JsonProperty("loaded")]
        private Dictionary<string, string> _loaded = new Dictionary<string, string>();

        [JsonProperty("types")]
        private Dictionary<IncidentType, string[]> _types = new Dictionary<IncidentType, string[]>();

        // PUBLIC METHODS /////////////

        public void Initialize()
        {
            IncidentConfig[] incidents;
            List<string> values;
            IncidentVO incident;
            Dictionary<IncidentType, string> types = new Dictionary<IncidentType, string>
            {
                {IncidentType.Timeline, Filepath.INCIDENTS_TIMELINE},
                {IncidentType.Party, Filepath.INCIDENTS_PARTY},
                {IncidentType.Caught, Filepath.INCIDENTS_CAUGHT},
                {IncidentType.Peril, Filepath.INCIDENTS_PERIL}
            };
            foreach(KeyValuePair<IncidentType, string> kvp in types)
            {
                incidents = Resources.LoadAll<IncidentConfig>(kvp.Value);
                values = new List<string>();
                foreach(IncidentConfig config in incidents)
                {
                    incident = config?.GetIncident();
                    if (incident != null)
                    {
                        values.Add(incident.ID);
                        Incidents.Add(incident.ID, incident);
                    }
                }
                _types.Add(kvp.Key, values.ToArray());
            }
            Resources.UnloadUnusedAssets();
        }

        public IncidentVO[] GetIncidents(IncidentType type)
        {
            if (!_types.TryGetValue(type, out string[] ids)) return new IncidentVO[0];
            List<IncidentVO> result = new List<IncidentVO>();
            IncidentVO incident;
            foreach(string id in ids)
            {
                incident = LoadIncident(id);
                if (incident != null) result.Add(incident);
            }
            return result.ToArray();
        }

        public void SetDay(int day, bool reset=true)
        {
            if (_day != (ushort)day)
            {
                if (reset)
                {
                    _schedule.Remove(_day);
                    _moment = -1;
                }
                _day = (ushort)day;
                if (!_schedule.ContainsKey(_day))
                {
                    _schedule.Add(_day, new List<string>());
                }
            }
            LoadIncident();
        }

        public bool IsComplete(string incidentId) => _completed.ContainsKey(incidentId);

        public void Reset()
        {
            _day = 0;
            _moment = -1;
            _completed.Clear();
            _schedule.Clear();
            Incident = null;
        }

        public void AddDependency(IncidentConfig config, string filepath, IncidentType type)
        {
            IncidentVO incident = config?.GetIncident();
            if (incident == null) return;
            Dependencies[incident.ID] = new LoadedIncident(filepath, type);
            Incidents[incident.ID] = incident;
        }

        public IncidentVO NextIncident()
        {
            if (!_schedule.TryGetValue(_day, out List<string> queue) || _schedule[_day] == null)
            {
                _schedule.Remove(_day);
                return null;
            }

            if (!string.IsNullOrEmpty(Incident?.ID) && Incident.OneShot)
            {
                _completed[Incident.ID] = _day;
            }
            Incident = null;
            queue.RemoveAt(0);
            return LoadIncident();
        }

        public void Schedule(string incidentID, int day = -1)
        {
            ushort dday = (day < 0) ? _day : (ushort)day;
            if (!_schedule.TryGetValue(dday, out List<string> queue) || queue == null)
            {
                _schedule[dday] = queue = new List<string>();
            }
            if (!_schedule[dday].Contains(incidentID))
            {
                _schedule[dday].Add(incidentID);
                if (dday == _day) LoadIncident();
            }
        }

        public void Schedule(IncidentVO incident, int day = -1)
        {
            if (incident != null)
            {
                Incidents[incident.ID] = incident;
                Schedule(incident.ID, day);
            }
        }

        public IncidentVO LoadIncident()
        {
            if (!_schedule.TryGetValue(_day, out List<string> queue) || queue?.Count == 0)
                return Incident = null;

            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            for (Incident = null; Incident == null && queue.Count > 0; queue.RemoveAt(0))
            {
                Incident = LoadIncident(queue[0]);
                if (Incident != null) return Incident;
            }
            return Incident=null;
        }

        public IncidentVO LoadIncident(string id)
        {
            if (Incidents.TryGetValue(id, out IncidentVO incident) && incident != null)
                return incident;

            if (_loaded.TryGetValue(id, out string path) && !string.IsNullOrEmpty(path))
            {
                IncidentConfig config = Resources.Load<IncidentConfig>(path);
                incident = config?.GetIncident();
                if (incident != null) return incident;
            }

            if (Dependencies.TryGetValue(id, out LoadedIncident loaded))
            {
                switch(loaded.Type)
                {
                    case IncidentType.Party:
                        AmbitionApp.GetModel<PartyModel>().LoadParty(loaded.Filepath);
                        break;
                }
                Incidents.TryGetValue(id, out incident);
            }
            return incident;
        }

        public IncidentVO LoadIncident(string incidentID, IncidentType type)
        {
            IncidentVO incident = LoadIncident(incidentID);
            if (incident != null) return incident;

            IncidentConfig config;
            string path = null;

            switch (type)
            {
                case IncidentType.Caught:
                    path = Filepath.INCIDENTS_CAUGHT + incidentID;
                    break;
                case IncidentType.Party:
                    path = Filepath.INCIDENTS_PARTY + incidentID;
                    break;
                case IncidentType.Peril:
                    path = Filepath.INCIDENTS_PERIL + incidentID;
                    break;
                case IncidentType.Reward:
                    path = Filepath.INCIDENTS_REWARD + incidentID;
                    break;
                default:
                    path = Filepath.INCIDENTS_TIMELINE + incidentID;
                    break;
            }
            config = Resources.Load<IncidentConfig>(path);
            incident = config?.GetIncident();
            if (incident != null)
            {
                _loaded.Add(incidentID, path);
                Incidents.Add(incidentID, incident);
            }
            return incident;
        }

        // PRIVATE / PROTECTED METHODS /////////////
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
}
