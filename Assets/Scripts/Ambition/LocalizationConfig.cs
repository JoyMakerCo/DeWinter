#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

namespace Ambition
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

        public string GenerateLocalizationKey(ScriptableObject obj) => AssetDatabase.GetAssetPath(obj) + ".";
        public string GenerateLocalizationKey(MonoBehaviour obj) => obj.GetInstanceID() + obj.name + ".";

        public bool Post(UnityEngine.Object obj, Dictionary<string, string> phrases, bool removeUnused = false)
        {
            if (phrases == null || !Pull()) return false;
            string key = (obj is MonoBehaviour)
                ? GenerateLocalizationKey(obj as MonoBehaviour)
                : GenerateLocalizationKey(obj as ScriptableObject);
            if (removeUnused)
            {
                Dictionary<string, string> currPhrases = GetPhrases(obj);
                int len = key.Length;
                foreach (string current in currPhrases.Keys)
                {
                    if (!phrases.ContainsKey(current))
                    {
                        _phrases.Remove(key + current);
                    }
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
            string key = obj is MonoBehaviour
                ? GenerateLocalizationKey(obj as MonoBehaviour)
                : GenerateLocalizationKey(obj as ScriptableObject);
            _phrases[key] = phrase;
            return true;
        }

        public void RemovePhrases(string localizationKey)
        {
            if (Pull())
            {
                string[] keys = _phrases.Keys.Where(k => k.StartsWith(localizationKey)).ToArray();
                foreach (string key in keys)
                {
                    _phrases.Remove(key);
                }
            }
        }

        public Dictionary<string, string> GetPhrases(UnityEngine.Object obj)
        {
            if (!Pull()) return null;
            string key = obj is MonoBehaviour
                ? GenerateLocalizationKey(obj as MonoBehaviour)
                : GenerateLocalizationKey(obj as ScriptableObject);
            int count = key.Length;
            return _phrases.Where(k => k.Key.StartsWith(key)).ToDictionary(k => k.Key.Substring(count), k => k.Value);
        }

        public string GetPhrase(UnityEngine.Object obj)
        {
            if (!Pull()) return null;
            string key = obj is MonoBehaviour
                ? GenerateLocalizationKey(obj as MonoBehaviour)
                : GenerateLocalizationKey(obj as ScriptableObject);
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
            string key = obj is MonoBehaviour
                ? GenerateLocalizationKey(obj as MonoBehaviour)
                : GenerateLocalizationKey(obj as ScriptableObject);
            MoveKey(previousKey, key);
            return key;
        }

        [UnityEditor.MenuItem("Assets/Create/Localization Config")]
        public static void CreateLocalizationConfig()
        {
            Util.ScriptableObjectUtil.CreatUniqueInstance<LocalizationConfig>("Localization Config");
        }

        [UnityEditor.MenuItem("Ambition/Localization/Update Localization File")]
        public static void UpdateLocalizationFile()
        {
            string[] assets = AssetDatabase.FindAssets("t:" + typeof(LocalizationConfig).ToString());
            foreach (string asset in assets)
            {
                string path = AssetDatabase.GUIDToAssetPath(asset);
                LocalizationConfig config = AssetDatabase.LoadAssetAtPath<LocalizationConfig>(path);
                if (config?._phrases != null && config.DefaultLocalizationFile != null)
                {
                    string fileText = JsonConvert.SerializeObject(config._phrases, Formatting.Indented);
                    path = AssetDatabase.GetAssetPath(config.DefaultLocalizationFile);
                    File.WriteAllText(path, fileText);
                }
            }
        }
    }
}
#endif
