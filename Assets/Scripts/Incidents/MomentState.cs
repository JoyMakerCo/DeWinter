using System;
using UFlow;

namespace Ambition
{
	public class MomentState : UState
	{
		public override void OnEnterState ()
		{
			CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
			MomentVO moment = model.Moment;
            int index = model.Incident.GetNodeIndex(moment);
            if (index >= 0)
			{
                AmbitionApp.SendMessage<TransitionVO[]>(model.Incident.GetLinkData(index));
			}
		}
	}
}
