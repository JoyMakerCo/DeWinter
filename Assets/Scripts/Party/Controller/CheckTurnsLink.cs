using UFlow;
namespace Ambition
{
    public class CheckTurnsLink : ULink
    {
        public override bool Validate()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            return model.Turn < model.Turns;
        }
    }
}
