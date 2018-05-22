using System;
using UFlow;

namespace Ambition
{
	public class CheckConfidenceLink : ULink
	{
		override public void Initialize()
		{
			if (AmbitionApp.GetModel<PartyModel>().Confidence <= 0)
				Activate();
		}
	}
}
