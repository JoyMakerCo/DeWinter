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
			for (int flip = model.Remark.NumTargets; flip > 0; flip >>= 1)
	    	{
	    		profile <<= 1;
	    		profile += (flip & 1);
	    	}
	    	model.Remark.NumTargets = profile;
	    	model.Remarks = model.Remarks; // Fire a message
	    	AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_SELECTED, null);
		}
	}
}

