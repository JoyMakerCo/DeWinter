using System;
using System.Collections.Generic;
using Core;

namespace Ambition 
{
	public class GuestTargetedCmd : ICommand<GuestVO>
	{
		public void Execute (GuestVO guest)
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			GuestVO [] guests = model.Guests;
            GuestVO[] result = null;
			if (model.Remark != null && guest != null) // The argument guest is the one being explicitly targeted
			{
				int index = Array.IndexOf(guests, guest);
				if (index >= 0)
				{
					int num = model.Remark.NumTargets;
					result = new GuestVO[num];
					for(int i=0; i<num; i++)
					{
						result[i] = guests[(index+i)%guests.Length];
					}
				}
			}
			AmbitionApp.SendMessage<GuestVO[]>(PartyMessages.GUESTS_TARGETED, result);
		}
	}
}
