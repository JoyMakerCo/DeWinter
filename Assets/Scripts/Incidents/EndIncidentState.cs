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
            AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, model.Incident);
            if (model.PlayCount.ContainsKey(model.Incident.Name))
                model.PlayCount[model.Incident.Name]++;
            else model.PlayCount[model.Incident.Name] = 1;
            model.IncidentQueue.Remove(model.Incident);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC, 2f);
        }
    }
}
