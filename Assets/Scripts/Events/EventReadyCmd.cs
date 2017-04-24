using System;
using System.Collections;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class EventReadyCmd : ICommand<DateTime>
	{
		private const string EVENT_DIALOG_ID = "EventPopUpModal";

		public void Execute (DateTime day)
		{
			EventModel emod = DeWinterApp.GetModel<EventModel>();
			if (emod.SelectedEvent != null)
			{
				DeWinterApp.OpenDialog<EventVO>(EVENT_DIALOG_ID, emod.SelectedEvent);
			}
		}
	}
}