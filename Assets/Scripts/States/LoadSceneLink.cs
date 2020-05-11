using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneLink : ULink, Util.IInitializable<string>
    {
        string _sceneID;

        public void Initialize(string data)
        {
            _sceneID = data;
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
        }

        public override bool Validate()
        {
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
            return false;
        }

        private void HandleFadeOutComplete()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, _sceneID);
            Activate();
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOutComplete);
        }
    }
}
