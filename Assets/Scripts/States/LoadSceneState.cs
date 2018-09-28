using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneState : UState<string>
    {
        string _sceneID;

        public override void SetData(string sceneID) => _sceneID = sceneID;
        override public void OnEnterState()
        {
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        private void HandleFadeOutComplete()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, _sceneID);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
