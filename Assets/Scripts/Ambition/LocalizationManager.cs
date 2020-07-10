#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using ILocalizedAsset = AmbitionEditor.ILocalizedAsset;

namespace Ambition
{
    public class LocalizationManager : ScriptableObject
    {
        public string GoogleSheetId = "1YNtdQCWlgGjg0ruRC5XPlwQIXYaKmx4bs6f2bW2j9jA";
        public TextAsset DefaultLocalizationFile;
        public TextAsset OutputLocalizationFile;

        [SerializeField, HideInInspector]
        private string[] _files = new string[0];

        public void Localize()
        {
            ILocalizedAsset asset;
            Dictionary<string, string> phrases = DefaultLocalizationFile != null
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(DefaultLocalizationFile.text)
                : new Dictionary<string, string>();
            Dictionary<string, string> loc;
            foreach (string path in _files)
            {
                asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) as ILocalizedAsset;
                loc = asset?.Localize();
                if (loc != null)
                {
                    foreach(KeyValuePair<string, string> kvp in loc)
                    {
                        phrases[kvp.Key] = kvp.Value;
                    }
                }
            }
            if (OutputLocalizationFile != null)
            {
                phrases.OrderBy(k => k.Key);
                File.WriteAllText(AssetDatabase.GetAssetPath(OutputLocalizationFile), JsonConvert.SerializeObject(phrases, Formatting.Indented));
                EditorUtility.SetDirty(OutputLocalizationFile);
            }
        }

        public void AddFiles(string[] paths)
        {
            List<string> files = new List<string>(_files);
            ILocalizedAsset asset;
            int len = files.Count;
            foreach (string path in paths)
            {
                if (!files.Contains(path))
                {
                    asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) as ILocalizedAsset;
                    if (asset != null) files.Add(path);
                }
            }
            if (files.Count > len)
            {
                SerializedObject obj = new SerializedObject(this);
                SerializedProperty prop = obj.FindProperty("_files");
                if (prop?.isArray ?? false)
                {
                    prop.arraySize = files.Count;
                    for (int i = files.Count - 1; i >= 0; --i)
                        prop.GetArrayElementAtIndex(i).stringValue = files[i];
                    obj.ApplyModifiedProperties();
                }
            }
        }

        /*
        public static bool Post(Dictionary<string, string> phrases, IEnumerable<string> remove = null)
        {
            Dictionary<string, string> localizations = _instance?._GetLocalizationsFromFile();
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
                foreach(KeyValuePair<string, string> kvp in phrases)
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
            string path = AssetDatabase.GetAssetPath(_instance.DefaultLocalizationFile);
            File.WriteAllText(path, fileText);
            return true;
        }

        public static bool Remove(IEnumerable<string> keys) => Post(null, keys);
        public static bool Post(string key, string value) => Post(new Dictionary<string, string>() { { key, value } });
        public static bool Post(string key, Dictionary<string, string> phrases) => Post(phrases.ToDictionary(k => k.Key + ".", k => k.Value));
        public static Dictionary<string, string> GetPhrases() => _instance?._GetLocalizationsFromFile();
        public static Dictionary<string, string> GetPhrases(string key) => _instance?._GetLocalizationsFromFile()?.Where(s => s.Key?.StartsWith(key) ?? false).ToDictionary(s => s.Key, s => s.Value);

        public static void Post(ILocalizedAsset obj, string phrase) => Post(obj.LocalizationKey, phrase);
        public static Dictionary<string, string> GetPhrases(ILocalizedAsset obj) => GetPhrases(obj.LocalizationKey);
        */
        [UnityEditor.MenuItem("Ambition/Create/Localization Manager")]
        public static void CreateLocalizationConfig()
        {
            Util.ScriptableObjectUtil.GetUniqueInstance<LocalizationManager>();
        }

        [UnityEditor.MenuItem("Ambition/Sync Localizations")]
        public static void SyncLocalization()
        {
            // TODO: Pull localizations from Database; store in "File"
            //GoogleUtil.Get(GOOGLE_LOCALIZATION_SHEET_ID, "Sheet1!A2:E", OnReceivedJSON);
            //GoogleUtil.Post(GOOGLE_LOCALIZATION_SHEET_ID, )
            OnReceivedJSON(false, null);
        }

        private static void OnReceivedJSON(bool success, Dictionary<string, string> remote)
        {
            if (!success)
            {
                Debug.LogError("GOOGLE ERROR: Could not retrieve Localizations from Google Doc! Updating Local file only");
                return;
            }
        }

        private static void ResolveConflicts(Dictionary<string, string[]> conflicts, Action<Dictionary<string, string>> onConflictsResolved)
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

    public class ConflictResolutionEditor : Editor
    {
        private Action<Dictionary<string, string>> _onCloseDelegate = null;
        private Dictionary<string, string[]> _conflicts = null;


    }

    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Localize"))
            {
                ((LocalizationManager)target).Localize();
            }
        }
    }
}
#endif