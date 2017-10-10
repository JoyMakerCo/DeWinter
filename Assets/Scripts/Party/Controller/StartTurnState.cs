using System;
using UFlow;
using Core;

namespace Ambition
{
	public class StartTurnState : UState
	{
		private MessageSvc _messageService = App.Service<MessageSvc>();

		public override void OnEnterState ()
		{
			_messageService.Send(PartyMessages.START_TURN);
		}
	}
}
