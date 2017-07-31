using System;
using System.Collections;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class EventReadyCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			EventModel emod = AmbitionApp.GetModel<EventModel>();
			if (emod.SelectedEvent != null)
			{
				AmbitionApp.OpenDialog<EventVO>(DialogConsts.EVENT, emod.SelectedEvent);
			}
		}
	}
}