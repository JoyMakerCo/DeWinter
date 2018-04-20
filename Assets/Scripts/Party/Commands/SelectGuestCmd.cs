using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class SelectGuestCmd : ICommand<GuestVO>
	{
		public void Execute (GuestVO guest)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			RemarkVO remark = model.Remark;
			if (remark == null) return;
			
			MapModel map = AmbitionApp.GetModel<MapModel>();
			GuestVO [] guests = map.Room.Guests;
			int index = Array.IndexOf(guests, guest);
			if (index >= 0)
			{
				int num = guests.Length;
				model.TargetedGuests = new GuestVO[remark.NumTargets];
				for (int i=remark.NumTargets-1; i>=0; i--)
				{
					model.TargetedGuests[i] = guests[(i+index)%num];
				}
	            AmbitionApp.SendMessage(PartyMessages.END_TURN);
			}
		}
	}
}
