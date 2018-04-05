using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

namespace Ambition
{
	public class WorkTheRoomTutorialCmd : ICommand<RoomVO>
	{
		public void Execute (RoomVO room)
		{
			// AmbitionApp.OpenDialog<string[]>(TutorialTooltip.DIALOG_ID, new string[]{"drawer1"});
			AmbitionApp.UnregisterCommand<WorkTheRoomTutorialCmd, RoomVO>(MapMessage.GO_TO_ROOM);
		}
	}
}
