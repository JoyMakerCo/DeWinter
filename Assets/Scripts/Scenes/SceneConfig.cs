using System;
using UnityEngine;
using System.Collections.Generic;

namespace Ambition
{
    public class SceneConfig : ScriptableObject, AmbitionEditor.ILocalizedAsset
    {
        public SceneFab[] Scenefabs;

        public bool GetSceneFab(string id, out SceneFab sceneFab)
        {
            for (int i=Scenefabs.Length-1; i>=0; --i)
            {
                sceneFab = Scenefabs[i];
                if (sceneFab.ID == id) return true;
            }
            sceneFab = default;
            return false;
        }

        [Serializable]
        public struct SceneFab
        {
            public string ID;
            public GameObject Prefab;
            public bool ShowHeader;
            public float FadeInTime;
            public float FadeOutTime;
            public string HeaderTitle;
            public FMODEvent Music;
            public FMODEvent Ambient;
        }
#if UNITY_EDITOR
        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (SceneFab scene in Scenefabs)
            {
                if (scene.ShowHeader && !string.IsNullOrEmpty(scene.HeaderTitle) && scene.Prefab != null)
                {
                    result[SceneConsts.SCENE_LOC + scene.Prefab.name] = scene.HeaderTitle;
                }
            }
            return result;
        }

        [UnityEditor.MenuItem("Ambition/Create/SceneConfig")]
        public static void CreateSceneConfig()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<SceneConfig>("SceneConfig");
        }
#endif
    }
}
