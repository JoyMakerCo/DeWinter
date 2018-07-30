using System;
using Util;

#if (UNITY_EDITOR)
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace Ambition
{
    public class IncidentConfig : ScriptableObject, IDirectedGraphObject
    {
        public static readonly string[] MONTHS =
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        public int Day = 1;
        public int Month = 0;
        public int Year = 1795;
        public IncidentVO Incident;

        #if (UNITY_EDITOR)

        [SerializeField]
        private int _index;

        [SerializeField]
        private bool _isNode;

        [HideInInspector]
        public Vector2[] Positions;

        private SerializedObject _serializedObject;
        private SerializedObject _SerializedObject
        {
            get
            {
                if (_serializedObject == null)
                    _serializedObject = new SerializedObject(this);
                return _serializedObject;
            }
        }

        void Awake()
        {
            Incident = new IncidentVO();
            Positions = new Vector2[0];
        }

        public void Select(int index, bool isNode)
        {
            _SerializedObject.FindProperty("_index").intValue = index;
            _SerializedObject.FindProperty("_isNode").boolValue = isNode;
        }

        public SerializedProperty GetNodes()
        {
            return _SerializedObject.FindProperty("Incident.Nodes");
        }

        public SerializedProperty GetLinks()
        {
            return _SerializedObject.FindProperty("Incident.Links");
        }

        public SerializedProperty GetLinkData()
        {
            return _SerializedObject.FindProperty("Incident.LinkData");
        }

        public SerializedProperty GetPositions()
        {
            return _SerializedObject.FindProperty("Positions");
        }

        public GUIContent GetGUIContent(int nodeIndex)
        {
            GUI.color = (nodeIndex == 0 ? Color.cyan : Color.white);
            GUI.skin.button.alignment = TextAnchor.UpperCenter;
            string str = Incident.Nodes[nodeIndex].Text;                
            return new GUIContent(str);
        }

        public void InitNodeData(SerializedProperty nodeData)
        {
            nodeData.FindPropertyRelative("Text").stringValue = "New Moment";
        }

        public void InitLinkData(SerializedProperty linkData) {}

        public void UpdateGraph()
        {
            if (_SerializedObject != null)
                _SerializedObject.Update();
        }

        public void ApplyModifiedProperties()
        {
            if (_SerializedObject != null)
                _SerializedObject.ApplyModifiedProperties();
        }

        [OnOpenAssetAttribute(1)]
        public static bool OpenIncidentConfig(int instanceID, int line)
        {
            IncidentConfig config = EditorUtility.InstanceIDToObject(instanceID) as IncidentConfig;
            if (config == null) return false; // we did not handle the open
            GraphEditorWindow window = GraphEditorWindow.Show(config);

            return window != null;
        }
#endif
    }

#if (UNITY_EDITOR)
    [CustomEditor(typeof(IncidentConfig))]
    public class IncidentConfigDrawer : Editor
    {
        private const string FOCUS_ID = "FOCUS_ID";

        private int _index;

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
            bool repaint = false;
            serializedObject.Update();
            int index = serializedObject.FindProperty("_index").intValue;
            if (index >= 0)
            {
                SerializedProperty property;
                bool selectText = _index != index;
                bool isNode = serializedObject.FindProperty("_isNode").boolValue;
                if (isNode)
                {
                    property = serializedObject.FindProperty("Incident.Nodes").GetArrayElementAtIndex(index);
                    DrawMoment(property);
                }
                else
                {
                    property = serializedObject.FindProperty("Incident.LinkData").GetArrayElementAtIndex(index);
                    DrawTransition(property);
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
                //if ((Event.current.modifiers & EventModifiers.Command) > 0 && Event.current.keyCode == KeyCode.Backspace)
                //{
                //    DeleteSelected(index, isNode);
                //}
            }
            serializedObject.ApplyModifiedProperties();
        }

        private bool DrawMoment(SerializedProperty moment)
        {
            EditorGUI.BeginChangeCheck();
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
            GUI.SetNextControlName(FOCUS_ID);
            SerializedProperty text = transition.FindPropertyRelative("Text");
            text.stringValue = GUILayout.TextArea(text.stringValue);
            EditorGUILayout.PropertyField(transition.FindPropertyRelative("Rewards"), true);
            return EditorGUI.EndChangeCheck();
        }

        private void DeleteSelected(int index, bool isNode)
        {
            if (index < 0) return;
            if (isNode)
            {
                Vector2Int ln;
                SerializedProperty links = serializedObject.FindProperty("Incident.Links");
                DeleteIndex("Nodes", index);
                DeleteIndex("Positions", index);

                for (int i = links.arraySize - 1; i >= 0; i--)
                {
                    ln = links.GetArrayElementAtIndex(i).vector2IntValue;
                    if (ln.x == index || ln.y == index)
                    {
                        DeleteIndex("Links", i);
                        DeleteIndex("LinkData", i);
                    }
                    else
                    {
                        if (ln.x >= index) ln.x -= 1;
                        if (ln.y >= index) ln.y -= 1;
                        links.GetArrayElementAtIndex(i).vector2IntValue = ln;
                    }
                }
            }
            else
            {
                DeleteIndex("Links", index);
                DeleteIndex("LinkData", index);
            }
            serializedObject.FindProperty("_index").intValue = -1;
            serializedObject.ApplyModifiedProperties();
        }

        private bool DeleteIndex(string listID, int index)
        {
            SerializedProperty list = serializedObject.FindProperty("Incident" + listID);
            if (list == null || index >= list.arraySize) return false;
            list.DeleteArrayElementAtIndex(index);
            if (index < list.arraySize && list.GetArrayElementAtIndex(index) == null)
                list.DeleteArrayElementAtIndex(index);
            return true;
        }
    }
    #endif
}
