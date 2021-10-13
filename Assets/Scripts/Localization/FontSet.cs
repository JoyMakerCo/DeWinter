using System;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class FontSet : ScriptableObject
    {
        public Font Default;
        public FontSubstitution[] Fonts;

        [Serializable]
        public struct FontSubstitution
        {
            public Font ProxyFont;
            public Font SubstitutionFont;
        }

#if (UNITY_EDITOR)
        [UnityEditor.MenuItem("Assets/Create/Create Font Set")]
        public static void CreateParty()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<FontSet>("New Font Set");
        }
#endif
    }
}
