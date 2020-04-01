namespace Ambition
{
    public class FadeOutState : UFlow.UState
    {
        public override void OnEnterState()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }
    }

    public class FadeInState : UFlow.UState
    {
        public override void OnEnterState()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
