using System;
using UFlow;

namespace Ambition
{
	public class EndEventState : UState
	{
		public override void OnEnterState ()
		{
			EventModel model = AmbitionApp.GetModel<EventModel>();
			EventVO e = model.Event;
			if (e != null) AmbitionApp.SendMessage<EventVO>(EventMessages.END_EVENT, e);
			model.Event = null;
			AmbitionApp.CloseDialog("EVENT");
		}
	}
}
