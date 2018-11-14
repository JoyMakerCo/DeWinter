using UFlow;
namespace Ambition
{
    public class FadeOutLink:ULink
    {
        public override void Initialize()
        {
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
        }
    }
}
