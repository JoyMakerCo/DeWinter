namespace Ambition
{
    public class LoadParisIncidentState : UFlow.UState
    {
        public override void OnEnterState()
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            AmbitionApp.GetModel<IncidentModel>().Schedule(paris.Location?.IntroIncident);
        }
    }
}
