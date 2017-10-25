using System;
using UFlow;

namespace Ambition
{
	public class WaitForEventTransition : UTransition
	{
		public override bool InitializeAndValidate ()
		{
			AmbitionApp.Subscribe<EventVO>(HandleEvent);
			return false;
		}

		private void HandleEvent(EventVO e)
		{
			if (e != null && e.currentStage != null)
				Validate();
		}

		public override void Dispose ()
		{
			AmbitionApp.Unsubscribe<EventVO>(HandleEvent);
		}
	}
}
