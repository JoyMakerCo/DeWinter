using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneState : UState<string>
    {
        private string _sceneID;
        override public void SetData(string sceneID)
        {
            _sceneID = sceneID;
        }
        override public void OnEnterState()
        {
            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, _sceneID);
        }
    }
}
