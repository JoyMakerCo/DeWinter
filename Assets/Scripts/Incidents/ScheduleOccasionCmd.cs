using Core;
using System;

namespace Ambition
{
    public class ScheduleIncidentCmd : ICommand<IncidentVO>
    {
        public void Execute(IncidentVO incident)
        {
            AmbitionApp.GetModel<IncidentModel>().Schedule(incident);
        }
    }

    public class SchedulePartyCmd : ICommand<PartyVO>
    {
        public void Execute(PartyVO party)
        {
            AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
            AmbitionApp.GetModel<PartyModel>().Schedule(party);
        }
    }
}
