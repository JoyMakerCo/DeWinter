using System;
using UFlow;
namespace Ambition
{
    public class TutorialState : UState, Core.IState
    {
        public override void OnEnter() => AmbitionApp.SendMessage(TutorialMessages.TUTORIAL_STEP, ID);
    }
}
