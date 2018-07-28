using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Util
{
    public interface IDirectedGraphObject
    {
#if (UNITY_EDITOR)
        void Select(int index, bool isNode);
        void UpdateGraph();
        void ApplyModifiedProperties();
        SerializedProperty GetNodes();
        SerializedProperty GetLinks();
        SerializedProperty GetLinkData();
        SerializedProperty GetPositions();
        void InitNodeData(SerializedProperty nodeData);
        void InitLinkData(SerializedProperty linkData);
        GUIContent GetGUIContent(int nodeIndex);
#endif
    }

    public class DirectedGraphObject : ScriptableObject, IDirectedGraphObject
    {
        public DirectedGraph Graph;

#if (UNITY_EDITOR)
        [HideInInspector]
        public int Index;

        [HideInInspector]
        public bool IsNode;

        [HideInInspector]
        public Rect[] Positions;

        private SerializedObject _serializedObject;

        void Awake()
        {
            Graph = new DirectedGraph();
            Positions = new Rect[0];
            _serializedObject = new SerializedObject(this);
        }

        public void Select(int index, bool isNode)
        {
            _serializedObject.FindProperty("Index").intValue = index;
            _serializedObject.FindProperty("IsNode").boolValue = isNode;
        }

        public SerializedProperty GetNodes()
        {
            return _serializedObject.FindProperty("Graph").FindPropertyRelative("Nodes");
        }

        public SerializedProperty GetLinks()
        {
            return _serializedObject.FindProperty("Graph").FindPropertyRelative("Links");
        }

        public SerializedProperty GetLinkData()
        {
            return _serializedObject.FindProperty("Graph").FindPropertyRelative("LinkData");
        }

        public SerializedProperty GetPositions()
        {
            return _serializedObject.FindProperty("Positions");
        }

        public void InitNodeData(SerializedProperty nodeData) {}
        public void InitLinkData(SerializedProperty linkData) {}

        public GUIContent GetGUIContent(int nodeIndex)
        {
            GUIContent result = new GUIContent("Node " + nodeIndex);
            return result;
        }

        public void UpdateGraph()
        {
            if (_serializedObject != null)
                _serializedObject.Update();
        }

        public void ApplyModifiedProperties()
        {
            if (_serializedObject != null)
                _serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}
