using System.Collections;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class EventReadyCmd : ICommand<string>
	{
		private const string EVENT_DIALOG_ID = "EventPopUpModal";

		public void Execute (string sceneID)
		{
			if (sceneID == SceneConsts.GAME_ESTATE)
			{
				EventModel emod = DeWinterApp.GetModel<EventModel>();
				if (emod.SelectedEvent != null)
				{
					DeWinterApp.OpenDialog<EventVO>(EVENT_DIALOG_ID, emod.SelectedEvent);
				}
			}
		}
	}
}