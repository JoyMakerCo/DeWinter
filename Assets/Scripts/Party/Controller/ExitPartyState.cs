using System;
using System.Collections.Generic;
namespace Ambition
{
    public class ExitPartyState : UFlow.UState
    {
        public override void OnEnter()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            AmbitionApp.GetModel<IncidentModel>().Schedule(model.Party?.ExitIncident);
        }
    }
}
