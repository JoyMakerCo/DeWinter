using UFlow;
namespace Ambition
{
    public class FadeInLink:ULink
    {
        public override void Initialize()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }

        public override bool Validate() => true;
    }
}
