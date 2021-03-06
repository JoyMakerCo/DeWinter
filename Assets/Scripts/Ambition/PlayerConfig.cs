﻿using System;
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

    public class PlayerConfig : ScriptableObject, AmbitionEditor.ILocalizedAsset
    {
        public string Name;
        public IncidentConfig[] Incidents;
        public ChapterConfig[] Chapters;
        public ItemConfig[] Inventory;
        public string Description;
        public Sprite Portrait;
        public Sprite Doll;
        public int Livre;

#if (UNITY_EDITOR)
        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result[name + ".name"] = Name;
            for (int i = Chapters.Length - 1; i >= 0; --i)
                result[name + ".chapter." + i] = Chapters[i].Name;
            return result;
        }

        [UnityEditor.MenuItem("Assets/Create/Player")]
        public static void CreatePlayer() => Util.ScriptableObjectUtil.CreateScriptableObject<PlayerConfig>("New Player Character");
#endif
    }
}
