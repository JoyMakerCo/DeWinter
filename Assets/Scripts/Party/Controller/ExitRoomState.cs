using System;
namespace Ambition
{
    public class ExitRoomState : UFlow.UState
    {
        public override void OnEnterState()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            if (model.GetRequiredIncident() == null) model.NextRequiredIncident();
        }
    }
}
