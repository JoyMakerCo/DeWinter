using System;
using Core;
using UFlow;

namespace Ambition
{
    public class RestState : UState
    {
        public override void OnEnterState()
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            model.Exhaustion.Value = model.Exhaustion.Value > 0 ? 0 : -1;
        }
    }
}
