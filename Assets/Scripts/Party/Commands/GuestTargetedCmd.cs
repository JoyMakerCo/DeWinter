using System;
using System.Collections.Generic;
using Core;

namespace Ambition 
{
	public class GuestTargetedCmd : ICommand<GuestVO>
	{
		public void Execute (GuestVO guest)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			if (model.Remark != null && guest != null) // The argument guest is the one being explicitly targeted
			{
				int index = Array.IndexOf(model.Guests, guest);
				if (index >= 0)
				{
					List<GuestVO> guests = new List<GuestVO>(){guest};
					int numGuests = model.Guests.Length;
					// Select the other targets in the profile
					for (int i = numGuests-1; i > 0; i--)
					{
						if (((1 << ((i+numGuests-index)%numGuests)) & model.Remark.Profile) > 0)
						{
							guests.Add(model.Guests[i]);
						}
					}
					AmbitionApp.SendMessage<GuestVO[]>(PartyMessages.GUESTS_TARGETED, guests.ToArray());
				}
			}
		}
	}
}