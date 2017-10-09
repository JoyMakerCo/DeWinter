using System;
using UnityEngine;
using UnityEditor;

namespace UFlow
{
	public class UFlowFile : ScriptableObject
	{
		public UMachine Machine;
	}

	[CustomEditor(typeof(UFlowFile))]
	public class LevelScriptEditor : Editor 
	{
	    public override void OnInspectorGUI()
	    {
			UFlowFile file = target as UFlowFile;
	        
	        EditorGUILayout.TextField("Machine ID", file.Machine.ID);
			if(GUILayout.Button("Edit"))
	        {
				UFlowEditor.ShowWindow().LoadMachine(file.Machine.ID);
	        }
	    }
	}
}

