using System;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class BackgroundConfig : ScriptableObject
    {
        public BackgroundMap[] Backgrounds;

        [Serializable]
        public struct BackgroundMap
        {
            public Sprite IncidentBackground;
            public Sprite ModalBackground;
        }

#if (UNITY_EDITOR)
        [UnityEditor.MenuItem("Assets/Create/Background Config")]
        public static void CreatePrefabConfig()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<BackgroundConfig>();
        }
#endif
    }
}
