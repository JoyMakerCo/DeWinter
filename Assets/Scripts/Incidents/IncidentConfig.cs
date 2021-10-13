#pragma warning disable 0414
using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using UGraph;

#if (UNITY_EDITOR)
//using System.Linq;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEditor.Callbacks;
using Newtonsoft.Json;
using Core;
#endif

namespace Ambition
{
    [Serializable]
    public class IncidentConfig : DirectedGraphConfig,
        IDirectedGraphNodeRenderer,
        IDirectedGraphNodeInitializer,
        IDirectedGraphLinkInitializer,
        IDirectedGraphDeleteNodeHandler,
        IDirectedGraphDeleteLinkHandler,
        AmbitionEditor.ILocalizedAsset
    {
        public const int NUM_CHAPTERS = 4;
        public const string NODE_KEY = ".node.";
        public const string LINK_KEY = ".link.";

        public IncidentVO Incident = new IncidentVO();

        public string Name = "";

        [SerializeField]
        private bool[] _factions = new bool[Enum.GetValues(typeof(FactionType)).Length];
        [SerializeField]
        private bool[] _chapters = new bool[NUM_CHAPTERS];
        [SerializeField]
        private long _date;

        public IncidentVO GetIncident() => new IncidentVO(Incident)
        {
            ID = name,
            Chapters = GetChapters(),
            Factions = GetFactions()
        };

        FactionType[] GetFactions()
        {
            if (_factions == null || _factions.Length == 0)
            {
                return new FactionType[] { FactionType.None };
            }
            List<FactionType> factions = new List<FactionType>();
            for (int i = _factions.Length - 1; i >= 0; i--)
                if (_factions[i])
                    factions.Add((FactionType)(i));
            return factions.ToArray();
        }

        private int[] GetChapters()
        {
            if (_chapters == null) return new int[0];
            List<int> chapters = new List<int>();
            for (int i = _chapters.Length - 1; i >= 0; i--)
                if (_chapters[i]) chapters.Add(i);
            return chapters.ToArray();
        }

#if (UNITY_EDITOR)
        private readonly string[] MONTHS = new string[]{
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        };

        [SerializeField]
        private string[] _nodeText;

        [SerializeField]
        private string[] _linkText;

        public override string  GraphProperty => "Incident";

        private SerializedProperty GetIncidentProperty(SerializedObject obj) => obj.FindProperty(GraphProperty);
        private SerializedProperty GetNodeTextProperty(SerializedObject obj) => obj.FindProperty("_nodeText");
        private SerializedProperty GetLinkTextProperty(SerializedObject obj) => obj.FindProperty("_linkText");

        public override void InitializeEditor(SerializedObject serializedObject)
        {
            Incident = Incident ?? new IncidentVO();
        }

        public override void CleanupEditor(SerializedObject serializedObject) { }

        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> phrases = new Dictionary<string, string>();
            SerializedObject obj = new SerializedObject(this);
            SerializedProperty list = GetNodeTextProperty(obj);
            string str;
            phrases[name + ".name"] = Name;
            phrases[name + ".description"] = GetIncidentProperty(obj).FindPropertyRelative("Description").stringValue;
            for (int i = list.arraySize - 1; i >= 0; i--)
            {
                str = list.GetArrayElementAtIndex(i)?.stringValue;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    phrases[name + NODE_KEY + i] = str;
                }
            }
            list = GetLinkTextProperty(obj);
            for (int i = list.arraySize - 1; i >= 0; i--)
            {
                str = list.GetArrayElementAtIndex(i)?.stringValue;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    phrases[name + LINK_KEY + i] = str;
                }
            }
            return phrases;
        }

        public override void RenderNodeUI(SerializedProperty moment, int nodeIndex)
        {
            if (moment == null) return;
            SerializedObject obj = moment.serializedObject;
            SerializedProperty incident = GetIncidentProperty(obj);
            SerializedProperty nodeText = GetNodeTextProperty(obj);

            EditorGUI.BeginChangeCheck();
            GUI.SetNextControlName("FOCUS_ID");
            GUI.enabled = (nodeIndex > 0);
            if (GUILayout.Button("Set as Start"))
            {
                SerializedProperty link;
                Vector2Int ends;
                SerializedProperty list = incident.FindPropertyRelative("Nodes");
                GetNodeTextProperty(obj)?.MoveArrayElement(nodeIndex, 0);
                list.MoveArrayElement(nodeIndex, 0);
                obj.FindProperty("_Positions")?.MoveArrayElement(nodeIndex, 0);

                list = GetLinkTextProperty(obj);
                for (int i=list.arraySize-1; i>=0; i--)
                {
                    link = list.GetArrayElementAtIndex(i);
                    if (link != null)
                    {
                        ends = link.vector2IntValue;
                        if (ends.x == nodeIndex) ends.x = 0;
                        else if (ends.x == 0) ends.x = nodeIndex;
                        if (ends.y == nodeIndex) ends.y = 0;
                        else if (ends.y == 0) ends.y = nodeIndex;
                        link.vector2IntValue = ends;
                    }
                }
                nodeIndex = 0;
                list = obj.FindProperty("_Nodes");
                if (list != null)
                {
                    list.arraySize = 1;
                    list.GetArrayElementAtIndex(0).intValue = 0;
                }
            }   
            GUI.enabled = true;

            GUIStyle wrappedStyle = new GUIStyle(EditorStyles.textArea);
            wrappedStyle.wordWrap = true;

            string momentText = (nodeText != null && nodeIndex < nodeText.arraySize )
                ? EditorGUILayout.TextArea(nodeText.GetArrayElementAtIndex(nodeIndex).stringValue, wrappedStyle)
                : null;
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Background"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Character1"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Character2"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Speaker"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Rewards"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Music"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("AmbientSFX"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("OneShotSFX"), true);
            if (EditorGUI.EndChangeCheck() && momentText != null)
            {
                SetListItem(nodeText, nodeIndex, momentText);
                nodeText.serializedObject.ApplyModifiedProperties();
            }
        }

        public override void RenderLinkUI(SerializedProperty transition, int fromNode, int toNode)
        {
            if (transition != null)
            {
                SerializedProperty linkText = GetLinkTextProperty(transition.serializedObject);
                EditorGUI.BeginChangeCheck();
                GUI.SetNextControlName("FOCUS_ID");
                int index = Incident.GetLinkIndex(fromNode, toNode);
                if (linkText != null && index >= linkText.arraySize)
                {
                    linkText.arraySize = index + 1;
                }
                string text = EditorGUILayout.TextArea(linkText?.GetArrayElementAtIndex(index).stringValue ?? string.Empty);
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("Rewards"), true);
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("Requirements"), true);
                EditorGUILayout.LabelField("Links marked as XOR are mutually exclusive");
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("xor"), true);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Flags: Type, Phrase, Value");
                EditorGUILayout.LabelField("Specify Phrase to override default");
                EditorGUILayout.LabelField("Specify non-zero Value to display");
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("Flags"), true);
                if (EditorGUI.EndChangeCheck())
                {
                    SetListItem(linkText, index, text);
                    linkText.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        public override void RenderConfigUI(SerializedObject serializedObject)
        {
            SerializedProperty incident = GetIncidentProperty(serializedObject);
            SerializedProperty dateProperty = incident.FindPropertyRelative("_date");
            SerializedProperty nodeprop = GetNodeTextProperty(serializedObject);
            SerializedProperty linkprop = GetLinkTextProperty(serializedObject);

            if (GUILayout.Button("Import From Localization File"))
            {
                string key = incident.FindPropertyRelative("LocalizationKey").stringValue + ".";
                Dictionary<string, string> phrases = null;//LocalizationManager.GetPhrases(key);
                if (phrases == null)
                {
                    throw new Exception(">> ERROR: Could not open localization config!");
                }

                nodeprop.arraySize = 0;
                linkprop.arraySize = 0;
                int len = key.Length + NODE_KEY.Length;
                int index;
                foreach(KeyValuePair<string, string> kvp in phrases)
                {
                    if (kvp.Key.StartsWith(key+NODE_KEY))
                    {
                        index = int.Parse(kvp.Key.Substring(len));
                        if (nodeprop.arraySize <= index)
                            nodeprop.arraySize = index + 1;
                        nodeprop.GetArrayElementAtIndex(index).stringValue = kvp.Value;
                    }
                    else if (kvp.Key.StartsWith(key+LINK_KEY))
                    {
                        index = int.Parse(kvp.Key.Substring(len));
                        if (linkprop.arraySize <= index)
                            linkprop.arraySize = index + 1;
                        linkprop.GetArrayElementAtIndex(index).stringValue = kvp.Value;
                    }
                }
            }
            serializedObject.FindProperty("Name").stringValue = EditorGUILayout.TextField("Name", serializedObject.FindProperty("Name").stringValue);
            EditorGUILayout.LabelField("Description:");
            incident.FindPropertyRelative("Description").stringValue = EditorGUILayout.TextArea(incident.FindPropertyRelative("Description").stringValue);
            bool fixedDate = GUILayout.Toggle(dateProperty.longValue >= 0, "Date");
            if (!fixedDate) dateProperty.longValue = -1;
            else
            {
                EditorGUILayout.BeginHorizontal();
                DateTime date = dateProperty.longValue > 0 ? DateTime.MinValue.AddTicks(dateProperty.longValue) : DateTime.MinValue;
                int day = Mathf.Clamp(EditorGUILayout.IntField(date.Day, GUILayout.MaxWidth(30f)), 1, DateTime.DaysInMonth(date.Year, date.Month));
                int month = EditorGUILayout.Popup(date.Month - 1, MONTHS) + 1;
                int year = Mathf.Clamp(EditorGUILayout.IntField(date.Year, GUILayout.MaxWidth(80f)), 1, 9999);
                dateProperty.longValue = new DateTime(year, month, day).Ticks;
                EditorGUILayout.EndHorizontal();
            }

            incident.FindPropertyRelative("Political").boolValue = GUILayout.Toggle(incident.FindPropertyRelative("Political").boolValue, "Political");
            incident.FindPropertyRelative("OneShot").boolValue = GUILayout.Toggle(incident.FindPropertyRelative("OneShot").boolValue, "One-Shot");
            GUILayout.Space(8f);
            SerializedProperty prop = serializedObject.FindProperty("_factions");
            for (int i = 0; i < prop.arraySize; i++)
            {
                prop.GetArrayElementAtIndex(i).boolValue = GUILayout.Toggle(prop.GetArrayElementAtIndex(i).boolValue, ((FactionType)i).ToString());
            }

            GUILayout.Space(8f);
            prop = serializedObject.FindProperty("_chapters");
            prop.arraySize = IncidentConfig.NUM_CHAPTERS;
            for (int i = 0; i < IncidentConfig.NUM_CHAPTERS; i++)
            {
                prop.GetArrayElementAtIndex(i).boolValue = GUILayout.Toggle(prop.GetArrayElementAtIndex(i).boolValue, "Chapter " + i.ToString());
            }
            EditorGUILayout.PropertyField(incident.FindPropertyRelative("Requirements"), true);
        }

        public bool Render(SerializedProperty node, int index, Rect rect, bool selected)
        {
            if (node == null) return false;
            SerializedProperty list = GetNodeTextProperty(node.serializedObject);
            if (list == null
                || !list.isArray
                || index >= node.serializedObject.FindProperty("_Positions")?.arraySize)
            {
                return false;
            }

            if (index > list.arraySize)
            {
                list.arraySize = index + 1;
            }

            if (index >= list.arraySize)
            {
                list.arraySize = index + 1;
            }

            GUI.color = (index == 0)
               ? (selected ? Color.blue : Color.cyan)
               : (selected ? Color.gray : Color.white);
            GUI.skin.button.alignment = TextAnchor.UpperCenter;
            GUI.skin.button.wordWrap = true;
            string str = list.GetArrayElementAtIndex(index).stringValue;
            return GUI.Button(rect, str, GUI.skin.button);
        }

        public void InitializeNode(SerializedProperty nodeData, int index)
        {
            SetListItem(GetNodeTextProperty(nodeData.serializedObject), index, "New Moment");
        }

        public void InitializeLink(SerializedProperty link, int index, int fromNode, int toNode)
        {
            SetListItem(GetLinkTextProperty(link.serializedObject), index, "");
            link.FindPropertyRelative("xor").boolValue = false;
            link.FindPropertyRelative("Rewards").arraySize = 0;
            link.FindPropertyRelative("Requirements").arraySize = 0;
            link.FindPropertyRelative("Flags").arraySize = 0;
        }

        [MenuItem("Assets/Create/Incident")]
        public static void CreateIncident()
        {
            ScriptableObjectUtil.CreateScriptableObject<IncidentConfig>("New Incident");
        }

        private void SelectFocusID()
        {
            EditorGUI.FocusTextInControl("FOCUS_ID");
            TextEditor txt = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            if (txt != null)
            {
                txt.SelectAll();
                Array.Find(Resources.FindObjectsOfTypeAll<EditorWindow>(), w => w.titleContent.text == "Inspector").Focus();
            }
        }

        private SerializedProperty SetListItem(SerializedObject serializedObject, string key, int index, string value)
        {
            return SetListItem(serializedObject?.FindProperty(key), index, value);
        }

        private SerializedProperty SetListItem(SerializedProperty list, int index, string value)
        {
            if (list == null || !list.isArray) return null;
            if (list.arraySize <= index) list.arraySize = index + 1;
            list.GetArrayElementAtIndex(index).stringValue = value;
            return list;
        }

        public void DeleteNode(SerializedProperty node, int index)
        {
            DeleteIndex(GetNodeTextProperty(node.serializedObject), index);
        }

        public void DeleteLink(SerializedProperty link, int index)
        {
            DeleteIndex(GetLinkTextProperty(link.serializedObject), index);
        }

        private bool DeleteIndex(SerializedProperty list, int index)
        {
            if (list == null || !list.isArray || index < 0 || index >= list.arraySize)
                return false;
            list.DeleteArrayElementAtIndex(index);
            if (index < list.arraySize && list.GetArrayElementAtIndex(index) == null)
                list.DeleteArrayElementAtIndex(index);
            return true;
        }

        [OnOpenAssetAttribute()]
        public static bool Open(int instanceID, int line) => Open<IncidentConfig>(instanceID, line);
    }

    [CustomEditor(typeof(IncidentConfig))]
    public class IncidentConfigDrawer : Editor
    {
        public override void OnInspectorGUI() => (target as IncidentConfig)?.OnRenderInspector(serializedObject);
#endif
    }
}