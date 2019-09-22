#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

namespace Core
{
    public class LocalizationConfig : ScriptableObject
    {
        public TextAsset DefaultLocalizationFile;

        [SerializeField, HideInInspector]
        private Dictionary<string, string> _phrases = null;

        private bool Pull(bool overwrite = false)
        {
            if (DefaultLocalizationFile != null && (overwrite || _phrases == null))
            {
                _phrases = JsonConvert.DeserializeObject<Dictionary<string, string>>(DefaultLocalizationFile.text);
            }
            return _phrases != null;
        }

        public string GenerateLocalizationKey(UnityEngine.Object obj) => ( obj.GetInstanceID() + "."  + obj.name + "." );

        public bool Post(UnityEngine.Object obj, Dictionary<string, string> phrases, bool removeUnused = false)
        {
            if (!Pull() || phrases == null) return false;
            string key = GenerateLocalizationKey(obj);
            if (removeUnused)
            {
                Dictionary<string, string> currPhrases = GetPhrases(obj);
                int len = key.Length;
                foreach (string current in currPhrases.Keys)
                {
                    if (!phrases.ContainsKey(current))
                        _phrases.Remove(key+current);
                }
            }
            foreach (KeyValuePair<string, string> k in phrases)
                _phrases[key + k.Key.ToString()] = k.Value;
            return true;
        }

        public bool Post(string key, string phrase)
        {
            if (!Pull()) return false;
            _phrases[key] = phrase;
            return true;
        }

        public bool Post(UnityEngine.Object obj, string phrase)
        {
            if (!Pull()) return false;
            _phrases[GenerateLocalizationKey(obj)] = phrase;
            return true;
        }

        public Dictionary<string, string> GetPhrases(UnityEngine.Object obj)
        {
            if (!Pull()) return null;
            string key = GenerateLocalizationKey(obj);
            int count = key.Length;
            return _phrases.Where(k => k.Key.StartsWith(key)).ToDictionary(k => k.Key.Substring(count), k => k.Value);
        }

        public string GetPhrase(UnityEngine.Object obj)
        {
            if (!Pull()) return null;
            string key = GenerateLocalizationKey(obj);
            _phrases.TryGetValue(key, out string phrase);
            return phrase?.Substring(key.Length);
        }

        public void MoveKey(string fromKey, string toKey)
        {
            if (fromKey == toKey
                || string.IsNullOrWhiteSpace(fromKey)
                || string.IsNullOrWhiteSpace(toKey)
                || !Pull()) return;

            int len = fromKey.Length;
            if (_phrases.ContainsKey(fromKey))
            {
                _phrases[toKey] = _phrases[fromKey];
            }
            _phrases.Remove(fromKey);
            KeyValuePair<string, string>[] phrases = _phrases
                .Where(p => p.Key.StartsWith(fromKey)).ToArray();
            foreach (KeyValuePair<string, string> kvp in phrases)
            {
                _phrases.Remove(kvp.Key);
                _phrases[toKey + kvp.Key.Substring(len)] = kvp.Value;
            }
        }

        public string UpdateLocalizationKey(UnityEngine.Object obj, string previousKey)
        {
            string key = GenerateLocalizationKey(obj);
            MoveKey(previousKey, key);
            return key;
        }

        public void UpdateFile()
        {
            if (_phrases != null && DefaultLocalizationFile != null)
            {
                string fileText = JsonConvert.SerializeObject(_phrases, Formatting.Indented);
                string path = AssetDatabase.GetAssetPath(DefaultLocalizationFile);
                File.WriteAllText(path, fileText);
            }
        }

        [UnityEditor.MenuItem("Assets/Create/Localization Config")]
        public static void CreateLocalizationConfig()
        {
            Util.ScriptableObjectUtil.CreatUniqueInstance<LocalizationConfig>("Localization Config");
        }

    }

    [CustomEditor(typeof(LocalizationConfig))]
    public class LocalizationConfigInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Update File"))
            {
                (target as LocalizationConfig)?.UpdateFile();
            }
        }
    }
}
#endif
