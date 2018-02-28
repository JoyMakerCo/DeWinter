using System;
using UFlow;

namespace Ambition
{
	public class CheckEventsTransition : UTransition
	{
		public override bool InitializeAndValidate ()
		{
			EventModel model = AmbitionApp.GetModel<EventModel>();
			return model.Config != null;
		}
	}
}
