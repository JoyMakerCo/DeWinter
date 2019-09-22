namespace Ambition
{
    public class LoadParisIncidentState : UFlow.UState
    {
        public override void OnEnterState(string[] args)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            if (paris.Location?.IntroIncidentConfig != null)
                calendar.Schedule(paris.Location.IntroIncidentConfig.GetIncident(), calendar.Today);
        }
    }
}
