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
        private int _index=-1;

        [SerializeField]
        private bool _isNode=false;

        [HideInInspector]
        public Vector2[] Positions;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontSave;
            if (Incident == null) Incident = new IncidentVO();
            if (Positions == null) Positions = new Vector2[0];
        }

        public void Select(SerializedObject graphObject, int index, bool isNode)
        {
            graphObject.FindProperty("_index").intValue = index;
            graphObject.FindProperty("_isNode").boolValue = isNode;
        }

        public SerializedProperty GetNodes(SerializedObject graphObject)
        {
            return graphObject.FindProperty("Incident.Nodes");
        }

        public SerializedProperty GetLinks(SerializedObject graphObject)
        {
            return graphObject.FindProperty("Incident.Links");
        }

        public SerializedProperty GetLinkData(SerializedObject graphObject)
        {
            return graphObject.FindProperty("Incident.LinkData");
        }

        public SerializedProperty GetPositions(SerializedObject graphObject)
        {
            return graphObject.FindProperty("Positions");
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
                if ((Event.current.modifiers & EventModifiers.Command) > 0 && Event.current.keyCode == KeyCode.Backspace)
                {
                    DeleteSelected(index, isNode);
                }
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
