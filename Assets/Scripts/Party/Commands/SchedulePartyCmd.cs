using System;
namespace Ambition
{
    public class SchedulePartyCmd : Core.ICommand<PartyVO>
    {
        public void Execute(PartyVO party)
        {
            PartyVO[] parties = AmbitionApp.Calendar.GetOccasions<PartyVO>(party.Day);
            if (Array.IndexOf(parties, party) < 0)
            {
                AmbitionApp.Calendar.Schedule(party, party.Day);
            }
            AmbitionApp.SendMessage(party);
        }
    }
}
