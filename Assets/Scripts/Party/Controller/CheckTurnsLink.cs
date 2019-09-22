using System;
namespace Ambition
{
    public class CheckTurnsLink : UFlow.ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<PartyModel>().TurnsLeft > 0;
    }
}
