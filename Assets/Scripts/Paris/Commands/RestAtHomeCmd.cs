using System;
using Core;

namespace Ambition
{
    public class RestAtHomeState : UFlow.UState
    {
        public override void OnEnterState()
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            paris.LocationID = ParisConsts.HOME;
            paris.Location = null;

        }
    }
}
