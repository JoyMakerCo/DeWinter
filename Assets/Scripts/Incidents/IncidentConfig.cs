#pragma warning disable 0414
using UnityEngine;
using System;
using System.Collections.Generic;
using Util;
using UGraph;

#if (UNITY_EDITOR)
using System.Linq;
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
        IDirectedGraphDeleteLinkHandler
    {
        public const int NUM_CHAPTERS = 4;

        public IncidentVO Incident = new IncidentVO();

        [SerializeField]
        private bool[] _factions = new bool[Enum.GetValues(typeof(FactionType)).Length];
        [SerializeField]
        private bool[] _chapters = new bool[NUM_CHAPTERS];
        [SerializeField]
        private long _date;
        [SerializeField]
        private string _localizationKey = null;

        public IncidentVO GetIncident() => new IncidentVO(Incident)
        {
            Name = name,
            Factions = GetFactions(),
            Chapters = GetChapters(),
            AssetPath = this.name,
            Date = _date <= 0 ? default : new DateTime(_date),
            LocalizationKey = _localizationKey
        };

        private FactionType[] GetFactions()
        {
            if (_factions == null || _factions.Length == 0)
                return new FactionType[] { FactionType.Neutral };
            List<FactionType> result = new List<FactionType>();
            for (int i = _factions.Length - 1; i >= 0; i--)
                if (_factions[i])
                    result.Add((FactionType)(i));
            return result.ToArray();
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
        private const string NODE_KEY = "node.";
        private const string LINK_KEY = "link.";
        private const string LOCALIZATION_KEY_KEY = "_localizationKey";

        private LocalizationConfig _localization;
        private SerializedProperty _localizationProp;

        [SerializeField]
        private string[] _nodeText;
        private SerializedProperty _nodelist;

        [SerializeField]
        private string[] _linkText;
        private SerializedProperty _linklist;

        public override string  GraphProperty => "Incident";

        public override void InitializeEditor(SerializedObject serializedObject)
        {
            Incident = Incident ?? new IncidentVO();
            _localization = Resources.Load<LocalizationConfig>("Localization Config");
            if (_localization == null)
            {
                throw new Exception("Could not open Localization Config!");
            }
            _localizationKey = _localization.UpdateLocalizationKey(this, _localizationKey);
            _localizationProp = serializedObject.FindProperty(LOCALIZATION_KEY_KEY);
            _localizationProp.stringValue = _localizationKey;
            Dictionary<string, string> phrases = _localization.GetPhrases(this);
            _nodelist = serializedObject.FindProperty("_nodeText");
            KeyValuePair<string, string>[] pairs = phrases.Where(k => k.Key.StartsWith(NODE_KEY)).ToArray();
            int index;
            foreach (KeyValuePair<string, string> kvp in pairs)
            {
                index = int.Parse(kvp.Key.Substring(kvp.Key.LastIndexOf('.') + 1));
                SetListItem(_nodelist, index, kvp.Value);
            }
            _linklist = serializedObject.FindProperty("_linkText");
            pairs = phrases.Where(k => k.Key.StartsWith(LINK_KEY)).ToArray();
            foreach (KeyValuePair<string, string> kvp in pairs)
            {
                index = int.Parse(kvp.Key.Substring(kvp.Key.LastIndexOf('.') + 1));
                SetListItem(_linklist, index, kvp.Value);
            }
        }

        public override void CleanupEditor(SerializedObject serializedObject)
        {
            if (_localization != null && serializedObject != null)
            {
                Dictionary<string, string> phrases = new Dictionary<string, string>();
                string key = _localization.GenerateLocalizationKey(this);
                _localizationKey = _localizationProp?.stringValue ?? _localizationKey;
                if (_localizationKey != key)
                {
                    _localization.RemovePhrases(_localizationKey);
                }
                _localizationProp.stringValue = key;
                for (int i=(_nodelist?.arraySize ?? 0) - 1; i>=0; i--)
                {
                    phrases[NODE_KEY + i.ToString()] = _nodelist.GetArrayElementAtIndex(i).stringValue;
                }
                _nodelist.ClearArray();
                for (int i = (_linklist?.arraySize ?? 0) - 1; i >= 0; i--)
                {
                    phrases[LINK_KEY + i.ToString()] = _linklist.GetArrayElementAtIndex(i).stringValue;
                }
                _linklist.ClearArray();
                _localization.Post(this, this.name);
                _localization.Post(this, phrases, true);
                _localization = null;
                phrases = null;
            }
        }

        private readonly string[] MONTHS = new string[]{
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        };

        private void OnEnable()
        {
            //hideFlags = HideFlags.DontSave; TODO: This is causing build problems
            if (Incident == null) Incident = new IncidentVO();
            //new DirectedGraph<MomentVO, TransitionVO>();
        }

        public override void RenderNodeUI(SerializedProperty moment, int nodeIndex)
        {
            SerializedProperty list;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = (nodeIndex != 0);
            if (GUILayout.Button("Set as Start"))
            {
                string[] subs = moment.propertyPath.Split('[', ']');
                list = moment.serializedObject.FindProperty("Incident.Nodes");
                int index = int.Parse(subs[1]);
                Vector2Int link;
                list.MoveArrayElement(index, 0);
                list = moment.serializedObject.FindProperty("Incident.Positions");
                list.MoveArrayElement(index, 0);
                list = moment.serializedObject.FindProperty("Incident.Links");
                for (int i = list.arraySize - 1; i >= 0; i--)
                {
                    link = list.GetArrayElementAtIndex(i).vector2IntValue;
                    if (link.x == index) link.x = 0;
                    else if (link.x < index) link.x++;
                    if (link.y == index) link.y = 0;
                    else if (link.y < index) link.y++;
                    list.GetArrayElementAtIndex(i).vector2IntValue = link;
                }
            }
            GUI.enabled = true;

            GUI.SetNextControlName("FOCUS_ID");
            EditorStyles.textField.wordWrap = true;
            list = moment.serializedObject.FindProperty("_nodeText");
            string momentText = GUILayout.TextArea(list != null && nodeIndex < list.arraySize ? list.GetArrayElementAtIndex(nodeIndex).stringValue : null);
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
                SetListItem(list, nodeIndex, momentText);
            }
        }

        public override void RenderLinkUI(SerializedProperty transition, int fromNode, int toNode)
        {
            if (transition != null)
            {
                EditorGUI.BeginChangeCheck();
                GUI.SetNextControlName("FOCUS_ID");
                SerializedProperty list = transition.serializedObject.FindProperty("_linkText");
                int index = Incident.GetLinkIndex(fromNode, toNode);
                string linktext = GUILayout.TextArea(list != null && index < list.arraySize ? list.GetArrayElementAtIndex(index).stringValue : null);
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("Rewards"), true);
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("Requirements"), true);
                EditorGUILayout.LabelField("Links marked as XOR are mutually exclusive");
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("xor"), true);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Flags: Type, Phrase, Value");
                EditorGUILayout.LabelField("Specify Phrase to override default");
                EditorGUILayout.LabelField("Specify non-zero Value to display");
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("Flags"), true);
                if (EditorGUI.EndChangeCheck() && linktext != null)
                {
                    SetListItem(list, index, linktext);
                }
            }
        }

        public override void RenderConfigUI(SerializedObject serializedObject)
        {
            SerializedProperty incident = serializedObject.FindProperty("Incident");
            SerializedProperty dateProperty = serializedObject.FindProperty("_date");
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
            EditorGUILayout.PropertyField(incident.FindPropertyRelative("Tags"), true);
            EditorGUILayout.PropertyField(incident.FindPropertyRelative("Requirements"), true);
        }

        public bool Render(SerializedProperty node, int index, Rect rect, bool selected)
        {
            SerializedProperty list = node?.serializedObject?.FindProperty("_nodeText");
            if (list == null || !list.isArray || index >= list.arraySize)
            {
                return false;
            }
            GUI.color = (index == 0)
               ? (selected ? Color.blue : Color.cyan)
               : (selected ? Color.gray : Color.white);
            GUI.skin.button.alignment = TextAnchor.UpperCenter;
            GUI.skin.button.wordWrap = true;
            string str = node.serializedObject.FindProperty("_nodeText").GetArrayElementAtIndex(index).stringValue;
            return GUI.Button(rect, str, GUI.skin.button);
        }

        public void InitializeNode(SerializedProperty nodeData, int index)
        {
            nodeData.FindPropertyRelative("Text").stringValue = "New Moment";
            SetListItem(_nodelist, index, "New Moment");
        }

        public void InitializeLink(SerializedProperty link, int index, int fromNode, int toNode)
        {
            SetListItem(_linklist, index, "");
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
            SerializedProperty list = node.serializedObject.FindProperty("_nodeText");
            int size = list.arraySize;
            if (index < size)
            {
                list.DeleteArrayElementAtIndex(index);
                if (list.arraySize == size)
                    list.DeleteArrayElementAtIndex(index);
            }
        }

        public void DeleteLink(SerializedProperty link, int index)
        {
            SerializedProperty list = link.serializedObject.FindProperty("_linkText");
            int size = list.arraySize;
            if (index < size)
            {
                list.DeleteArrayElementAtIndex(index);
                if (list.arraySize == size)
                    list.DeleteArrayElementAtIndex(index);
            }
        }


        [OnOpenAssetAttribute()]
        public static bool Open(int instanceID, int line) => Open<IncidentConfig>(instanceID, line);
    }

    [CustomEditor(typeof(IncidentConfig))]
    public class IncidentConfigDrawer : Editor
    {
        public override void OnInspectorGUI() => ((IncidentConfig)target).OnRenderInspector(serializedObject);
#endif
    }
}