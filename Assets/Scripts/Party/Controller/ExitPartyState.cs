using System;
namespace Ambition
{
    public class ExitPartyState : UFlow.UState
    {
        public override void OnEnterState()
        {
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            incidentModel.Schedule(model.Party.ExitIncident);
        }
    }
}
