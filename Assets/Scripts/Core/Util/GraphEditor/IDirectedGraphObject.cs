using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Util
{
    public interface IDirectedGraphObject
    {
#if (UNITY_EDITOR)
        void Select(SerializedObject graphObject, int index, bool isNode);
        SerializedProperty GetNodes(SerializedObject graphObject);
        SerializedProperty GetLinks(SerializedObject graphObject);
        SerializedProperty GetLinkData(SerializedObject graphObject);
        SerializedProperty GetPositions(SerializedObject graphObject);
        void InitNodeData(SerializedProperty nodeData);
        void InitLinkData(SerializedProperty linkData);
        GUIContent GetGUIContent(int nodeIndex);
#endif
    }
}
