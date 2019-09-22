using UFlow;
namespace Ambition
{
    public class CheckPartyComplete : ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<PartyModel>()?.Party?.IsComplete ?? true;
    }
}
