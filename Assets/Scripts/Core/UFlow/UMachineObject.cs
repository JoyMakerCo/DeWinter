using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Util;

namespace UFlow
{
    public class UMachineObject : DirectedGraphObject
    {
        [HideInInspector]
        public int[] Exits = new int[0];
        [HideInInspector]
        public Dictionary<int,int[]> Aggregates = new Dictionary<int,int[]>();

        public UMachineGraph CreateMachineData()
        {
return null;
            //UMachineGraph graph = new UMachineGraph(Graph);
            //graph.Exits = Exits;
            //graph.Aggregates = Aggregates;
            //return graph;
        }

        #if (UNITY_EDITOR)
        [MenuItem("Assets/Create/Create UFlow Machine")]
        public static void CreateMachine()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<UMachineObject>();
        }

        #endif
    }
}
