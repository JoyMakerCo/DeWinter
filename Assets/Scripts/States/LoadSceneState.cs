using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneState : UInputState
    {
        private string _sceneID;

        public override void Initialize(params object[] parameters) => _sceneID = parameters[0] as string;
        public override void OnEnterState()
        {
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFade);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFade);
        }

        private void HandleFade()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFade);
            AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, _sceneID);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
            Activate();
        }
    }
}
