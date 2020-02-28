using System;
namespace Ambition
{
    public class ReturnToEstateState : UFlow.UState
    {
        public override void OnEnterState()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            model.LocationID = null;
            model.Location = null;
        }
    }
}
