using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneState : UState<string>
    {
        override public void OnEnterState()
        {
            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, Data);
        }
    }
}
