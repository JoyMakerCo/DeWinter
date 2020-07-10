using System;
using UFlow;
namespace Ambition
{
    public class CheckPartyLink : ULink
    {
        public override bool Validate()
        {
            OccasionVO[] occasions = AmbitionApp.GetModel<CalendarModel>().GetOccasions(OccasionType.Party);
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            foreach (OccasionVO occasion in occasions)
            {
                if (model.Parties.TryGetValue(occasion.ID, out PartyVO party) && (party?.Attending ?? false))
                    return true;
            }
            return false;
        }
    }
}
