using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using Util;

namespace UFlow
{
    [Serializable]
    public class UMachineObject : ScriptableObject, IDirectedGraphObject
    {
        public int[] Exits;
        public Dictionary<int, int[]> Aggregates;
        public UMachineGraph Machine;

        #if (UNITY_EDITOR)

        public Vector2[] Positions;

        [SerializeField]
        private int _index = -1;

        [SerializeField]
        private bool _isNode;

        private SerializedObject _graphObject;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontSave;
            if (Machine == null) Machine = new UMachineGraph();
            if (Exits == null) Exits = new int[0];
            if (Aggregates == null) Aggregates = new Dictionary<int, int[]>();
            _graphObject = new SerializedObject(this);
        }

        public void Select(int index, bool isNode)
        {
            GraphObject.FindProperty("_index").intValue = index;
            GraphObject.FindProperty("_isNode").boolValue = isNode;
        }

        public SerializedObject GraphObject
        {
            get {
                if (_graphObject == null) _graphObject = new SerializedObject(this);
                return _graphObject;
            }
        }

        public SerializedProperty GraphProperty
        {
            get { return GraphObject.FindProperty("Machine");  }
        }

        public GUIContent GetGUIContent(int nodeIndex)
        {
            GUI.color = (nodeIndex == 0 ? Color.cyan : Color.white);
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            string str = Machine.Nodes[nodeIndex].ID;
            return new GUIContent(str);
        }

        public void InitNodeData(SerializedProperty nodeData)
        {
            nodeData.FindPropertyRelative("ID").stringValue = "New State";
        }

        public void InitLinkData(SerializedProperty linkData) { }


        [OnOpenAsset(1)]
        public static bool OpenMachine(int instanceID, int line)
        {
            UMachineObject machine = EditorUtility.InstanceIDToObject(instanceID) as UMachineObject;
            return (machine != null) && (null != GraphEditorWindow.Show(machine));
        }

        [MenuItem("Assets/Create/Create UFlow Machine")]
        public static void CreateIncident()
        {
            ScriptableObjectUtil.CreateScriptableObject<UMachineObject>("New Machine");
        }
        #endif
    }

#if (UNITY_EDITOR)
    [CustomEditor(typeof(UMachineObject))]
    public class UMachineConfigDrawer : Editor
    {
        
    }
#endif
}
