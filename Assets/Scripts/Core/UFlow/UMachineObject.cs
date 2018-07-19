using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Util;

namespace UFlow
{
    public class UMachineObject : DirectedGraphObject<UStateNode, UGraphLink>
    {
        [HideInInspector]
        public int[] Exits = new int[0];
        [HideInInspector]
        public Dictionary<int,int[]> Aggregates = new Dictionary<int,int[]>();

        public UMachineGraph CreateMachineData()
        {
            UMachineGraph graph = new UMachineGraph(Graph);
            graph.Exits = Exits;
            graph.Aggregates = Aggregates;
            return graph;
        }

        #if (UNITY_EDITOR)
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        new public static bool OnOpenAsset(int instanceID, int line)
        {
            UMachineObject obj = Selection.activeObject as UMachineObject;
            if (obj == null) return false;
            if (obj.Graph == null) obj.Graph = new UMachineGraph();
            GraphEditorWindow.Show<UFlowEditor>(obj, "UFlow");
            return true;
        }

        [MenuItem("Assets/Create/Create UFlow Machine")]
        public static void CreateMachine()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<UMachineObject>();
        }

        #endif
    }
}
