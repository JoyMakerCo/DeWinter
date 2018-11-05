#pragma warning disable 0414
using UnityEngine;
using System;
using Util;

#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace Ambition
{
    [Serializable]
    public class IncidentConfig : ScriptableObject, IDirectedGraphObjectConfig
    {
        public IncidentVO Incident = new IncidentVO();

#if (UNITY_EDITOR)
        [SerializeField]
        int _index = -1;

        [SerializeField]
        bool _isNode;

        SerializedObject _serializedObject;

        public SerializedObject GraphObject
        {
            get
            {
                if (_serializedObject == null)
                    _serializedObject = new SerializedObject(this);
                return _serializedObject;
            }
        }

        public SerializedProperty GraphProperty
        {
            get
            {
                return GraphObject.FindProperty("Incident");
            }
        }

        private void OnEnable()
        {
            //hideFlags = HideFlags.DontSave; TODO: This is causing build problems
            if (Incident == null) Incident = new IncidentVO();
            _serializedObject = new SerializedObject(this);
        }

        public void Select(int index, bool isNode)
        {
            GraphObject.FindProperty("_index").intValue = index;
            GraphObject.FindProperty("_isNode").boolValue = isNode;
        }

        public void InitNodeData(SerializedProperty nodeData)
        {
            nodeData.FindPropertyRelative("Text").stringValue = "New Moment";
        }

        public void InitLinkData(SerializedProperty linkData) { }


        [OnOpenAsset(1)]
        public static bool OpenIncidentConfig(int instanceID, int line)
        {
            IncidentConfig config = EditorUtility.InstanceIDToObject(instanceID) as IncidentConfig;
            return (config != null) && (null != GraphEditorWindow.Show(config));
        }

        [MenuItem("Assets/Create/Create Incident")]
        public static void CreateIncident()
        {
            ScriptableObjectUtil.CreateScriptableObject<IncidentConfig>("New Incident");
        }

        public static IncidentConfig CreateIncident(string name)
        {
            return ScriptableObjectUtil.CreateScriptableObject<IncidentConfig>(name);
        }

        public GUIContent GetGUIContent(int nodeIndex)
        {
            GUI.color = (nodeIndex == 0 ? Color.cyan : Color.white);
            GUI.skin.button.alignment = TextAnchor.UpperCenter;
            GUI.skin.button.wordWrap = true;
            string str = Incident.Nodes[nodeIndex].Text;
            return new GUIContent(str);
        }
#endif
    }

#if (UNITY_EDITOR)
    [CustomEditor(typeof(IncidentConfig))]
    public class IncidentConfigDrawer : Editor
    {
        private readonly string[] MONTHS = new string[]{
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        };

        private const string FOCUS_ID = "FOCUS_ID";

        private int _index = -1;

        private void OnEnable()
        {
            _index = -1;
        }

        private void OnDisable()
        {
            serializedObject.FindProperty("_index").intValue = -1;
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty property = null;
            serializedObject.Update();

            int index = serializedObject.FindProperty("_index").intValue;
            if (index >= 0)
            {
                bool selectText = _index != index;
                bool isNode = serializedObject.FindProperty("_isNode").boolValue;
                if (isNode)
                {
                    property = serializedObject.FindProperty("Incident.Nodes").GetArrayElementAtIndex(index);
                    DrawMoment(property);
                }
                else
                {
                    property = serializedObject.FindProperty("Incident.LinkData");
                    property.arraySize = serializedObject.FindProperty("Incident.Links").arraySize;
                    DrawTransition(property.GetArrayElementAtIndex(index));
                }
                _index = index;
                if (property != null && selectText)
                {
                    EditorGUI.FocusTextInControl(FOCUS_ID);
                    TextEditor txt = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                    if (txt != null)
                    {
                        txt.SelectAll();
                        Array.Find(Resources.FindObjectsOfTypeAll<EditorWindow>(), w => w.titleContent.text == "Inspector").Focus();
                    }
                }
                if ((Event.current.modifiers & (EventModifiers.Command | EventModifiers.Control)) > 0
                   && (Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Delete)
                    && GraphEditorWindow.DeleteSelected(serializedObject.targetObject as IncidentConfig, index, isNode))
                {
                    serializedObject.FindProperty("_isNode").boolValue = false;
                    serializedObject.FindProperty("_index").intValue = -1;
                }
            }
            else
            {
                SerializedProperty incident = serializedObject.FindProperty("Incident");
                SerializedProperty dateProperty = incident.FindPropertyRelative("_date");
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
            }
            serializedObject.ApplyModifiedProperties();
        }

        //This is where the display in the moment editor is handled
        private bool DrawMoment(SerializedProperty moment)
        {
            EditorGUI.BeginChangeCheck();
            GUI.enabled = (moment.propertyPath != "Incident.Nodes.Array.data[0]");
            if (GUILayout.Button("Set as Start"))
            {
                string[] subs = moment.propertyPath.Split('[', ']');
                SerializedProperty list = moment.serializedObject.FindProperty("Incident.Nodes");
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

            GUI.SetNextControlName(FOCUS_ID);
            EditorStyles.textField.wordWrap = true;
            SerializedProperty text = moment.FindPropertyRelative("Text");
            text.stringValue = GUILayout.TextArea(text.stringValue);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Background"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Character1"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Character2"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Speaker"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Rewards"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Music"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("AmbientSFX"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("OneShotSFX"), true);
            return EditorGUI.EndChangeCheck();
        }

        private bool DrawTransition(SerializedProperty transition)
        {
            EditorGUI.BeginChangeCheck();
            if (transition != null)
            {
                GUI.SetNextControlName(FOCUS_ID);
                SerializedProperty text = transition.FindPropertyRelative("Text");
                text.stringValue = GUILayout.TextArea(text.stringValue);
                EditorGUILayout.PropertyField(transition.FindPropertyRelative("Rewards"), true);
            }
            return EditorGUI.EndChangeCheck();
        }
    }
#endif
}