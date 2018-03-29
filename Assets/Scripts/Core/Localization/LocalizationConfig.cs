using System;
using UnityEngine;

namespace Core
{
    public class LocalizationConfig : ScriptableObject
    {
        public string GoogleSheetsKey;
        private string _json;
        internal string GetLocalizationData(string language)
        {
            return _json;
        }
        void Start()
        {

        }
#if (UNITY_EDITOR)
        public static void CreateLocalizationConfig()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<LocalizationConfig>("LocalizationConfig");
        }
#endif
    }
}