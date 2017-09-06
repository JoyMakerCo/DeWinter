using System;
using UFlow;

namespace Ambition
{
	public class WaitForNextState : UState, IPersistentState
	{
		public override void OnEnterState ()
		{
			AmbitionApp.Subscribe(GameMessages.NEXT_STATE, EndState);
		}

		public void OnExitState ()
		{
			AmbitionApp.Unsubscribe(GameMessages.NEXT_STATE, EndState);
		}
	}
}
