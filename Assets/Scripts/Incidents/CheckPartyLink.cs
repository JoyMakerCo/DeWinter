using UFlow;
namespace Ambition
{
    public class CheckPartyLink : ULink
    {
        public override bool Validate()
        {
            return AmbitionApp.GetModel<PartyModel>().Party != null;
        }
    }
}
