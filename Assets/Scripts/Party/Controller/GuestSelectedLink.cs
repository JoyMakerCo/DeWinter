using UFlow;

namespace Ambition
{
    public class GuestSelectedLink : ULink
    {
        override public void Initialize()
        {
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleSelect);
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleSelect);
        }

        private void HandleSelect(CharacterVO guest)
        {
            if (guest != null) Activate();
        }
    }
}
