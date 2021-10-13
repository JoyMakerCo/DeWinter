using System;
using UFlow;
namespace Ambition
{
    public class PartyTutorialController : UFlow.UFlowConfig
    {
        public override void Configure()
        {
            State("PartyTutorialStart");
            State("PartyTutorialIncidentInput");
            State("PartyTutorialIncident");
            State("PartyTutorialEndIncidentInput");
            State("PartyTutorialTimer");
            State("PartyTutorialTimerInput");
            State("PartyTutorialEnd");

            Bind<TutorialState>("PartyTutorialStart");
            Bind<MessageInput, string>("PartyTutorialIncidentInput", PartyMessages.SHOW_ROOM);
            Bind<TutorialState>("PartyTutorialIncident");
            Bind<TutorialMapLoadedInput>("PartyTutorialEndIncidentInput");
            Bind<TutorialState>("PartyTutorialTimer");
            Bind<MessageInput, string>("PartyTutorialTimerInput", TutorialMessages.TUTORIAL_NEXT_STEP);
            Bind<EndTutorialState>("PartyTutorialEnd");
        }
    }

    public class TutorialMapLoadedInput : UInput, IDisposable
    {
        public TutorialMapLoadedInput() => AmbitionApp.Subscribe<int>(PartyMessages.TURN, HandleTurn);
        public void Dispose() => AmbitionApp.Unsubscribe<int>(PartyMessages.TURN, HandleTurn);
        private void HandleTurn(int turn) => Activate();
    }
}