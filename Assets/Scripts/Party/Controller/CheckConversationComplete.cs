using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using UFlow;

namespace Ambition
{
	public class CheckConversationTransition : ULink
	{
		public override bool Validate()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            return model.Guests.Count(g=>g.IsLockedIn) == model.Guests.Length;
		}
	}
}
