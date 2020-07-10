using UFlow;
namespace Ambition
{
    public class FadeInLink:ULink, Util.IInitializable
    {
        public void Initialize() => AmbitionApp.SendMessage(GameMessages.FADE_IN);
    }
}
