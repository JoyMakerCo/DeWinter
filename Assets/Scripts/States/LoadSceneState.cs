using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneState : UState<string>
    {
        string _sceneID;

        public override void SetData(string data) => _sceneID = data;

        public override void OnEnterState(string[] args)
        {
            //_sceneID = args[0];
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
