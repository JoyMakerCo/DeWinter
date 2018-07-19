using System;
using UFlow;

namespace Ambition
{
    public class RoundExpiredState : UState
    {
        public override void OnEnterState()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            foreach (GuestVO guest in model.Guests)
            {
                if (!guest.IsLockedIn) guest.Interest--;
                if (guest.Interest <= 0)
                {
                    guest.Interest = 0;
                    guest.State = GuestState.Bored;
                }
            }
        }
    }
}
