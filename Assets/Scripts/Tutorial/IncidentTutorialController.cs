using System;
using UFlow;
namespace Ambition
{
    public class IncidentTutorialController : UFlow.UFlowConfig
    {
        public const string CONTROLLER_ID = "IncidentTutorialController";

        public override void Configure()
        {
            State("IncidentTutorialStart");
            State("IncidentTutorialNoOp");
            State("IncidentTutorialNoOpInput");
            State("IncidentTutorialWaitOptions");
            State("IncidentTutorialWaitOptionsInput");
            State("IncidentTutorialOptions");
            State("IncidentTutorialOptionsInput");
            State("EndTutorial");

            Bind<MessageInput, string>("IncidentTutorialNoOpInput", IncidentMessages.TRANSITION);
            Bind<IncidentTutorialOptionsInput>("IncidentTutorialWaitOptionsInput");
            Bind<MessageInput, string>("IncidentTutorialOptionsInput", IncidentMessages.TRANSITION);

            Bind<TutorialState>("IncidentTutorialNoOp");
            Bind<TutorialState>("IncidentTutorialWaitOptions");
            Bind<TutorialState>("IncidentTutorialOptions");
            Bind<EndTutorialState>("EndTutorial");
        }
    }

    public class IncidentTutorialOptionsInput : UInput, IDisposable
    {
        public IncidentTutorialOptionsInput()
        {
            AmbitionApp.Subscribe<TransitionVO[]>(CheckMomentDecision);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<TransitionVO[]>(CheckMomentDecision);
        }

        private void CheckMomentDecision(TransitionVO[] xitions)
        {
            if (xitions?.Length > 1) Activate();
        }
    }
}
