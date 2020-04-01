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
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            calendar.Complete(model.Incident);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC, 2f);
        }
    }
}
