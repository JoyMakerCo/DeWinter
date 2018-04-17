using UFlow;

namespace Ambition
{
    public class FadeOutState : UState
    {
        override public void OnEnterState()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }
    }
}
