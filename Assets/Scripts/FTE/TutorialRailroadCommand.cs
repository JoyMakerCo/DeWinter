using System;
using Core;
using UnityEngine;

namespace Ambition
{
	public class TutorialRailroadCommand : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			GameObject btn = GameObject.Find("LeaveThePartyButton");
			if (btn != null) btn.AddComponent<LeavePartyBtn>();
			AmbitionApp.UnregisterCommand<TutorialRailroadCommand, RoomVO>();
		}
	}
}
