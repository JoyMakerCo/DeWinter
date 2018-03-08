using System;
using UFlow;

namespace Ambition
{
	public class CheckIncidentsLink : ULink
	{
		public override bool InitializeAndValidate ()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			return model.Incident != null;
		}
	}
}
