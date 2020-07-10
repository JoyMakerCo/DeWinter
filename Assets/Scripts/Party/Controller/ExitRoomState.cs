using System;
namespace Ambition
{
    public class ExitRoomState : UFlow.UState
    {
        public override void OnEnterState()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            if (model.RequiredIncident == null) model.NextRequiredIncident();
        }
    }
}
