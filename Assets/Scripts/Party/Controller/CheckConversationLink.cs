using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using UFlow;

namespace Ambition
{
	public class CheckConversationLink : ULink
	{
		public override bool Validate()
            => false;
		//{ 
  //          ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
  //          return model.Guests.Any(g=>!g.IsLockedIn);
		//}
	}
}
