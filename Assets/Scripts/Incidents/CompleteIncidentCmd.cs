using System;
using Core;
using UFlow;
using UnityEngine;

namespace Ambition
{
	public class CompleteIncidentCmd : ICommand<IncidentVO>
	{
		public void Execute(IncidentVO incident)
		{
            Debug.Log("CompleteIncidentCmd.Execute");
            AmbitionApp.GetModel<GameModel>().MarkCompleteIncident(incident);

            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            if (incident == model.Incident)
            {
                model.Moment = null;
            }
            if (model.IncidentQueue.Remove(incident)
                && model.Incident != null
                && model.Moment == null)
            {
                AmbitionApp.SendMessage(model.Incident);
            }
		}
	}
}
