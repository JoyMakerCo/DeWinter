using UnityEngine;
using UFlow;

namespace Ambition
{
    public class TutorialGuestLink : ULink, Util.IInitializable, System.IDisposable
    {
        public override bool Validate() => false;

        public void Initialize()
        {
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }

        private void HandleGuest(CharacterVO guest)
        {
            Activate();
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }
    }
}
