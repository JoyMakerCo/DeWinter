using System;
using UFlow;

namespace Ambition
{
	public class CheckConfidenceLink : ULink
	{
		public override bool InitializeAndValidate ()
		{
			return AmbitionApp.GetModel<PartyModel>().Confidence <= 0;
		}
	}
}
