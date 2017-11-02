using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UFlow;

namespace Ambition
{
	public class StartupManager : MonoBehaviour
	{
		void Awake ()
		{
			// UFlow Associations
			// In the future, this will be handled by config
			AmbitionApp.RegisterState<StartConversationState>("InitConversation");
			AmbitionApp.RegisterState<OpenDialogState, string>("ReadyGo", DialogConsts.READY_GO);
			AmbitionApp.RegisterState<StartTurnState>("StartTurn");
			AmbitionApp.RegisterState<EndTurnState>("EndTurn");
			AmbitionApp.RegisterState<EndConversationState>("EndConversation");

			AmbitionApp.RegisterTransition("ConversationController", "InitConversation", "ReadyGo");
			AmbitionApp.RegisterTransition<WaitForCloseDialogTransition>("ConversationController", "ReadyGo", "StartTurn", DialogConsts.READY_GO);
			AmbitionApp.RegisterTransition<WaitForMessageTransition>("ConversationController", "StartTurn", "EndTurn", PartyMessages.END_TURN);
			AmbitionApp.RegisterTransition<CheckConversationTransition>("ConversationController", "EndTurn", "EndConversation");
			AmbitionApp.RegisterTransition("ConversationController", "EndTurn", "StartTurn");

			// Estate States. This lands somewhere between confusing and annoying.
			AmbitionApp.RegisterState<StartEstateState>("InitEstate");
			AmbitionApp.RegisterState<StartEventState>("StartEvent");
			AmbitionApp.RegisterState<EventState>("EventStage");
			AmbitionApp.RegisterState<EndEventState>("EndEvent");
			AmbitionApp.RegisterState<EnterEstateState>("Estate");
			AmbitionApp.RegisterState<StyleChangeState>("StyleChange");
			AmbitionApp.RegisterState<CreateInvitationsState>("CreateInvitations");

			AmbitionApp.RegisterTransition<CheckEventsTransition>("EstateController", "InitEstate", "StartEvent");
			AmbitionApp.RegisterTransition("EstateController", "InitEstate", "Estate");
			AmbitionApp.RegisterTransition("EstateController", "StartEvent", "EventStage");
			AmbitionApp.RegisterTransition<WaitForEventTransition>("EstateController", "EventStage", "EventStage");
			AmbitionApp.RegisterTransition<WaitForEndEventTransition>("EstateController", "EventStage", "EndEvent");
			AmbitionApp.RegisterTransition("EstateController", "EndEvent", "Estate");
			AmbitionApp.RegisterTransition("EstateController", "Estate", "StyleChange");
			AmbitionApp.RegisterTransition<WaitForCloseDialogTransition>("EstateController", "StyleChange", "CreateInvitations", DialogConsts.MESSAGE);

			AmbitionApp.RegisterState<InitGameState>("InitGame");
			AmbitionApp.RegisterState<StartGameState>("StartGame");
			AmbitionApp.RegisterTransition("GameController", "InitGame", "StartGame");

			AmbitionApp.InvokeMachine("GameController");
			
			Destroy(this.gameObject);
		}
	}
}
