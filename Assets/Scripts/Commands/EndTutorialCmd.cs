using UnityEngine;
using System;
using Core;

namespace Ambition
{
	public class EndTutorialCmd : ICommand
	{
		public void Execute ()
		{
			MapModel map = AmbitionApp.GetModel<MapModel>();
			if (map.Room != null && map.Room.HostHere)
			{
				AmbitionApp.OpenMessageDialog("party_tutorial_end");

				AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
				AmbitionApp.UnregisterCommand<WorkTheRoomTutorialCmd, RoomVO>(MapMessage.GO_TO_ROOM);
				AmbitionApp.UnregisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);
				AmbitionApp.UnregisterCommand<EndTutorialCmd>(PartyMessages.SHOW_MAP);
				AmbitionApp.UnregisterCommand<TutorialRailroadCommand, RoomVO>();

				AmbitionApp.RegisterCommand<OutOfConfidenceDialogCmd>(PartyMessages.SHOW_MAP);
			}
		}
	}
}
