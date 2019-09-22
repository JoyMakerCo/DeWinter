using UFlow;
using Core;

namespace Ambition
{
    public class TutorialState : UState
    {
        public override void OnEnterState(string[] args)
        {
            AmbitionApp.SendMessage(TutorialMessage.TUTORIAL_STEP, ID);
        }

        public override void Cleanup()
        {
            AmbitionApp.SendMessage(TutorialMessage.TUTORIAL_STEP_COMPLETE, ID);
            base.Cleanup();
        }
    }
}
