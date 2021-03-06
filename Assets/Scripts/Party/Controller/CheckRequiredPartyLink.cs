﻿using System;
using UFlow;
namespace Ambition
{
    public class CheckRequiredPartyLink : ULink
    {
        public override bool Validate()
        {
            PartyVO[] parties = AmbitionApp.GetModel<PartyModel>().GetParties();
            return Array.Exists(parties, p => p.RSVP == RSVP.Required);
        }
    }
}
