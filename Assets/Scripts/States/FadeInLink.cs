using UFlow;
namespace Ambition
{
    public class FadeInLink:ULink
    {
        public override bool Validate()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
            return true;
        }
    }
}
