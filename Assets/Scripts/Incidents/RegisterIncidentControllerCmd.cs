using Core;

namespace Ambition
{
    public class RegisterIncidentControllerCmd : UFlow.UFlowConfig
    {
        public override void Initialize()
        {
            State("StartIncidentsDecision"); // If there are no queued incidents, don't bother chaging scenes
            State("StartIncidents"); // Fade out to the first incident
            State("StartIncident"); // Start the incident and fade in
            State("Moment"); // Send out moment and transition data
            State("Transition"); // Multiple options; advance to the selected moment
            State("EndIncident"); // No more moments; end the incident
            State("CheckNextIncident");
            State("IncidentTransition");
            State("Exit", false);

            Link("StartIncidentsDecision", "Exit"); // Don't bother transitioning if there are no incidents
            Link("CheckNextIncident", "Exit"); // Don't bother transitioning if there are no incidents
            Link("Transition", "Moment");

            BindLink<CheckIncidentLink>("StartIncidentsDecision", "StartIncidents");
            BindLink<LoadSceneLink, string>("StartIncidents", "StartIncident", SceneConsts.INCIDENT_SCENE);
            BindLink<MessageLink, string>("Moment", "Transition", IncidentMessages.TRANSITION);
            BindLink<CheckMomentDecision>("Transition", "Moment");
            BindLink<FadeOutLink>("EndIncident", "CheckNextIncident");
            BindLink<CheckIncidentLink>("CheckNextIncident", "IncidentTransition");
            BindLink<FadeInLink>("IncidentTransition", "StartIncident");

            Bind<StartIncidentState>("StartIncident");
            Bind<MomentState>("Moment");
            Bind<EndIncidentState>("EndIncident");
        }
    }
}
