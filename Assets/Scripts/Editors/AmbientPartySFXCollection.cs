using System;
using System.Linq;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{
    [Serializable]
    public struct AmbientPartySFX
    {
        public FMODEvent Event;
        public bool Indoors;
        public int Importance;
    }

    public class AmbientPartySFXCollection : ScriptableObject
    {
        public AmbientPartySFX[] AmbientPartySFXs;

        public FMODEvent GetFMODEvent(bool indoors, int importance)
        {
            foreach (AmbientPartySFX a in AmbientPartySFXs)
            {
                if (a.Indoors == indoors && a.Importance == importance)
                {
                    return a.Event;
                }
            }
            return AmbientPartySFXs[0].Event; // If all else fails, just throw out the indoors trivial SFX
        }

#if (UNITY_EDITOR)
        [MenuItem("Assets/Create/Create AmbientPartySFX Collection")]
        public static void CreateAvatarConfig()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<AmbientPartySFXCollection>();
        }
#endif
    }
}
