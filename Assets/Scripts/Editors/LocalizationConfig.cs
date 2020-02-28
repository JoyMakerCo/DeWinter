#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Localizedtext = Ambition.AmbitionLocalizedText;

namespace AmbitionEditor
{
    public class LocalizationConfig : ScriptableObject
    {
        public TextAsset DefaultLocalizationFile;
        public DefaultAsset LocalizationDirectory;
        public Languages DefaultLanguage = Languages.en;
        public string GoogleSheetId = "1YNtdQCWlgGjg0ruRC5XPlwQIXYaKmx4bs6f2bW2j9jA";

        public bool Post(Dictionary<string, string> phrases, IEnumerable<string> remove = null)
        {
            Dictionary<string, string> localizations = _GetLocalizationsFromFile();
            if (localizations == null) return false;

            if (remove != null)
            {
                foreach (string removal in remove)
                {
                    localizations.Remove(removal);
                }
            }

            if (phrases != null)
            {
                foreach (KeyValuePair<string, string> kvp in phrases)
                {
                    if (kvp.Value != null)
                    {
                        localizations[kvp.Key] = kvp.Value;
                    }
                    else
                    {
                        localizations.Remove(kvp.Key);
                    }
                }
            }

            localizations = localizations.Where(k => k.Key != null && k.Value != null).OrderBy(k => k.Key).ToDictionary(k => k.Key, k => k.Value);
            string fileText = JsonConvert.SerializeObject(localizations, Formatting.Indented);
            string path = AssetDatabase.GetAssetPath(DefaultLocalizationFile);
            File.WriteAllText(path, fileText);
            return true;
        }

        public void CreateDefaultLocalizations()
        {
            string[] guids = AssetDatabase.FindAssets("l:Localization");
            Dictionary<string, string> result = _GetLocalizationsFromFile() ?? new Dictionary<string, string>();
            Dictionary<string, string> locs;
            ILocalizedAsset asset;
            string path;

            foreach (string guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path) as ILocalizedAsset;
                locs = asset?.Localize();
                if (locs != null)
                {
                    path = path.Substring(path.LastIndexOf('/') + 1) + "." + guid;
                    asset.SetLocalizationKey(path);
                    foreach (string key in locs.Keys)
                    {
                        if (!string.IsNullOrEmpty(key))
                        {
                            result[path + "." + key] = locs[key];
                        }
                        else
                        {
                            result[path] = locs[key];
                        }
                    }
                }
            }

            string fileText = JsonConvert.SerializeObject(result, Formatting.Indented);
            path = AssetDatabase.GetAssetPath(LocalizationDirectory);
            File.WriteAllText(path + "/" + DefaultLanguage.ToString() + ".json", fileText);
        }

        public bool Remove(IEnumerable<string> keys) => Post(null, keys);
        public bool Post(string key, string value) => Post(new Dictionary<string, string>() { { key, value } });
        public bool Post(string key, Dictionary<string, string> phrases) => Post(phrases.ToDictionary(k => k.Key + ".", k => k.Value));
        public Dictionary<string, string> GetPhrases() => _GetLocalizationsFromFile();
        public Dictionary<string, string> GetPhrases(string key) => _GetLocalizationsFromFile()?.Where(s => s.Key?.StartsWith(key) ?? false).ToDictionary(s => s.Key, s => s.Value);

        public void Post(ILocalizedAsset obj, string phrase) => Post(obj.GetLocalizationKey(), phrase);
        public Dictionary<string, string> GetPhrases(ILocalizedAsset obj) => GetPhrases(obj.GetLocalizationKey());

        [UnityEditor.MenuItem("Ambition/Create/Localization Config")]
        public void CreateLocalizationConfig()
        {
            Util.ScriptableObjectUtil.GetUniqueInstance<LocalizationConfig>("Localization Config");
        }

        [UnityEditor.MenuItem("Ambition/Sync Localizations")]
        public void SyncLocalization()
        {
            // TODO: Pull localizations from Database; store in "File"
            //GoogleUtil.Get(GOOGLE_LOCALIZATION_SHEET_ID, "Sheet1!A2:E", OnReceivedJSON);
            //GoogleUtil.Post(GOOGLE_LOCALIZATION_SHEET_ID, )
            OnReceivedJSON(false, null);
        }

        private void OnReceivedJSON(bool success, Dictionary<string, string> remote)
        {
            if (success)
            {
                // TODO: Conflict resolution
                Post(remote);
            }
            else
            {
                Debug.LogError("GOOGLE ERROR: Could not retrieve Localizations from Google Doc! Updating Local file only");
                return;
            }

        }

        private void ResolveConflicts(Dictionary<string, string[]> conflicts, Action<Dictionary<string, string>> onConflictsResolved)
        {
            if (conflicts.Count == 0) onConflictsResolved(new Dictionary<string, string>());
            else { } // OpenDialog(ConflictResolutionEditor(conflicts, onConflictsResolved));
        }

        private Dictionary<string, string> _GetLocalizationsFromFile()
        {
            return DefaultLocalizationFile != null
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(DefaultLocalizationFile.text)
                : null;
        }
    }

    [CustomEditor(typeof(LocalizationConfig))]
    public class LocalizationConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Create Default Localizations"))
            {
                ((LocalizationConfig)target).CreateDefaultLocalizations();
            }
        }
    }
}
#endif
