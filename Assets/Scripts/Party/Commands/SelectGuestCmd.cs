using System;
using Core;

namespace Ambition
{
	public class SelectGuestCmd : ICommand<GuestVO>
	{
		public void Execute (GuestVO guest)
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
			RemarkVO remark = model.Remark;
            if (remark != null)
            {
                GuestVO[] guests = model.Guests;
                int index = Array.IndexOf(guests, guest);
                if (index >= 0)
                {
                    int num = model.Remarks.Length - 1;
                    RemarkVO[] hand = model.Remarks;
                    for (int i = Array.IndexOf(model.Remarks, remark);
                         i < num;
                         i++)
                    {
                        hand[i] = hand[i + 1];
                    }
                    hand[num] = null;
                    model.Remarks = hand;

                    num = guests.Length;
                    for (int i = remark.NumTargets - 1; i >= 0; i--)
                    {
                        guest = guests[(i + index) % num];
                        // Broadcast change to guest
                        AmbitionApp.SendMessage(PartyMessages.GUEST_SELECTED, guest);
                    }

                    // Erode the interest of guests not iteracted with
                    for (int i = num - model.Remark.NumTargets; i > 0; i--)
                    {
                        guest = guests[(index - i + num) % num];
                        AmbitionApp.SendMessage(PartyMessages.GUEST_IGNORED, guest);
                    }
                    model.Remark = null;
                }
            }
		}
	}
}
