using System;
using Core;

namespace Ambition
{
	public class StartIncidentCmd : ICommand<string>
	{
		public void Execute (string EventID)
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            model.Config = model.FindEvent(EventID);
		}
	}
}
