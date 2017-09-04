using System;
using UFlow;

namespace Ambition
{
	public class WaitForNextState : UState
	{
		public override void OnEnterState ()
		{
			AmbitionApp.Subscribe(GameMessages.NEXT_STATE, End);
		}

		public override void OnExitState ()
		{
			AmbitionApp.Unsubscribe(GameMessages.NEXT_STATE, End);
		}
	}
}
