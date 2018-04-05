using UnityEngine;
using UFlow;

namespace Ambition
{
    public class TutorialGuestLink : ULink
    {
        public override bool InitializeAndValidate()
        {
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
            return false;
        }

        private void HandleGuest(GuestVO guest)
        {
            Validate();
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }
    }
}
