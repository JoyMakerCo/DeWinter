using UFlow;
namespace Ambition
{
    public class FadeOutInLink : ULink, Util.IInitializable, System.IDisposable
    {
        public override bool Validate() => false;

        public void Initialize()
        {
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
