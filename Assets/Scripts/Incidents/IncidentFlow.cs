using Core;

namespace Ambition
{
    public class IncidentFlow : UFlow.UFlowConfig
    {
        public override void Configure()
        {
            State("StartIncidents?"); // If there are no queued incidents, don't bother chaging scenes
            State("StartIncidents"); // Fade out to the first incident
            State("StartIncident"); // Start the incident and fade in
            State("Moment"); // Send out moment and transition data
            State("OptionInput");
            State("Moment?"); // Multiple options; advance to the selected moment
            State("EndIncident", false); // No more moments; end the incident
            State("NextIncident?");
            State("NextIncidentTransition");
            State("Exit", false);

            Link("StartIncidents?", "Exit"); // Don't bother transitioning if there are no incidents
            Link("NextIncident?", "Exit");
            Link("Moment?", "Moment");
            Link("Moment?", "EndIncident");
            Link("NextIncidentTransition", "StartIncident");

            Decision("StartIncidents?", CheckIncident);
            Decision("NextIncident?", CheckIncident);
            Decision("Moment?", ()=>AmbitionApp.GetModel<IncidentModel>().Moment != null);

            Bind<LoadSceneInput, string>("StartIncidents", SceneConsts.INCIDENT_SCENE);
            Bind<MessageInput, string>("OptionInput", IncidentMessages.TRANSITION);
            Bind<FadeOutIn>("NextIncidentTransition");

            Bind<StartIncidentState>("StartIncident");
            Bind<MomentState>("Moment");
            Bind<EndIncidentState>("EndIncident");
        }

        private bool CheckIncident()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            while (model.Incident != null) // Iterates through incidents that meet requirements
            {
                if (AmbitionApp.CheckIncidentEligible(model.Incident))
                {
                    return true;
                }
                model.NextIncident();
            }
            return false;
        }

        private class FadeOutIn : UFlow.UInput, System.IDisposable
        {
            public FadeOutIn() => AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, OnActivate);
            public override void OnEnter() => AmbitionApp.SendMessage(GameMessages.FADE_OUT);
            public void Dispose() => AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, OnActivate);
            private void OnActivate()
            {
                AmbitionApp.SendMessage(GameMessages.FADE_IN);
                Activate();
            }
        }
    }
}
