using UFlow;

namespace Ambition
{
    public class GuestSelectedLink : ULink, Util.IInitializable, System.IDisposable
    {
        public override bool Validate() => false;

        public void Initialize()
        {
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleSelect);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleSelect);
        }

        private void HandleSelect(CharacterVO guest)
        {
            if (guest != null) Activate();
        }
    }
}
