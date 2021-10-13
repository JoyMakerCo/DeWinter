using System;
namespace Ambition
{
    public class ExitRoomState : UFlow.UState
    {
        public override void OnEnter()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            if (model.RequiredIncident == null) model.NextRequiredIncident();
            model.Incidents = null;
        }
    }
}
