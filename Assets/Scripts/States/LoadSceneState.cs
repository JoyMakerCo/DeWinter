using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneState : UState<string>
    {
        string _sceneID;

        public override void SetData(string data)
        {
            _sceneID = data;
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
        }

        public override void OnEnterState()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        private void HandleFadeOutComplete()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, _sceneID);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
        }
    }
}
