using System;
using UFlow;

namespace Ambition
{
    public class LoadSceneInput : UInput, Util.IInitializable<string>, IDisposable
    {
        public string SceneID { get; protected set; }
        public LoadSceneInput() => AmbitionApp.Subscribe<string>(GameMessages.SCENE_LOADED, HandleSceneLoaded);
        public virtual void Initialize(string sceneID) => SceneID = sceneID;
        public override void OnEnter()
        {
            if (string.IsNullOrEmpty(SceneID)) throw new ArgumentNullException("LoadSceneInput: Null Argment found");
            AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, SceneID);
        }
        public void Dispose() => AmbitionApp.Unsubscribe<string>(GameMessages.SCENE_LOADED, HandleSceneLoaded);

        private void HandleSceneLoaded(string sceneID)
        {
            if (sceneID == SceneID)
                Activate();
        }
    }
}
