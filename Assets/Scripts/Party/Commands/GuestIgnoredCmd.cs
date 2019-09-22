using System;
using Core;
using Util;

namespace Ambition
{
    public class GuestIgnoredCmd : ICommand<CharacterVO>
    {
        public void Execute(CharacterVO guest)
        {
            //// Don't bother if the guest is already locked in
            //if (guest.State == GuestState.Bored)
            //{
            //    PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            //    guest.Opinion -= partyModel.BoredomPenalty;
            //    AmbitionApp.SendMessage(PartyMessages.BURN_REMARKS, partyModel.BoredomRemarkPenalty);
            //    AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_BORED, guest);
            //}
        }
    }
}
