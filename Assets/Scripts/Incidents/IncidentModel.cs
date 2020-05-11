using Core;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ambition
{
    [Serializable]
    public class IncidentModel : Model, Util.IInitializable, IResettable
    {
        public void Initialize()
        {
            IncidentConfig [] incidents = UnityEngine.Resources.LoadAll<IncidentConfig>("PerilIncidents");
            PerilIncidents = incidents.Select(i => i.GetIncident()).ToList();

			IncidentConfig gettingCaught = UnityEngine.Resources.Load("Incidents/Getting Caught") as IncidentConfig;
            GettingCaughtIncident = gettingCaught.GetIncident();
        }

        public List<IncidentVO> IncidentQueue = new List<IncidentVO>();
        public IncidentVO Incident => IsIncidentQueued ? IncidentQueue[0] : null;
        public bool IsIncidentQueued => IncidentQueue?.Count > 0;
        public List<IncidentVO> PerilIncidents;
        public Dictionary<string, int> PlayCount = new Dictionary<string, int>();
        public IncidentVO GettingCaughtIncident;

        [UnityEngine.SerializeField]
        private int _moment = -1;
        public MomentVO Moment
        {
            get => Incident?.Nodes != null && _moment >= 0 && _moment < Incident.Nodes.Length
                ? Incident.Nodes[_moment]
                : null;
            set => _moment = (value != null && Incident?.Nodes != null)
                ? Array.IndexOf(Incident.Nodes, value)
                : -1;
        }

        public void Reset()
        {
            IncidentQueue.Clear();
            _moment = -1;
        }
    }
}
