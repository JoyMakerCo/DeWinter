using System;
using Core;

namespace Ambition
{
	public class StartEventCmd : ICommand<string>
	{
		public void Execute (string EventID)
		{
			EventModel model = AmbitionApp.GetModel<EventModel>();
            model.Config = model.FindEvent(EventID);
		}
	}
}
