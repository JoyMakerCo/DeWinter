#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ambition
{
    public static class IncidentDebug
    {
        private static Dictionary<string, string> GetTranslations(Dictionary<string, string> allPhrases, string key, out IEnumerable<string> remove)
        {
            if (allPhrases == null || string.IsNullOrWhiteSpace(key))
            {
                remove = null;
                return null;
            }
            Dictionary<string, string> result = allPhrases.Where(k => k.Key.StartsWith(key)).ToDictionary(k => k.Key, k => k.Value);
            remove = result.Keys;
            return result;
        }

        [MenuItem("Ambition/Create Incident Report")]
        public static void CreateIncidentReport()
        {
            string IncidentID;
            string result = "";
            SerializedObject obj;
            SerializedProperty list;

            Dictionary<string, string> localizations = LocalizationConfig.GetPhrases();
            if (localizations == null)
            {
                result += "Could not load LocalizationConfig!\n\n";
            }

            string[] guids = AssetDatabase.FindAssets("t:IncidentConfig");
            IncidentConfig config;
            string key;
            string path;
            Dictionary<string, string> phrases = null;
            Dictionary<string, string> writeBack = new Dictionary<string, string>();
            string str;
            int len;
            int index;
            List<string> remove = new List<string>();
            IEnumerable<string> oldKeys;
            foreach (string guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                len = path.Length;
                config = AssetDatabase.LoadAssetAtPath<IncidentConfig>(path);
                obj = new SerializedObject(config);
                key = obj.FindProperty("_localizationKey")?.stringValue;
                IncidentID = config.Incident.LocalizationKey;

                result += path + ":\n";

                phrases = GetTranslations(localizations, key, out oldKeys);
                if (oldKeys != null) remove.AddRange(oldKeys);
                if (phrases == null || phrases.Count == 0)
                {
                    key = obj.FindProperty("Incident.LocalizationKey")?.stringValue;
                    phrases = GetTranslations(localizations, key, out oldKeys);
                    if (oldKeys != null) remove.AddRange(oldKeys);
                }
                if (phrases == null || phrases.Count == 0)
                {
                    result += " - No localizations found\n";

                    phrases = new Dictionary<string, string>();
                    list = obj.FindProperty("Incident.Nodes");
                    if (list != null)
                    {
                        for (int i = list.arraySize - 1; i >= 0; i--)
                        {
                            str = list.GetArrayElementAtIndex(i)?.FindPropertyRelative("Text")?.stringValue;
                            if (string.IsNullOrWhiteSpace(str)) result += " - Node at " + i.ToString() + " Undefined!\n";
                            if (!string.IsNullOrWhiteSpace(str)) phrases[path + ".node." + i] = str;
                            else phrases.Remove(path + ".node." + i);
                        }
                    }
                    else
                    {
                        result += " - No Moments Found!\n";
                    }

                    list = obj.FindProperty("Incident.LinkData");
                    if (list != null)
                    {
                        for (int i = list.arraySize - 1; i >= 0; i--)
                        {
                            str = list.GetArrayElementAtIndex(i)?.FindPropertyRelative("Text")?.stringValue;
                            if (str == null) result += " - Link at " + i.ToString() + " Undefined!\n";
                            if (!string.IsNullOrWhiteSpace(str)) phrases[path + ".link." + i] = str;
                            else phrases.Remove(path + ".link." + i);
                        }
                    }
                    else
                    {
                        result += " - No Transitions Found!\n";
                    }
                }
                obj.FindProperty("_localizationKey").stringValue = null;
                obj.FindProperty("Incident.LocalizationKey").stringValue = path;
                obj.FindProperty("_nodeText").arraySize = 0;
                obj.FindProperty("_linkText").arraySize = 0;
                if (phrases != null && phrases.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kvp in phrases)
                    {
                        list = null;
                        if (kvp.Key.Length > len)
                        {
                            if (kvp.Key.Substring(len).StartsWith(".node"))
                            {
                                list = obj.FindProperty("_nodeText");
                            }
                            else if (kvp.Key.Substring(len).StartsWith(".link"))
                            {
                                list = obj.FindProperty("_linkText");
                            }
                            if (list != null)
                            {
                                index = int.Parse(kvp.Key.Substring(len + 6));
                                if (index >= list.arraySize)
                                {
                                    list.arraySize = index + 1;
                                }
                                list.GetArrayElementAtIndex(index).stringValue = kvp.Value;
                            }
                        }
                    }
                    result += " - " + phrases.Count + " phrases written\n";
                    writeBack = writeBack.Concat(phrases).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                else
                {
                    result += " - No Localizations written!\n";
                }
                obj.ApplyModifiedProperties();
                result += "\n";
            }
            if (writeBack.Count > 0)
            {
                LocalizationConfig.Post(writeBack, remove.ToArray());
                Debug.Log("Wrote " + writeBack.Count + " phrases");
            }
            System.IO.File.WriteAllText("Assets/Incident Report.txt", result);
        }

        [MenuItem("Ambition/Realign Incident Dates")]
        public static void RealignIncidentDates()
        {
            string[] guids = AssetDatabase.FindAssets("t:IncidentConfig");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                IncidentConfig config = AssetDatabase.LoadAssetAtPath<IncidentConfig>(path);
                SerializedObject obj = new SerializedObject(config);
                obj.FindProperty("Incident._date").longValue = obj.FindProperty("_date").longValue;
                obj.ApplyModifiedProperties();
            }
        }
    }
}
#endif