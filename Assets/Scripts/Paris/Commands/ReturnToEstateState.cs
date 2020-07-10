using System;
namespace Ambition
{
    public class ReturnToEstateState : UFlow.UState
    {
        public override void OnEnterState()
        {
            AmbitionApp.GetModel<ParisModel>().Location = null;
        }
    }
}
