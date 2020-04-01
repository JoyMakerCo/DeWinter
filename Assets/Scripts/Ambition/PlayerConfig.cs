using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ambition
{
    [Serializable]
    public class ChapterConfig
    {
        public string Name;
        public Util.DateConfig Date;
        public Sprite Splash;
        public FMODEvent Sting;
    }

    public class PlayerConfig : ScriptableObject
    {
        public IncidentConfig[] Incidents;
        public ChapterConfig[] Chapters;
        public InventoryItem[] Inventory;
        public string Description;
        public Sprite Portrait;
        public Sprite Doll;
        public int Livre;

#if (UNITY_EDITOR)
        [UnityEditor.MenuItem("Assets/Create/Player")]
        public static void CreatePlayer() => Util.ScriptableObjectUtil.CreateScriptableObject<PlayerConfig>("New Player Character");
#endif
    }
}
