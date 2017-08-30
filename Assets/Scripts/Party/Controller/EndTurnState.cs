using System;
using UFlow;

namespace Ambition
{
	public class EndTurnState : UState
	{
		public override void OnEnterState ()
		{
			AmbitionApp.GetModel<PartyModel>().TurnsLeft--;
			AmbitionApp.Execute<BoredomCmd>();
			AmbitionApp.Execute<HostVictoryCheckCmd>();
			End();
		}
	}
}
