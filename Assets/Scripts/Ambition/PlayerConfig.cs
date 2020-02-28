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

    public class PlayerConfig : ScriptableObject, AmbitionEditor.ILocalizedAsset
    {
        public IncidentConfig[] Incidents;
        public ChapterConfig[] Chapters;
        public InventoryItem[] Inventory;
        public string Description;
        public Sprite Portrait;
        public Sprite Doll;
        public int Livre;

        [SerializeField] private string _localizationKey;

        public string GetLocalizationKey() => _localizationKey;

#if (UNITY_EDITOR)
        public void SetLocalizationKey(string value)
        {
            SerializedObject obj = new SerializedObject(this);
            obj.FindProperty("_localizationKey").stringValue = value;
            obj.ApplyModifiedProperties();
        }

        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            return result;
        }

        [UnityEditor.MenuItem("Assets/Create/Player")]
        public static void CreatePlayer() => Util.ScriptableObjectUtil.CreateScriptableObject<PlayerConfig>("New Player Character");
#endif
    }
}
