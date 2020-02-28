using Core;
using System;
using System.Collections.Generic;

namespace Ambition
{
    [Serializable]
    public class IncidentModel : Model, Util.IInitializable, IResettable
    {
        public void Initialize()
        {
            IncidentConfig [] incidents = UnityEngine.Resources.LoadAll<IncidentConfig>("PerilIncidents");
<<<<<<< Updated upstream
            PerilIncidents = incidents.Select(i => i.GetIncident()).ToList();
=======
            PerilIncidents = new List<IncidentVO>();
            Array.ForEach(incidents, (i)=>PerilIncidents.Add(i.GetIncident()));

			IncidentConfig gettingCaught = UnityEngine.Resources.Load("Incidents/Getting Caught") as IncidentConfig;
            GettingCaughtIncident = gettingCaught.GetIncident();
>>>>>>> Stashed changes
        }

        public List<IncidentVO> IncidentQueue = new List<IncidentVO>();
        public IncidentVO Incident => IncidentQueue.Count > 0 ? IncidentQueue[0] : null;

        public List<IncidentVO> PerilIncidents;

<<<<<<< Updated upstream
=======
        public IncidentVO GettingCaughtIncident;

        public int GetPlayCount( string incidentID )
        {
            var _game = AmbitionApp.GetModel<GameModel>();
            if (_game.IncidentHistory.ContainsKey(incidentID))
            {
                return _game.IncidentHistory[incidentID];
            }
            return 0;
        }

>>>>>>> Stashed changes
        [UnityEngine.SerializeField]
        private int _moment = -1;
        public MomentVO Moment
        {
            set
            {
                _moment = value == null ? -1 : (Incident?.GetNodeIndex(value) ?? -1);
                if (_moment >= 0) AmbitionApp.SendMessage(value);
            }
            get => Incident?.Nodes[_moment];
        }

        public void Reset()
        {
            IncidentQueue.Clear();
            _moment = -1;
        }
    }
}
