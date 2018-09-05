using System;
using UFlow;

namespace Ambition
{
    public class RestAtHomeState : UState
    {
        public override void OnEnterState()
        {
            AmbitionApp.GetModel<GameModel>().Exhaustion = 0;
        }
    }
}
