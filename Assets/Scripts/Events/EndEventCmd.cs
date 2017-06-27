using System;
using Core;

namespace Ambition
{
	public class EndEventCmd : ICommand<EventVO>
	{
		public void Execute (EventVO e)
		{
			EventModel mod = AmbitionApp.GetModel<EventModel>();
			if (mod.SelectedEvent == e)
			{
				mod.SelectedEvent = null;
			}
		}
	}
}