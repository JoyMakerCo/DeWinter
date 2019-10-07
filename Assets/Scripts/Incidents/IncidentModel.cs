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
        }

        public List<IncidentVO> IncidentQueue = new List<IncidentVO>();
        public IncidentVO Incident => IncidentQueue.Count > 0 ? IncidentQueue[0] : null;

        public List<IncidentVO> PerilIncidents;

        [UnityEngine.SerializeField]
        private int _moment = -1;
        public MomentVO Moment
        {
            set
            {
                _moment = value == null ? -1 : (Incident?.GetNodeIndex(value) ?? -1);
                if (_moment >= 0) AmbitionApp.SendMessage(value);
            }
            get => Incident?.Nodes != null && _moment >= 0 && _moment < Incident.Nodes.Length
                ? Incident.Nodes[_moment]
                : null;
        }

        public void Reset()
        {
            IncidentQueue.Clear();
            _moment = -1;
        }
    }
}
