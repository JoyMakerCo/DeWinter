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
            if (moment != null)
            {
                int index = model.Incident.GetNodeIndex(moment);
                AmbitionApp.SendMessage(moment.Rewards);
                if (index >= 0)
                {
                    AmbitionApp.SendMessage(model.Incident.GetLinkData(index));
                }
            }
        }
	}
}
