#pragma warning disable 0414

using System;
using UnityEngine;
using UnityEngine.Events;
using Util;
using UGraph;

#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.Serialization;
#endif


namespace UFlow
{
    [Serializable]
    public struct UConfigNode
    {
        public string ID;
        public UNodeType Type;
        public string[] Tags;
    }

    [Serializable]
    public class UConfigMachine : DirectedGraph<UConfigNode>
    {
        public UConfigMachine() : base(new DirectedGraph<UConfigNode>()) { }
    }

    [Serializable]
    public sealed class UFlowConfig : DirectedGraphConfig
    {
        [SerializeField]
        private DirectedGraph<UStateNode, UGraphLink> _Machine = new DirectedGraph<UStateNode, UGraphLink>();
        public UMachine GetMachine() => new UMachine(name, _Machine)
        {

        };

#if (UNITY_EDITOR)
        public override string GraphProperty => "_Machine";

        SerializedObject _obj = null;
        SerializedProperty _prop = null;

        [SerializeField]
        private int _index = -1;

        [SerializeField]
        private bool _isNode = false;
         
        public void Select(int index, bool isNode)
        {
            _index = index;
            _isNode = isNode;
        }

        public void InitNodeData(SerializedProperty nodeData)
        {
            nodeData.FindPropertyRelative("ID").stringValue = "New State";
            nodeData.FindPropertyRelative("Type").enumValueIndex = 0;
            nodeData.FindPropertyRelative("Tags").arraySize = 0;
        }

        public void InitLinkData(SerializedProperty linkData)
        {

        }

        public GUIContent GetGUIContent(int nodeIndex) => new GUIContent(this.name);

        //[OnOpenAsset(1)]
        //public static bool OpenUFlowConfig(int instanceID, int line)
        //{
        //    UFlowConfig config = EditorUtility.InstanceIDToObject(instanceID) as UFlowConfig;
        //    return (config != null) && (GraphEditorWindow.Show(config) != null);
        //}

        [MenuItem("Assets/Create/UFlow")]
        public static void CreateUFlow() => ScriptableObjectUtil.CreateScriptableObject<UFlowConfig>("New UFlow");
    }

    [CustomEditor(typeof(UFlowConfig))]
    public class IncidentConfigDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            if (serializedObject.FindProperty("_isNode").boolValue)
            {
                RenderNode(serializedObject.FindProperty("Graph.Nodes").GetArrayElementAtIndex(serializedObject.FindProperty("_index").intValue));
            }
        }

        private void RenderNode(SerializedProperty node)
        {
            EditorGUILayout.PropertyField(node.FindPropertyRelative("ID"), true);
            EditorGUILayout.PropertyField(node.FindPropertyRelative("Type"), true);
            EditorGUILayout.PropertyField(node.FindPropertyRelative("Tags"), true);
        }
#endif
    }
}
