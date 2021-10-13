using System;
using UFlow;
using UnityEngine;

namespace Ambition
{
	public class EndIncidentState : UState, Core.IState
	{
        public override void OnEnter()
        {
            IncidentModel model = AmbitionApp.Story;
            model.CompleteCurrentIncident();
            AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, model.Incident?.ID);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC, 2f);
        }
    }
}
