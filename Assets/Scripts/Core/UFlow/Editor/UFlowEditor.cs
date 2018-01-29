using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UFlow
{
	public class UFlowEditor : EditorWindow
	{
		private UFlowFile _machine;
			
		// Use this for initialization
		void OnGUI ()
		{
			
		}

		public void LoadMachine(string MachineID)
		{
			_machine = Resources.Load<UFlowFile>(MachineID);
		}

		[MenuItem ("Window/UFlow Editor")]
		public static UFlowEditor ShowWindow()
		{
			return EditorWindow.GetWindow<UFlowEditor>("UFlow Editor");
		}

		void OnInspectorGUI()
		{
			if (GUILayout.Button("New Machine"))
			{
				_machine = new UFlowFile();
			}
		}

		public void SaveMachine()
		{
			if(_machine != null)
			{
				 AssetDatabase.CreateAsset(_machine , "Assets/Resources/" + _machine.Machine.ID + ".asset");
				 AssetDatabase.SaveAssets();
				 AssetDatabase.Refresh();
			}
		}
 	}
}
