using System;
using System.Collections.Generic;
using System.Linq;
using UFlow;

namespace Ambition
{
	public class MomentState : UState
	{
		public override void OnEnterState(string[] args)
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			MomentVO moment = model.Moment;
			AmbitionApp.SendMessage(moment.Rewards);
			List<TransitionVO> transitions = model.Incident.GetLinks(moment).Where(t=>AmbitionApp.CheckRequirements(t.Requirements)).ToList();
			TransitionVO xor = transitions.Find(t => t.xor && t.Requirements != null && t.Requirements.Length > 0);
			if (xor != null) transitions.RemoveAll(t => t != xor && t.xor);
			AmbitionApp.SendMessage(transitions.ToArray());
		}
	}
}
