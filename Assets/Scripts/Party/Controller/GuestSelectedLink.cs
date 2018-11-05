using UFlow;

namespace Ambition
{
    public class GuestSelectedLink : ULink
    {
        override public void Initialize()
        {
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.SELECT_GUEST, HandleSelect);
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.SELECT_GUEST, HandleSelect);
        }

        private void HandleSelect(GuestVO guest)
        {
            if (guest != null) Activate();
        }
    }
}