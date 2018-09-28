using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class PlayerConfig : ScriptableObject
    {
        public IncidentConfig[] Incidents;
        public string Description;
        public Sprite Portrait;
        public Sprite Doll;

#if (UNITY_EDITOR)
        [UnityEditor.MenuItem("Assets/Create/Create Player")]
        public static void CreatePlayer()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<PlayerConfig>("New Player");
        }
#endif
    }
}
