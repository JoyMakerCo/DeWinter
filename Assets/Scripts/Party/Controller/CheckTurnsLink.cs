using System;
namespace Ambition
{
    public class CheckTurnsLink : UFlow.ULink
    {
        public override bool Validate()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            ++model.Turn;
            return model.TurnsLeft > 0;
        }
    }
}
