using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneState : UState, Util.IInitializable<string>
    {
        private string _sceneID;
        public void Initialize(string sceneID)
        {
            _sceneID = sceneID;
        }
        override public void OnEnterState()
        {
            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, _sceneID);
        }
    }
}
