using UnityEngine;
using UFlow;

namespace Ambition
{
    public class TutorialGuestLink : ULink
    {
        public override void Initialize()
        {
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }

        private void HandleGuest(GuestVO guest)
        {
            Activate();
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }
    }
}
