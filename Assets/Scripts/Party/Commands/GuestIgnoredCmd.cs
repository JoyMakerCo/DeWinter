using System;
using Core;
using Util;

namespace Ambition
{
    public class GuestIgnoredCmd : ICommand<GuestVO>
    {
        public void Execute(GuestVO guest)
        {
            // Don't bother if the guest is already locked in
            if (guest.State == GuestState.Bored)
            {
                guest.Opinion -= AmbitionApp.GetModel<PartyModel>().BoredomPenalty;
                AmbitionApp.SendMessage(PartyMessages.BURN_REMARKS, 1);
                AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_BORED, guest);
            }
        }
    }
}
