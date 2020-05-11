using UFlow;
using Core;

namespace Ambition
{
    public class TutorialState : UState, System.IDisposable
    {
        public override void OnEnterState()
        {
            AmbitionApp.SendMessage(TutorialMessage.TUTORIAL_STEP, ID);
        }

        public virtual void Dispose()
        {
            AmbitionApp.SendMessage(TutorialMessage.TUTORIAL_STEP_COMPLETE, ID);
        }
    }
}
