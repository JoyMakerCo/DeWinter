using System;
using System.Collections.Generic;
using Core;

namespace Ambition 
{
	public class TargetGuestCmd : ICommand<CharacterVO>
	{
		public void Execute (CharacterVO guest)
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            if (model == null || model.Remark == null || guest == null) // The argument guest is the one being explicitly targeted
            {
                AmbitionApp.SendMessage<CharacterVO>(PartyMessages.GUEST_TARGETED, null);
            }
            else
            {
/*                CharacterVO[] guests = model.Guests;
                int index = Array.IndexOf(guests, guest);
                if (index >= 0)
                {
                    int len = guests.Length;
                    for (int i = 0; i<model.Remark.NumTargets; i++)
                    {
                        //for (guest = guests[(i + index) % len];
                            //guest.State == GuestState.Offended;
                            // guest = guests[(i + index) % len])
                            //index++;
                        AmbitionApp.SendMessage(PartyMessages.GUEST_TARGETED, guest);
                    }
                }
                else
                {
                    AmbitionApp.SendMessage<CharacterVO>(PartyMessages.GUEST_TARGETED, null);
                }
*/
            }
        }
    }
}
