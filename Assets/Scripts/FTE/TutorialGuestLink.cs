using UnityEngine;
using UFlow;

namespace Ambition
{
    public class TutorialGuestLink : ULink
    {
        public override void Initialize()
        {
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }

        private void HandleGuest(CharacterVO guest)
        {
            Activate();
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }
    }
}
