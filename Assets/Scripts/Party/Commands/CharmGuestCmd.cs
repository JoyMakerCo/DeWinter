using System;
using Core;

namespace Ambition
{
    public class CharmGuestCmd : ICommand<GuestVO>
    {
        public void Execute(GuestVO guest)
        {
            guest.Opinion = 100;
            guest.State = GuestState.Charmed;
            AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
            AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
        }
    }
}
