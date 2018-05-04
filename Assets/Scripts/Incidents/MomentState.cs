using System;
using UFlow;

namespace Ambition
{
	public class MomentState : UState
	{
		public override void OnEnterState ()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			MomentVO moment = model.Moment;
			if (moment != null)
			{
				int index = Array.IndexOf(model.Incident.Moments, moment);
				TransitionVO[] transitions = Array.FindAll(model.Incident.Transitions, t=>t.Index == index);
				AmbitionApp.SendMessage<TransitionVO[]>(transitions);
			}
		}
	}
}
