using System;
using Core;

namespace Ambition
{
	public class FlipRemarkCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			int profile = 0;
			for (int flip = model.Remark.Profile; flip > 0; flip >>= 1)
	    	{
	    		profile <<= 1;
	    		profile += (flip & 1);
	    	}
	    	model.Remark.Profile = profile;
	    	model.Hand = model.Hand; // Fire a message
	    	AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_SELECTED, null);
		}
	}
}

