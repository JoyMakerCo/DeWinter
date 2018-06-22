namespace Ambition
{
    public class FadeOutState : UFlow.UState
    {
        override public void OnEnterState()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }
    }

    public class FadeInState : UFlow.UState
    {
        override public void OnEnterState()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
