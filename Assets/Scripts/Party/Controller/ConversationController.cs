using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFlow;

namespace Ambition
{
	public class ConversationController : UMachine
	{
		public ConversationController()
		{
			ID = "ConversationController";
		}

		public override void OnEnterState ()
		{
			_UFlow.RegisterState<StartConversationMachine>("StartConversation", ID);
			_UFlow.RegisterState<StartTurnMachine>("StartTurn", ID);
			_UFlow.RegisterState<PlayerTurnMachine>("PlayTurn", ID);
			_UFlow.RegisterState<GuestResponseMachine>("GuestResponse", ID);
			_UFlow.RegisterState<EndTurnMachine>("EndTurn", ID);

			_states = new string[5][];
			_states[0] = new string[]{"StartConversation", "StartTurn"};
			_states[1] = new string[]{"StartTurn", "PlayTurn"};
			_states[2] = new string[]{"PlayTurn", "GuestResponse"};
			_states[3] = new string[]{"GuestResponse", "EndTurn"};
			_states[4] = new string[]{"EndTurn", "StartTurn", null};
		}
	}
}
