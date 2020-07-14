using System;
using Core;
namespace Ambition
{
    public class DeclineInvitationCmd : ICommand<string> 
    {
        public void Execute(string partyID)
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            // You can't decline a required party.
            if (model.Parties.TryGetValue(partyID, out PartyVO party) && party?.RSVP != RSVP.Required)
            {
                party.RSVP = RSVP.Declined;
                // TODO: Any penalties for RSVP No goes here
            }
        }
    }
}
