using System;
using UFlow;

namespace Ambition
{
    public class SelectGuestLink : ULink
    {
        override public void Initialize()
        {
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.SELECT_GUEST, HandleGuest);
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.SELECT_GUEST, HandleGuest);
        }

        private void HandleGuest(GuestVO guest)
        {
            if (guest != null) Activate();
        }
    }
}
