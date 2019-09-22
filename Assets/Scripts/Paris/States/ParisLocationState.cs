using System;
using UFlow;

namespace Ambition
{
    public class ParisLocationState : UState
    {
        public override void OnEnterState(string[] args)
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, paris.Location?.Scene);
        }
    }
}
