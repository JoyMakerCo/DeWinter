using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Util
{
    public interface IDirectedGraphObjectConfig
    {
#if (UNITY_EDITOR)
        SerializedObject GraphObject { get;  }
        SerializedProperty GraphProperty { get; }
        void Select(int index, bool isNode);
        void InitNodeData(SerializedProperty nodeData);
        void InitLinkData(SerializedProperty linkData);
        GUIContent GetGUIContent(int nodeIndex);
#endif
    }
}
