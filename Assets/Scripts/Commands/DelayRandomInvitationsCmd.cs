using System;
using Core;

namespace Ambition
{
    public class DelayRandomInvitationsCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.UnregisterCommand<DelayRandomInvitationsCmd>(CalendarMessages.BEGIN_RANDOM_INVITATIONS);
            AmbitionApp.RegisterCommand<GenerateRandomInvitationsCmd>(CalendarMessages.UPDATE_CALENDAR);
        }
    }
}
