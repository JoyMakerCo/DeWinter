using System;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
using System.Linq;
using UnityEditor.Callbacks;
#endif

namespace UGraph
{
    public abstract class DirectedGraphConfig : ScriptableObject
    {
#if (UNITY_EDITOR)
        public abstract string GraphProperty { get; }
        [SerializeField]
        internal int[] _Nodes = new int[0];
        [SerializeField]
        internal int[] _Links = new int[0];
        [SerializeField]
        internal Vector2[] _Positions = new Vector2[0];

        public virtual void InitializeEditor(SerializedObject serializedObject) {}
        public virtual void CleanupEditor(SerializedObject serializedObject) {}
        public virtual void RenderNodeUI(SerializedProperty node, int nodeIndex)
        {
            EditorGUILayout.TextField("Node Index: " + nodeIndex.ToString());
        }

        public virtual void RenderLinkUI(SerializedProperty link, int fromNode, int toNode)
        {
            EditorGUILayout.TextField("Link From Node: " + fromNode.ToString());
            EditorGUILayout.TextField("Link To Node: " + toNode.ToString());
        }

        public virtual void RenderConfigUI(SerializedObject obj)
        {
            EditorGUILayout.TextField("Nodes: " + _Positions.Length.ToString());
        }

        public static bool Open<T>(int instanceID, int line) where T : DirectedGraphConfig
        {
            T config = EditorUtility.InstanceIDToObject(instanceID) as T;
            return (config != null) && (null != GraphEditorWindow.Show(config));
        }

        public void OnRenderInspector(SerializedObject serializedObject)
        {
            serializedObject.Update();
            if ((_Nodes?.Length ?? 0) > 0)
            {
                RenderNodeUI(serializedObject.FindProperty(GraphProperty+".Nodes").GetArrayElementAtIndex(_Nodes[0]), _Nodes[0]);
            }
            if ((_Links?.Length ?? 0) > 0)
            {
                Vector2Int ends = serializedObject.FindProperty(GraphProperty + ".Links").GetArrayElementAtIndex(_Links[0]).vector2IntValue;
                RenderLinkUI(serializedObject.FindProperty(GraphProperty + ".LinkData")?.GetArrayElementAtIndex(_Links[0]), ends.x, ends.y);
            }
            else if ((_Nodes?.Length ?? 0) == 0)
            {
                RenderConfigUI(serializedObject);
            }
            serializedObject.ApplyModifiedProperties();
        }

        public void OnCleanupInspector(SerializedObject obj)
        {
            if (obj == null) return;
            obj.FindProperty("_Nodes").arraySize = 0;
            obj.FindProperty("_Links").arraySize = 0;
        }
#endif
    }

    public interface IDirectedGraphNodeRenderer
    {
#if (UNITY_EDITOR)
        bool Render(SerializedProperty node, int index, Rect rect, bool selected);
#endif
    }

    public interface IDirectedGraphLinkRenderer
    {
#if (UNITY_EDITOR)
        bool Render(SerializedProperty link, int index, Vector2[] path, bool selected);
#endif
    }

    public interface IDirectedGraphNodeInitializer
    {
#if (UNITY_EDITOR)
        void InitializeNode(SerializedProperty node, int index);
#endif
    }

    public interface IDirectedGraphLinkInitializer
    {
#if (UNITY_EDITOR)
        void InitializeLink(SerializedProperty link, int index, int fromNode, int toNode);
#endif
    }

    public interface IDirectedGraphDeleteNodeHandler
    {
#if (UNITY_EDITOR)
        void DeleteNode(SerializedProperty node, int nodeIndex);
#endif
    }

    public interface IDirectedGraphDeleteLinkHandler
    {
#if (UNITY_EDITOR)
        void DeleteLink(SerializedProperty link, int linkIndex);
#endif
    }
}

