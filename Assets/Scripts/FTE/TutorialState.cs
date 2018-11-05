using UFlow;
using Core;

namespace Ambition
{
    public class TutorialState : UState
    {
        override public void OnEnterState()
        {
            AmbitionApp.SendMessage(TutorialMessage.TUTORIAL_STEP, ID);
        }

        override public void OnExitState()
        {
            AmbitionApp.SendMessage(TutorialMessage.TUTORIAL_STEP_COMPLETE, ID);
        }
    }
}
