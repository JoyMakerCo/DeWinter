using System;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
	public class EndTurnState : UState
	{
		public override void OnEnterState ()
		{
			AmbitionApp.SendMessage(PartyMessages.END_TURN);
		}
	}
}
