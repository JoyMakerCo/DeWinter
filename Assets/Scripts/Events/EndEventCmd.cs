using System;
using Core;

namespace DeWinter
{
	public class EndEventCmd : ICommand<EventVO>
	{
		public void Execute (EventVO e)
		{
			EventModel mod = DeWinterApp.GetModel<EventModel>();
			if (mod.SelectedEvent == e)
			{
				mod.SelectedEvent = null;
			}
		}
	}
}