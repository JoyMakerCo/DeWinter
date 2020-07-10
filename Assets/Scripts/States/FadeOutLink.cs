using UFlow;
namespace Ambition
{
    public class FadeOutLink:ULink, Util.IInitializable, System.IDisposable
    {
        public override bool Validate() => false;

        public void Initialize()
        {
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
            AmbitionApp.Subscribe(GameMessages.FADE_IN, Activate);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN, Activate);
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
        }
    }
}
