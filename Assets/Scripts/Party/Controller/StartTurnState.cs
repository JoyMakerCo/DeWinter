using System;
using UFlow;
using Core;

namespace Ambition
{
	public class StartTurnState : UState
	{
		public override void OnEnterState ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			model.Turn++;
			model.TargetedGuests = null;
			if (model.Turn == 1) AmbitionApp.SendMessage(PartyMessages.FILL_REMARKS);
			AmbitionApp.SendMessage(PartyMessages.START_TURN);
		}
	}
}
