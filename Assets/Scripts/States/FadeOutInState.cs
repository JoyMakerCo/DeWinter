using UFlow;
namespace Ambition
{
    public class FadeOutInState : UInputState
    {
        public override void Initialize(object[] parameters)
        {
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
