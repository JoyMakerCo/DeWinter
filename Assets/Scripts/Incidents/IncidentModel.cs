using System;
using Core;
using UnityEngine;

namespace Ambition
{
    public class IncidentModel : IModel, Util.IInitializable
    {
        public IncidentVO[] Incidents;
        private IncidentVO _incident;
        private MomentVO _moment;

        public void Initialize()
        {
            Timeline tl = Resources.Load<Timeline>("Timeline");
            Incidents = new IncidentVO[tl.Incidents.Length];
            for (int i = Incidents.Length - 1; i >= 0; i--)
            {
                Incidents[i] = tl.Incidents[i].GetIncident();
            }
            Resources.UnloadAsset(tl);
        }

        public IncidentVO Incident
        {
            get { return _incident; }
            set
            {
                _incident = value;
                AmbitionApp.SendMessage<IncidentVO>(value);
            }
        }

        public MomentVO Moment
        {
            get { return _moment; }
            set {
                _moment = value;
                AmbitionApp.SendMessage<MomentVO>(_moment);
            }
        }
    }
}
