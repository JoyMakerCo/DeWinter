using System;
using System.Collections;
using UnityEngine;
namespace Ambition
{
    public class SceneMediator : MonoBehaviour
    {
        public SceneConfig Config;
        public GameObject SceneRoot;

        public string SceneID { get; private set; } = "START_SCENE";
        public GameObject SceneObject;
        private string _queuedSceneID;
        private bool _inTransition = false;

        private void Start()
        {
            AmbitionApp.Subscribe<string>(GameMessages.LOAD_SCENE, HandleScene);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOut);
            AmbitionApp.Subscribe(GameMessages.FADE_IN_COMPLETE, HandleFadeIn);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);

            SceneView start = SceneObject?.GetComponent<SceneView>();
            (start as Util.IInitializable)?.Initialize();
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(GameMessages.LOAD_SCENE, HandleScene);
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOut);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN_COMPLETE, HandleFadeIn);
        }

        private void HandleScene(string sceneID)
        {
            _queuedSceneID = sceneID;
            if (!_inTransition)
            {
                if (Config.GetSceneFab(_queuedSceneID, out SceneConfig.SceneFab scenefab))
                {
                    AmbitionApp.SendMessage(GameMessages.FADE_OUT, scenefab.FadeOutTime);
                }
                else
                {
                    AmbitionApp.SendMessage(GameMessages.FADE_OUT);
                }
                AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
                if (Config.GetSceneFab(SceneID, out SceneConfig.SceneFab currScene))
                {
                    if (scenefab.Music.Name != currScene.Music.Name)
                    {
                        AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
                    }
                }
                _inTransition = true;
            }
        }

        private void HandleFadeOut()
        {
            if (!string.IsNullOrEmpty(_queuedSceneID) && Config.GetSceneFab(_queuedSceneID, out SceneConfig.SceneFab scenefab))
            {
                AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
                Config.GetSceneFab(SceneID, out SceneConfig.SceneFab oldScene);
                _inTransition = false;
                StopAllCoroutines();
                foreach (Transform child in SceneRoot.transform)
                {
                    Destroy(child.gameObject);
                }
                SceneID = _queuedSceneID;
                _queuedSceneID = null;

                (SceneObject?.GetComponent<SceneView>() as IDisposable)?.Dispose();
                SceneObject = Instantiate(scenefab.Prefab, SceneRoot.transform);
                (SceneObject?.GetComponent<SceneView>() as Util.IInitializable)?.Initialize();

                if (scenefab.ShowHeader)
                {
                    if (string.IsNullOrEmpty(scenefab.HeaderTitle))
                    {
                        AmbitionApp.SendMessage(GameMessages.SHOW_HEADER);
                    }
                    else
                    {
                        AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, SceneConsts.SCENE_LOC + scenefab.Prefab.name);
                    }
                }
                else AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
                if (oldScene.Music.Name != scenefab.Music.Name)
                {
                    AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, scenefab.Music);
                }
                AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENT, scenefab.Ambient);
                AmbitionApp.SendMessage(GameMessages.FADE_IN, scenefab.FadeInTime);
                AmbitionApp.SendMessage(GameMessages.SCENE_LOADED, SceneID);
            }
        }

        private void HandleFadeIn() => _inTransition = false;
    }
}
