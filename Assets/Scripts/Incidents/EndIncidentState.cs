using System;
using UFlow;
using UnityEngine;

namespace Ambition
{
	public class EndIncidentState : UState
	{
        public override void OnEnterState()
        {
            Debug.Log("EndIncidentState.OnEnterState");
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            IncidentVO incident = model.Incident;
            if (incident != null) AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, incident);
            for (incident = model.NextIncident();
                !AmbitionApp.CheckRequirements(incident?.Requirements);
                incident = model.NextIncident());
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC, 2f);
        }
    }
}
