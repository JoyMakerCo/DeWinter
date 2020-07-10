using System;
using UFlow;
namespace Ambition
{
    public class CheckPartyLink : ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<PartyModel>().UpdateParty() != null;
    }
}
