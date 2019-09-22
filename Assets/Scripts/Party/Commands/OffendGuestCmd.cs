using System;
using Core;

namespace Ambition
{
    public class OffendGuestCmd : ICommand<CharacterVO>
    {
        public void Execute(CharacterVO guest)
        {
/*            guest.Opinion = 0;
            if (guest.State != GuestState.PutOff)
                guest.State = GuestState.PutOff;
            else
            {
                PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
                ConversationModel conversationModel = AmbitionApp.GetModel<ConversationModel>();
                int burn = partyModel.Deck.Count < partyModel.OffendedRemarkPenalty ? partyModel.Deck.Count : 5;
                RemarkVO remark;
                RemarkVO[] remarks;
                guest.State = GuestState.Offended;
                AmbitionApp.SendMessage(PartyMessages.GUEST_LEFT, guest);
                AmbitionApp.SendMessage(PartyMessages.BURN_REMARKS, burn);
                for (burn = partyModel.OffendedRemarkPenalty - burn; burn > 0; burn--)
                {
                    remarks = Array.FindAll(conversationModel.Remarks, r => r != null);
                    remark = Util.RNG.TakeRandom(remarks);
                    AmbitionApp.SendMessage(PartyMessages.DISCARD, remark);
                }
            }
                */
        }
    }
}
