using Core;

namespace Ambition
{
    public class RegisterIncidentControllerCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterModel<IncidentModel>();
            AmbitionApp.RegisterCommand<UpdateIncidentsCmd>(CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterCommand<ScheduleIncidentCmd, IncidentVO>(CalendarMessages.SCHEDULED);
            AmbitionApp.RegisterCommand<CompleteIncidentCmd, IncidentVO>(CalendarMessages.CALENDAR_EVENT_COMPLETED);

            // INCIDENT MACHINE
            AmbitionApp.RegisterState("IncidentController", "StartIncidentDecision"); // If there are no transitions, don't bother chaging scenes
            AmbitionApp.RegisterState("IncidentController", "StartIncidents"); // Fade out to the first incident
            AmbitionApp.RegisterState<LoadSceneState, string>("IncidentController", "LoadIncidentScene", SceneConsts.INCIDENT_SCENE); // Fade out to the first incident
            AmbitionApp.RegisterState<SendMessageState, string>("IncidentController", "ShowHeader", GameMessages.SHOW_HEADER); // Fade out to the first incident
            AmbitionApp.RegisterState<StartIncidentState>("IncidentController", "StartIncident"); // Start the incident and fade in
            AmbitionApp.RegisterState<MomentState>("IncidentController", "Moment"); // Send out moment and transition data
            AmbitionApp.RegisterState("IncidentController", "Transition"); // Multiple options; advance to the selected moment
            AmbitionApp.RegisterState<EndIncidentState>("IncidentController", "EndIncident"); // No more moments; end the incident
            AmbitionApp.RegisterState("IncidentController", "CheckNextIncident");
            AmbitionApp.RegisterState<SendMessageState, string>("IncidentController", "EndIncidents", IncidentMessages.EXIT_INCIDENTS);
            AmbitionApp.RegisterState<FadeInState>("IncidentController", "ExitIncidents");

            AmbitionApp.RegisterLink<CheckIncidentLink>("IncidentController", "StartIncidentDecision", "StartIncidents");
            AmbitionApp.RegisterLink("IncidentController", "StartIncidentDecision", "EndIncidents"); // Don't bother transitioning if there are no incidents
            AmbitionApp.RegisterLink("IncidentController", "EndIncidents", "ExitIncidents");

            AmbitionApp.RegisterLink<FadeOutLink>("IncidentController", "StartIncidents", "LoadIncidentScene");
            AmbitionApp.RegisterLink("IncidentController", "LoadIncidentScene", "ShowHeader");
            AmbitionApp.RegisterLink("IncidentController", "ShowHeader", "StartIncident");
            AmbitionApp.RegisterLink<FadeInLink>("IncidentController", "StartIncident", "Moment");
            AmbitionApp.RegisterLink<CheckTransitionLink>("IncidentController", "Moment", "Transition");
            AmbitionApp.RegisterLink<CheckEndIncidentLink>("IncidentController", "Moment", "EndIncident");
            AmbitionApp.RegisterLink("IncidentController", "Transition", "Moment");

            AmbitionApp.RegisterLink<FadeOutLink>("IncidentController", "EndIncident", "CheckNextIncident");
            AmbitionApp.RegisterLink<CheckIncidentLink>("IncidentController", "CheckNextIncident", "StartIncident");
            AmbitionApp.RegisterLink("IncidentController", "CheckNextIncident", "EndIncidents");
        }
    }
}
