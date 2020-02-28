namespace Ambition
{
    public class LoadParisIncidentState : UFlow.UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            calendar.Schedule(paris.Location?.IntroIncident, calendar.Today);
        }
    }
}
