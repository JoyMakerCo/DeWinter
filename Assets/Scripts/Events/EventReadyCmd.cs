using System;
using System.Collections;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class EventReadyCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			EventModel emod = DeWinterApp.GetModel<EventModel>();
			if (emod.SelectedEvent != null)
			{
				DeWinterApp.OpenDialog<EventVO>(DialogConsts.EVENT, emod.SelectedEvent);
			}
		}
	}
}