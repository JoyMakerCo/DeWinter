using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneState : UState<string>
    {
        string _sceneID;

        public override void SetData(string node) => _sceneID = node;
        override public void OnEnterState()
        {
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
        }

        private void HandleFadeOutComplete()
        {
            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, _sceneID);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
