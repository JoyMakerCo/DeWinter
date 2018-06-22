using System;
using UFlow;

namespace Ambition
{
	public class CheckConfidenceLink : ULink
	{
		override public bool Validate()
		{
			return (AmbitionApp.GetModel<PartyModel>().Confidence <= 0);
		}
	}
}
