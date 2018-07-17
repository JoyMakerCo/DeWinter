using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class SelectGuestCmd : ICommand<GuestVO>
	{
		public void Execute (GuestVO guest)
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			RemarkVO remark = model.Remark;
			if (remark == null) return;
			
            GuestVO [] guests = model.Guests;
			int index = Array.IndexOf(guests, guest);
			if (index >= 0)
			{
				int num = guests.Length;
				GuestVO[] targets = new GuestVO[remark.NumTargets];
				for (int i=remark.NumTargets-1; i>=0; i--)
				{
					targets[i] = guests[(i+index)%num];
				}
	            AmbitionApp.SendMessage<GuestVO[]>(PartyMessages.GUESTS_SELECTED, targets);
			}
		}
	}
}
