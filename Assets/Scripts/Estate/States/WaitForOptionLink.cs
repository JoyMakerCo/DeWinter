using System;
using UFlow;

namespace Ambition
{
	public class WaitForOptionLink : ULink
	{
		public override void Initialize()
		{
			AmbitionApp.Subscribe<int>(IncidentMessages.INCIDENT_OPTION, HandleOption);
		}

        private void HandleOption(int option)
        {
            CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = model.Incident;
            if (incident != null)
            {
                MomentVO moment = model.Moment;
                MomentVO[] neighbors = incident.GetNeighbors(moment);
                if (option < neighbors.Length)
                {
                    TransitionVO[] transitions = model.Incident.GetLinkData(moment);

                    if (option < transitions.Length) // This should be a tautology, but whatever
                        Array.ForEach(transitions[option].Rewards, AmbitionApp.SendMessage);

                    model.Moment = neighbors[option];
                }
                else model.Moment = null;
            }
            Activate();
        }
		
		public override void Dispose ()
		{
			AmbitionApp.Unsubscribe<int>(IncidentMessages.INCIDENT_OPTION, HandleOption);
		}
	}
}
