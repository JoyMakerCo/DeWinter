using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
    public class IncidentModel : IModel, Util.IInitializable
    {
        public List<IncidentVO> Incidents;
        public List<IncidentVO> PastIncidents = new List<IncidentVO>();
        private List<IncidentVO> _queue = new List<IncidentVO>();
        private MomentVO _moment;

        public void Initialize()
        {
            Timeline tl = Resources.Load<Timeline>("Timeline");
            Incidents = tl.Incidents.Select(i => i.Incident).ToList();
            Resources.UnloadAsset(tl);
        }

        public IncidentVO Incident
        {
            get { return _queue.Count > 0 ? _queue[0] : null; }
            set
            {
                if (value == null) return;
                int index = _queue.LastIndexOf(value);
                if (index == 0 || index == 1) return;
                _queue.Remove(value);
                if (_queue.Count == 0)
                {
                    _queue.Add(value);
                    AmbitionApp.SendMessage<IncidentVO>(value);
                }
                else
                {
                    _queue.Insert(1, value);
                }
            }
        }

        public void EndIncident()
        {
            if (_queue.Count > 0)
            {
                PastIncidents.Add(_queue[0]);
                _queue.RemoveAt(0);
            }
            if (_queue.Count > 0)
                AmbitionApp.SendMessage<IncidentVO>(_queue[0]);
        }

        public MomentVO Moment
        {
            get { return _moment; }
            set {
                _moment = value;
                AmbitionApp.SendMessage<MomentVO>(_moment);
            }
        }

        private IncidentVO Find(string incidentID)
        {
            return Incidents.Find(i => i.Name == incidentID);
        }
    }
}
