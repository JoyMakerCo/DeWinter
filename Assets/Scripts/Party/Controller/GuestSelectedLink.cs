using UFlow;

namespace Ambition
{
    public class GuestSelectedLink : ULink
    {
        override public void Initialize()
        {
            AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleGuestsSelected);
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleGuestsSelected);
        }

        private void HandleGuestsSelected(GuestVO [] guests)
        {
            if (guests != null) Activate();
        }
    }
}