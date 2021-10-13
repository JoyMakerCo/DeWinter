using System;
using UFlow;
namespace Ambition
{
    public class TestIncidentFlow : UFlowConfig
    {
        public override void Configure()
        {
            State("IncidentScene");
            State("IncidentTest");
            State("Moment"); // Send out moment and transition data
            State("OptionInput");
            State("Moment?"); // Multiple options; advance to the selected moment
            State("EndIncident", false); // No more moments; end the incident
            State("StopMusic");
            State("StopAmbient");
            State("Quit");

            Link("Moment?", "Moment");
            Link("Moment?", "EndIncident");

            Decision("Moment?", () => AmbitionApp.GetModel<IncidentModel>().Moment != null);

            Bind<LoadSceneInput, string>("IncidentScene", SceneConsts.INCIDENT_SCENE);
            Bind<StartIncidentState>("IncidentTest");
            Bind<MomentState>("Moment");
            Bind<MessageInput, string>("OptionInput", IncidentMessages.TRANSITION);
            Bind<ExitTestIncident>("EndIncident");
            Bind<MessageState, string>("StopMusic", AudioMessages.STOP_MUSIC);
            Bind<MessageState, string>("StopAmbient", AudioMessages.STOP_AMBIENT);
            Bind<UMachine>("Quit", FlowConsts.DAY_FLOW_CONTROLLER);
        }

        public class ExitTestIncident : UState
        {
            public override void OnEnter() => AmbitionApp.Story.SetTestIncident(null);
        }
    }
}
