using System;
using UFlow;
using Core;

using System.Linq;

namespace Ambition
{
	public class StartTurnState : UState
	{
		public override void OnEnterState ()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            if (model.Round%model.FreeRemarkCounter == 0)
				App.Service<MessageSvc>().Send(PartyMessages.ADD_REMARK);
            model.Round++;
            AmbitionApp.SendMessage<GuestVO[]>(model.Guests);
            if (model.Round == 1) AmbitionApp.SendMessage(PartyMessages.FILL_REMARKS);
			AmbitionApp.SendMessage(PartyMessages.START_ROUND);
		}
	}
}
