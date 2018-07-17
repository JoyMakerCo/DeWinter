using System;
using UFlow;

namespace Ambition
{
    public class SelectGuestsLink : ULink
    {
        override public void Initialize()
        {
            AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleGuests);
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleGuests);
        }

        private void HandleGuests(GuestVO [] guests)
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            if (guests == null) Array.ForEach(model.Guests, g=>g.Interest--);
            else foreach(GuestVO guest in model.Guests)
            {
                guest.Interest = Array.IndexOf(guests, guest) >= 0
                    ? guest.MaxInterest
                    : guest.Interest > 0
                    ? guest.Interest-1 : 0;
            }
            Activate();
        }
    }
}
