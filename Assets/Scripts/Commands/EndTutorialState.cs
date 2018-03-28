using UnityEngine;
using System;
using Core;
using UFlow;

namespace Ambition
{
	public class EndTutorialState : UState
	{
		public override void OnEnterState ()
		{
			AmbitionApp.OpenMessageDialog("party_tutorial_end");

			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.UnregisterCommand<WorkTheRoomTutorialCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.UnregisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);
			AmbitionApp.UnregisterCommand<TutorialRailroadCommand, RoomVO>();

			AmbitionApp.RegisterCommand<OutOfConfidenceDialogCmd>(PartyMessages.SHOW_MAP);
		}
	}
}
