using System;
using UFlow;
namespace Ambition
{
    public class CheckPartyLink : ULink
    {
        public override bool Validate()
        {
            return Array.Exists(AmbitionApp.GetModel<CalendarModel>().GetEvents<PartyVO>(), p => p.Attending);
        }
    }
}
