using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{

    public class GuestSFXCollection : ScriptableObject
    {
        [Serializable]
        public struct GuestSFX
        {
            public FMODEvent Event;
            public Gender Gender;
            public string Reaction;
        }

        public GuestSFX[] GuestSFXes;

        public FMODEvent GetFMODEvent(Gender gender, string reaction)
        {
            List<GuestSFX> tempSFX = new List<GuestSFX>();
            foreach (GuestSFX g in GuestSFXes)
            {
                if (g.Gender == gender && g.Reaction == reaction)
                {
                    tempSFX.Add(g);
                }
            }
            return tempSFX[UnityEngine.Random.Range(0, tempSFX.Count())].Event; // Returns a randomly chosen event from the list, or just that that particular event, if the list consists of 1 SFX
        }

        #if (UNITY_EDITOR)
        [MenuItem("Assets/Create/Create Guest SFX Collection")]
        public static void CreateGuestSFXCollection()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<GuestSFXCollection>();
        }
        #endif
    }
}
