using System;
namespace Ambition
{
    public class EndTutorialState : UFlow.UState
    {
        public override void OnEnter()
        {
            AmbitionApp.SendMessage(TutorialMessages.TUTORIAL_STEP, ID);
            AmbitionApp.SendMessage(TutorialMessages.END_TUTORIAL_STEP, _Flow?.FlowID);
            AmbitionApp.SendMessage(TutorialMessages.END_TUTORIAL, _Flow?.FlowID);
        }
    }
}
