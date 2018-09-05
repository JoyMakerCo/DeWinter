using System;
using Util;

#if (UNITY_EDITOR)
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace Ambition
{
    [Serializable]
    public class IncidentConfig : ScriptableObject, IDirectedGraphObject
    {
        public IncidentVO Incident;

#if (UNITY_EDITOR)

        [SerializeField]
        int _index=-1;

        [SerializeField]
        bool _isNode;

        SerializedObject _serializedObject;

        public SerializedObject GraphObject
        {
            get {
                if (_serializedObject == null)
                    _serializedObject = new SerializedObject(this);
                return _serializedObject;
            }
        }

        public Vector2[] Positions;

        public SerializedProperty GraphProperty
        {
            get {
                return GraphObject.FindProperty("Incident");
            }
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontSave;
            if (Incident == null) Incident = new IncidentVO();
            _serializedObject = new SerializedObject(this);
        }

        public void Select(int index, bool isNode)
        {
            GraphObject.FindProperty("_index").intValue = index;
            GraphObject.FindProperty("_isNode").boolValue = isNode;
        }

        public GUIContent GetGUIContent(int nodeIndex)
        {
            GUI.color = (nodeIndex == 0 ? Color.cyan : Color.white);
            GUI.skin.button.alignment = TextAnchor.UpperCenter;
            GUI.skin.button.wordWrap = true;
            string str = Incident.Nodes[nodeIndex].Text;                
            return new GUIContent(str);
        }

        public void InitNodeData(SerializedProperty nodeData)
        {
            nodeData.FindPropertyRelative("Text").stringValue = "New Moment";
        }

        public void InitLinkData(SerializedProperty linkData) {}


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

        public static IncidentConfig CreateIncident(IncidentVO incident)
        {
            IncidentConfig config = ScriptableObjectUtil.CreateScriptableObject<IncidentConfig>(incident.Name);
            SerializedObject obj = new SerializedObject(config);
            obj.Update();
            SerializedProperty configIncident = obj.FindProperty("Incident");
            SerializedProperty prop = obj.FindProperty("Positions");
            Vector2Int link = new Vector2Int();
            prop.arraySize = incident.Positions.Length;
            for (int i = incident.Positions.Length - 1; i >= 0; i--)
            {
                prop.GetArrayElementAtIndex(i).vector2Value = incident.Positions[i];
            }

            configIncident.FindPropertyRelative("Name").stringValue = incident.Name;

            prop = configIncident.FindPropertyRelative("Links");
            prop.arraySize = incident.Transitions.Length;
            for (int i = prop.arraySize - 1; i >= 0; i--)
            {
                link.x = incident.Transitions[i].Index;
                link.y = incident.Transitions[i].Target;
                prop.GetArrayElementAtIndex(i).vector2IntValue = link;
            }

            SerializedProperty element;
            prop = configIncident.FindPropertyRelative("Nodes");
            prop.arraySize = incident.Moments.Length;
            for (int i = incident.Moments.Length - 1; i >= 0; i--)
            {
                element = prop.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("Text").stringValue = incident.Moments[i].Text;
                if (incident.Moments[i].Rewards != null)
                {
                    element.FindPropertyRelative("Rewards").arraySize = incident.Moments[i].Rewards.Length;
                    for (int r = incident.Moments[i].Rewards.Length - 1; r >= 0; r--)
                    {
                        element.FindPropertyRelative("Rewards").GetArrayElementAtIndex(r).FindPropertyRelative("Type").intValue = (int)(incident.Moments[i].Rewards[r].Type);
                        element.FindPropertyRelative("Rewards").GetArrayElementAtIndex(r).FindPropertyRelative("ID").stringValue = incident.Moments[i].Rewards[r].ID;
                        element.FindPropertyRelative("Rewards").GetArrayElementAtIndex(r).FindPropertyRelative("Amount").intValue = incident.Moments[i].Rewards[r].Amount;
                    }
                }

                element.FindPropertyRelative("Character1.AvatarID").stringValue = incident.Moments[i].Character1.AvatarID;
                element.FindPropertyRelative("Character1.Name").stringValue = incident.Moments[i].Character1.Name;
                element.FindPropertyRelative("Character1.Pose").stringValue = incident.Moments[i].Character1.Pose;
                element.FindPropertyRelative("Character2.AvatarID").stringValue = incident.Moments[i].Character2.AvatarID;
                element.FindPropertyRelative("Character2.Name").stringValue = incident.Moments[i].Character2.Name;
                element.FindPropertyRelative("Character2.Pose").stringValue = incident.Moments[i].Character2.Pose;

                element.FindPropertyRelative("Speaker").intValue = (int)(incident.Moments[i].Speaker);

                if (incident.Moments[i].AudioClips != null)
                {
                    element.FindPropertyRelative("AudioClips").arraySize = incident.Moments[i].AudioClips.Length;
                    for (int a = incident.Moments[i].AudioClips.Length - 1; a >= 0; a--)
                    {
                        element.FindPropertyRelative("AudioClips").GetArrayElementAtIndex(a).objectReferenceInstanceIDValue = incident.Moments[i].AudioClips[a].GetInstanceID();
                    }
                }

                if (incident.Moments[i].Music != null)
                    element.FindPropertyRelative("Music").objectReferenceInstanceIDValue = incident.Moments[i].Music.GetInstanceID();
                element.FindPropertyRelative("Background").objectReferenceInstanceIDValue = incident.Moments[i].Background.GetInstanceID();
            }

            prop = configIncident.FindPropertyRelative("LinkData");
            prop.arraySize = incident.LinkData.Length;
            for (int i = incident.LinkData.Length - 1; i >= 0; i--)
            {
                element = prop.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("Text").stringValue = incident.Transitions[i].Text;
                if (incident.Transitions[i].Rewards != null)
                {
                    for (int r = incident.Transitions[i].Rewards.Length - 1; r >= 0; r--)
                    {
                        element.FindPropertyRelative("Rewards").GetArrayElementAtIndex(r).FindPropertyRelative("Type").intValue = (int)(incident.Transitions[i].Rewards[r].Type);
                        element.FindPropertyRelative("Rewards").GetArrayElementAtIndex(r).FindPropertyRelative("ID").stringValue = incident.Transitions[i].Rewards[r].ID;
                        element.FindPropertyRelative("Rewards").GetArrayElementAtIndex(r).FindPropertyRelative("Amount").intValue = incident.Transitions[i].Rewards[r].Amount;
                    }
                }
            }

            obj.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            return config;
        }
#endif
    }

#if (UNITY_EDITOR)
    [CustomEditor(typeof(IncidentConfig))]
    public class IncidentConfigDrawer : Editor
    {
        private const string FOCUS_ID = "FOCUS_ID";
        private static readonly string[] MONTHS = new string[]{
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        };

        private int _index=-1;

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
            serializedObject.Update();
            int index = serializedObject.FindProperty("_index").intValue;
            if (index >= 0)
            {
                SerializedProperty property = null;
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
<<<<<<< HEAD
                    && Event.current.keyCode == KeyCode.Backspace
                    && GraphEditorWindow.DeleteSelected(((IDirectedGraphObject)serializedObject.targetObject), index, isNode))
=======
                    && (Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Delete)
                    && GraphEditorWindow.DeleteSelected(serializedObject, index, isNode))
>>>>>>> 9f7f794e52eac68e41e333d01759c8bbe33fa384
                {
                    serializedObject.FindProperty("_isNode").boolValue = false;
                    serializedObject.FindProperty("_index").intValue = -1;
                }
            }
            else
            {
                SerializedProperty incident = serializedObject.FindProperty("Incident");
                EditorGUILayout.BeginHorizontal();
                bool fixedDate = EditorGUILayout.Toggle("Date", incident.FindPropertyRelative("_date").longValue >= 0);
                if (fixedDate)
                {
                    long ticks = incident.FindPropertyRelative("_date").longValue;
                    DateTime date = ticks > 0 ? DateTime.MinValue.AddTicks(ticks) : DateTime.MinValue;
                    int day = Mathf.Clamp(EditorGUILayout.IntField(date.Day, GUILayout.MaxWidth(30f)), 1, DateTime.DaysInMonth(date.Year, date.Month));
                    int month = EditorGUILayout.Popup(date.Month - 1, MONTHS) + 1;
                    int year = Mathf.Clamp(EditorGUILayout.IntField(date.Year, GUILayout.MaxWidth(80f)), 1, 9999);
                    incident.FindPropertyRelative("_date").longValue = new DateTime(year, month, day).Ticks;
                }
                else
                {
                    incident.FindPropertyRelative("_date").longValue = -1;
                }
                EditorGUILayout.EndHorizontal();
                incident.FindPropertyRelative("OneShot").boolValue = EditorGUILayout.Toggle("One-Shot", incident.FindPropertyRelative("OneShot").boolValue);
            }
            serializedObject.ApplyModifiedProperties();
        }

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
                list = moment.serializedObject.FindProperty("Positions");
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
            text.stringValue = EditorGUILayout.TextArea(text.stringValue);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Background"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Character1"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Character2"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Speaker"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Rewards"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("Music"), true);
            EditorGUILayout.PropertyField(moment.FindPropertyRelative("AudioClips"), true);
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
