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
			App.Service<ModelSvc>().GetModel<PartyModel>().Remark = null;
			AmbitionApp.SendMessage<GuestVO[]>(AmbitionApp.GetModel<MapModel>().Room.Guests);
		}
	}
}
