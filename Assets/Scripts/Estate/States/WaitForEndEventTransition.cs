using System;
using UFlow;

namespace Ambition
{
	public class WaitForEndEventTransition : UTransition
	{
		public override bool InitializeAndValidate ()
		{
			AmbitionApp.Subscribe<MomentVO>(HandleMoment);
			return false;
		}

		private void HandleMoment(MomentVO m)
		{
			if (m == null) Validate();
		}

		public override void Dispose ()
		{
			AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
		}
	}
}
