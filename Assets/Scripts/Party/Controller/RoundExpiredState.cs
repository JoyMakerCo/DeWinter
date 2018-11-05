using System;
using UFlow;

namespace Ambition
{
    public class RoundExpiredState : UState
    {
        public override void OnEnterState()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            GuestVO[] guests = Array.FindAll(model.Guests, g => !g.IsLockedIn && g.State != GuestState.Bored);
            if (guests.Length > 0)
            {
                GuestVO g = Util.RNG.TakeRandom(guests);
                g.State = GuestState.Bored;
                AmbitionApp.SendMessage(PartyMessages.GUEST_REACTION_BORED, g);
            }
        }
    }
}
