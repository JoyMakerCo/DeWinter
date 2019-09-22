using System;
namespace Ambition
{
    public class ExitPartyState : UFlow.UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            calendar.Schedule(model.Party.ExitIncident);
        }
    }
}
