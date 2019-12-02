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
    public class LocalizationConfig : ScriptableObject
    {
        public TextAsset DefaultLocalizationFile;

        [SerializeField, HideInInspector] Localization[] _updates;
        private Dictionary<string, string> _localizations;

        public static string GOOGLE_LOCALIZATION_SHEET_ID = "1YNtdQCWlgGjg0ruRC5XPlwQIXYaKmx4bs6f2bW2j9jA";

        private static string UPDATE_KEY = "_updates";
        private static string KEY_KEY = "Key";
        private static string VALUE_KEY = "Value";

        private static LocalizationConfig _instance = null;

        private void OnEnable()
        {
            if (DefaultLocalizationFile == null) return;
            if (_instance == null)
            {
                _instance = this;
                _updates = _updates ?? new Localization[0];
            }
            _instance._Pull();
        }

        public static void Post(Dictionary<string, string> phrases, IEnumerable<string> remove = null)
        {
            if (_instance == null) return;

            SerializedObject obj = _GetSerializedObject();
            SerializedProperty locs = obj.FindProperty(UPDATE_KEY);
            SerializedProperty loc;
            string key, value=null;
            Dictionary<string, string> newPhrases = phrases?.ToDictionary(k => k.Key, k => k.Value);
            if (remove != null)
            {
                foreach(string removal in remove)
                {
                    if (!newPhrases.ContainsKey(removal))
                    {
                        newPhrases.Add(removal, null);
                    }
                }
            }

            if (_instance._localizations != null)
            {
                foreach(KeyValuePair<string, string> kvp in newPhrases)
                {
                    _instance._localizations[kvp.Key] = kvp.Value;
                }
            }

            Undo.RecordObject(_instance, "Update Localizations");

            for(int i=locs.arraySize-1; i>=0; i--)
            {
                loc = locs.GetArrayElementAtIndex(i);
                key = loc.FindPropertyRelative(KEY_KEY).stringValue;
                if (newPhrases?.TryGetValue(key, out value) ?? false)
                {
                    loc.FindPropertyRelative(VALUE_KEY).stringValue = value;
                    newPhrases.Remove(key);
                }
            }
            if (newPhrases != null)
            {
                _Add(newPhrases, locs);
            }
            obj.ApplyModifiedProperties();
        }

        public static void Remove(IEnumerable<string> keys) => Post(null, keys);
        public static void Post(string key, string value) => Post(new Dictionary<string, string>() { { key, value } });
        public static void Post(string key, Dictionary<string, string> phrases) => Post(phrases.ToDictionary(k => k.Key + ".", k => k.Value));
        public static Dictionary<string, string> GetPhrases() => _instance?._Pull();
        public static Dictionary<string, string> GetPhrases(string key) => _instance?._Pull()?.Where(s => s.Key?.StartsWith(key) ?? false).ToDictionary(s => s.Key, s => s.Value);

        public static void Post(ILocalizedAsset obj, string phrase) => Post(obj.LocalizationKey, phrase);
        public static Dictionary<string, string> GetPhrases(ILocalizedAsset obj) => GetPhrases(obj.LocalizationKey);
        public static string UpdateLocalizedAsset(ILocalizedAsset obj, out Dictionary<string, string> localizations, out List<string> removals)
        {
            localizations = obj?.Localize() ?? new Dictionary<string, string>();
            removals = new List<string>();
            if (_instance == null || obj == null) return null;

            string oldKey = obj.LocalizationKey;
            string locKey = AssetDatabase.GetAssetPath(obj as UnityEngine.Object);
            Dictionary<string, string> dictionary = _instance._Pull();
            string[] tmpList = localizations.Keys.ToArray();

            // Move the old keys to the new key, if they're different
            if (oldKey != locKey)
            {
                foreach (string k in dictionary.Keys)
                {
                    if (k.StartsWith(oldKey))
                    {
                        localizations[k.Replace(oldKey, locKey)] = dictionary[k];
                        removals.Add(k);
                    }
                }
            }

            // Move over the list of localizations in the object
            // This will stomp old values
            foreach (string k in tmpList)
            {
                localizations[k != "" ? (locKey + "." + k) : locKey] = localizations[k];
                localizations.Remove(k);
            }
            return locKey;
        }

        public static bool WriteToFile()
        {
            Dictionary<string, string> locs = _instance?._Pull();
            if (locs == null) return false;
            locs = locs.OrderBy(k => k.Key).ToDictionary(k=>k.Key, k=>k.Value);
            string fileText = JsonConvert.SerializeObject(locs, Formatting.Indented);
            string path = AssetDatabase.GetAssetPath(_instance.DefaultLocalizationFile);
            File.WriteAllText(path, fileText);

            SerializedObject obj = _GetSerializedObject();
            obj?.FindProperty(UPDATE_KEY)?.ClearArray();
            obj.ApplyModifiedProperties();

            return true;
        }

        private static SerializedObject _GetSerializedObject() => _instance != null ? new SerializedObject(_instance) : null;

        [UnityEditor.MenuItem("Ambition/Create/Localization Config")]
        public static void CreateLocalizationConfig()
        {
            _instance = Util.ScriptableObjectUtil.GetUniqueInstance<LocalizationConfig>("Localization Config");
        }

        [UnityEditor.MenuItem("Ambition/Sync Localizations")]
        public static void SyncLocalization()
        {
            if (_instance == null)
            {
                _instance = Util.ScriptableObjectUtil.GetUniqueInstance<LocalizationConfig>("Localization Config");
                if (_instance != null) _instance.OnEnable();
                else return;
            }

            // TODO: Pull localizations from Database; store in "File"
            //GoogleUtil.Get(GOOGLE_LOCALIZATION_SHEET_ID, "Sheet1!A2:E", OnReceivedJSON);
            //GoogleUtil.Post(GOOGLE_LOCALIZATION_SHEET_ID, )
            OnReceivedJSON(false, null);
        }

        private static void OnReceivedJSON(bool success, Dictionary<string, string> remote)
        {
            Dictionary<string, string> phrases = JsonConvert.DeserializeObject<Dictionary<string, string>>(_instance.DefaultLocalizationFile.text);
            List<string> updates;
            Dictionary<string, string[]> conflicts = new Dictionary<string, string[]>();

            List<string> remove = new List<string>();
            SerializedObject obj = _GetSerializedObject();
            SerializedProperty locs = obj.FindProperty(UPDATE_KEY);
            SerializedProperty loc;
            string key, value;

            if (success)
            {
                updates = new List<string>(phrases.Keys.Where(k => !remote.ContainsKey(k)));
                updates.ForEach(k => phrases.Remove(k)); // Remove all keys that exist on the local file but not on remote (initialized above)

                 // Update the local values with the remote values.
                foreach (KeyValuePair<string, string> kvp in remote)
                {
                    if (!phrases.ContainsKey(kvp.Key) || phrases[kvp.Key] != kvp.Value)
                    {
                        updates.Add(kvp.Key);
                        phrases[kvp.Key] = kvp.Value;
                    }
                }
            }
            else
            {
                Debug.LogError("GOOGLE ERROR: Could not retrieve Localizations from Google Doc! Updating Local file only");
                updates = new List<string>();
            }

            // Go through the locally modified serialized data
            for (int i = locs.arraySize - 1; i >= 0; i--)
            {
                loc = locs.GetArrayElementAtIndex(i);
                key = loc.FindPropertyRelative(KEY_KEY).stringValue;
                value = loc.FindPropertyRelative(VALUE_KEY)?.stringValue;

                // A value in the serialized data hasn't been marked as updated, and hence not a conflict
                if (!updates.Contains(key))
                {
                    if (value == null)
                    {
                        phrases.Remove(key); // Delete the key for null values
                    }
                    else
                    {
                        phrases[key] = value; // Update/Add the value
                    }
                }
                // Where the updated phrases and local modified values don't line up, there's a conflict
                else if (!phrases.ContainsKey(key) || phrases[key] != value)
                {
                    phrases.Remove(key);
                    conflicts.Add(key, new string[] { value, phrases[key] });
                }
            }
            ResolveConflicts(conflicts, (data) => {
                // Push the changes to the local file
                phrases = phrases.Concat(data).GroupBy(p => p.Key).ToDictionary(g => g.Key, g => g.Last().Value);
                string fileText = JsonConvert.SerializeObject(phrases, Formatting.Indented);
                string path = AssetDatabase.GetAssetPath(_instance.DefaultLocalizationFile);
                File.WriteAllText(path, fileText);

                // Update the serialized data
                locs.ClearArray();
                _Add(phrases, locs);
                obj.ApplyModifiedProperties();

                // Push the changes to the Google Doc
                if (success)
                {
                    GoogleUtil.Post(GOOGLE_LOCALIZATION_SHEET_ID, fileText, (postSuccess) =>
                    {
                        if (!postSuccess)
                        {
                            Debug.LogError("GOOGLE ERROR: Could not update remote Localizations!");
                        }
                    });
                }
            });
        }

        private static void ResolveConflicts(Dictionary<string, string[]> conflicts, Action<Dictionary<string, string>> onConflictsResolved)
        {
            if (conflicts.Count == 0) onConflictsResolved(new Dictionary<string, string>());
            else { } // OpenDialog(ConflictResolutionEditor(conflicts, onConflictsResolved));
        }

        private Dictionary<string, string> _Pull()
        {
            if (_localizations == null)
            {
                _localizations = JsonConvert.DeserializeObject<Dictionary<string, string>>(DefaultLocalizationFile.text);
                Array.ForEach(_updates, u => _localizations[u.Key] = u.Value);
            }
            return _localizations;
        }

        // This assumes that the updates object is non-null and index is valid.
        private static SerializedProperty _GetUpdate(SerializedProperty updates, int index, out string key, out string value)
        {
            SerializedProperty result = updates?.GetArrayElementAtIndex(index);
            key = result?.FindPropertyRelative(KEY_KEY).stringValue;
            value = result?.FindPropertyRelative(VALUE_KEY).stringValue;
            return result;
        }

        private static SerializedProperty _SetUpdate(SerializedProperty updates, int index, string key, string value)
        {
            return _SetUpdate(updates?.GetArrayElementAtIndex(index), key, value);
        }

        private static SerializedProperty _SetUpdate(SerializedProperty update, string key, string value)
        {
            if (update == null) return null;
            update.FindPropertyRelative(KEY_KEY).stringValue = key;
            update.FindPropertyRelative(VALUE_KEY).stringValue = value;
            return update;
        }

        private static void _Add(string key, string value, SerializedProperty locs)
        {
            locs.arraySize++;
            locs.GetArrayElementAtIndex(locs.arraySize - 1).FindPropertyRelative(KEY_KEY).stringValue = key;
            locs.GetArrayElementAtIndex(locs.arraySize - 1).FindPropertyRelative(VALUE_KEY).stringValue = value;
        }

        private static void _Add(Dictionary<string, string> phrases, SerializedProperty locs)
        {
            foreach (KeyValuePair<string, string> loc in phrases)
            {
                _Add(loc.Key, loc.Value, locs);
            }
        }

        [Serializable]
        public struct Localization // Since 2d arrays can't be serialized, need this "gem"
        {
            public string Key;
            public string Value;
        }
    }

    public class ConflictResolutionEditor : Editor
    {
        private Action<Dictionary<string, string>> _onCloseDelegate = null;
        private Dictionary<string, string[]> _conflicts = null;


    }
}
#endif
