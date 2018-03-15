using System;
using Core;

namespace Ambition
{
	public class EndTutorialCmd : ICommand
	{
		public void Execute ()
		{
			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.UnregisterCommand<WorkTheRoomTutorialCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.UnregisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);
			AmbitionApp.UnregisterCommand<EndTutorialCmd>(PartyMessages.END_PARTY);
			AmbitionApp.UnregisterCommand<TutorialRailroadCommand, RoomVO>();

			AmbitionApp.RegisterCommand<OutOfConfidenceDialogCmd>(PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterCommand<EndPartyCmd>(PartyMessages.END_PARTY);
		}
	}
}