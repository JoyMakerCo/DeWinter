using System;
using Core;

namespace Ambition
{
    public class OffendGuestCmd : ICommand<GuestVO>
    {
        public void Execute(GuestVO guest)
        {
            guest.Opinion = 0;
            if (guest.State != GuestState.PutOff)
                guest.State = GuestState.PutOff;
            else
            {
                ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
                int burn = model.Deck.Count < 5 ? model.Deck.Count : 5;
                RemarkVO remark;
                RemarkVO[] remarks;
                guest.State = GuestState.Offended;
                AmbitionApp.SendMessage(PartyMessages.BURN_REMARKS, burn);
                for (burn = 5 - burn; burn > 0; burn--)
                {
                    remarks = Array.FindAll(model.Remarks, r => r != null);
                    remark = Util.RNG.TakeRandom(remarks);
                    AmbitionApp.SendMessage(PartyMessages.DISCARD, remark);
                }
            }
        }
    }
}
