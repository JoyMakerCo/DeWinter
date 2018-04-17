using UFlow;

namespace Ambition
{
    public class FadeInState : UState
    {
        override public void OnEnterState()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
