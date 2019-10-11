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
private string _localizationKey;


        public IncidentVO GetIncident() => new IncidentVO(Incident)
        {
            Name = name,
            Factions = GetFactions(),
            Chapters = GetChapters()
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

        [SerializeField]
        private string[] _nodeText;
        private SerializedProperty _nodeTextProp;

        [SerializeField]
        private string[] _linkText;
        private SerializedProperty _linkTextProp;

        public override string  GraphProperty => "Incident";

        public override void InitializeEditor(SerializedObject serializedObject)
        {
            Incident = Incident ?? new IncidentVO();
            _nodeTextProp = serializedObject.FindProperty("_nodeText");
            _linkTextProp = serializedObject.FindProperty("_linkText");

// TODO: Move all of the english text back into IncidentConfig, save all, then delete the localization portion of this method
LocalizationConfig localizationConfig = Resources.Load<LocalizationConfig>("Localization Config");
if (localizationConfig == null)
{
    throw new Exception(">> ERROR: Could not open localization config!");
}
int index;
int subst = NODE_KEY.Length;
SerializedProperty property;
Dictionary<string, string> phrases = localizationConfig.GetPhrases(_localizationKey ?? "");
if ((phrases?.Count ?? 0) == 0) phrases = localizationConfig.GetPhrases(this);
if (phrases != null)
{
    foreach (KeyValuePair<string, string> kvp in phrases)
    {
        property = kvp.Key.StartsWith(NODE_KEY)
                    ? _nodeTextProp
                    : kvp.Key.StartsWith(LINK_KEY)
                    ? _linkTextProp
                    : null;
        if (property != null)
        {
            index = int.Parse(kvp.Key.Substring(subst));
            if (property.arraySize <= index)
                property.arraySize = index + 1;
            property.GetArrayElementAtIndex(index).stringValue = kvp.Value;
        }
    }
}
localizationConfig.RemovePhrases(_localizationKey);
serializedObject.FindProperty("Incident.LocalizationKey").stringValue = localizationConfig.GenerateLocalizationKey(this);
serializedObject.ApplyModifiedProperties();
        }

        public override void CleanupEditor(SerializedObject serializedObject)
        {
            PublishLocalizations();
        }

        public void PublishLocalizations()
        {
/*            LocalizationConfig localizationConfig = Resources.Load<LocalizationConfig>("Localization Config");
            if (localizationConfig == null)
            {
                throw new Exception(">> ERROR: Could not open localization config!");
            }
            Dictionary<string, string> phrases = new Dictionary<string, string>();
            SerializedObject obj = new SerializedObject(this);
            localizationConfig.RemovePhrases(Incident.LocalizationKey);

            obj.FindProperty("Incident.LocalizationKey").stringValue = localizationConfig.GenerateLocalizationKey(this);
            obj.ApplyModifiedProperties();

            for (int i = _nodeText.Length - 1; i>=0; i--)
            {
                if (!string.IsNullOrEmpty(_nodeText[i]))
                {
                   phrases[NODE_KEY + i.ToString()] = _nodeText[i];
                }
            }
            for (int i = _linkText.Length - 1; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(_linkText[i]))
                {
                    phrases[LINK_KEY + i.ToString()] = _linkText[i];
                }
            }
            localizationConfig.Post(this, this.name);
            localizationConfig.Post(this, phrases, true);
            localizationConfig = null;
            phrases = null;
 */       }

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
            if (moment == null) return;
            EditorGUI.BeginChangeCheck();
            GUI.SetNextControlName("FOCUS_ID");
            GUI.enabled = (nodeIndex > 0);
            if (GUILayout.Button("Set as Start"))
            {
                SerializedObject config = moment.serializedObject;
                SerializedProperty incident = moment.serializedObject.FindProperty("Incident");
                Vector2Int link;
                SerializedProperty list = incident.FindPropertyRelative("Nodes");
                _nodeTextProp.MoveArrayElement(nodeIndex, 0);
                list.MoveArrayElement(nodeIndex, 0);
                list = config.FindProperty("_Positions");
                list.MoveArrayElement(nodeIndex, 0);
                list = incident.FindPropertyRelative("Links");
                for (int i = list.arraySize - 1; i >= 0; i--)
                {
                    link = list.GetArrayElementAtIndex(i).vector2IntValue;
                    if (link.x == nodeIndex) link.x = 0;
                    else if (link.x < nodeIndex) link.x++;
                    if (link.y == nodeIndex) link.y = 0;
                    else if (link.y < nodeIndex) link.y++;
                    list.GetArrayElementAtIndex(i).vector2IntValue = link;
                }
                nodeIndex = 0;
            }
            GUI.enabled = true;

            GUIStyle wrappedStyle = new GUIStyle(EditorStyles.textArea);
            wrappedStyle.wordWrap = true;

            string momentText = (_nodeTextProp != null && nodeIndex < _nodeTextProp.arraySize )
                ? EditorGUILayout.TextArea(_nodeTextProp.GetArrayElementAtIndex(nodeIndex).stringValue, wrappedStyle)
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
                SetListItem(_nodeTextProp, nodeIndex, momentText);
                _nodeTextProp.serializedObject.ApplyModifiedProperties();
            }
        }

        public override void RenderLinkUI(SerializedProperty transition, int fromNode, int toNode)
        {
            if (transition != null)
            {
                EditorGUI.BeginChangeCheck();
                GUI.SetNextControlName("FOCUS_ID");
                int index = Incident.GetLinkIndex(fromNode, toNode);
                string linktext = (_linkTextProp != null && index < _linkTextProp.arraySize)
                    ? EditorGUILayout.TextArea(_linkTextProp.GetArrayElementAtIndex(index).stringValue)
                    : null;
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
                    SetListItem(_linkTextProp, index, linktext);
                    _linkTextProp.serializedObject.ApplyModifiedProperties();
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
            SetListItem(_nodeTextProp, index, "New Moment");
        }

        public void InitializeLink(SerializedProperty link, int index, int fromNode, int toNode)
        {
            SetListItem(_linkTextProp, index, "");
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
            DeleteIndex(_nodeTextProp, index);
        }

        public void DeleteLink(SerializedProperty link, int index)
        {
            DeleteIndex(_linkTextProp, index);
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