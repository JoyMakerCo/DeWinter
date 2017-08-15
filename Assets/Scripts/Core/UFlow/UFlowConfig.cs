using System;
using UnityEditor;
using UnityEngine;
using Util;

namespace UFlow
{
	[Serializable]
	public class UFlowStateConfig
	{
		public string State;
		public string[] Actions;
	}

	// Temporary class for defining sequential states
	// Will rework UI to represent a directed graph
	public class uFlowConfig : ScriptableObject
	{
		public UFlowStateConfig[] States;

		[MenuItem("Assets/Create/Create Guest Config")]
		public static void CreatePrefabConfig()
		{
			ScriptableObjectUtil.CreateScriptableObject<uFlowConfig>();
		}
	}
}
