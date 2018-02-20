using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if (UNITY_EDITOR)
	using UnityEditor;
	using UnityEditorInternal;
#endif

namespace Ambition
{
	public class EventCollection : ScriptableObject
	{
		public GameObject EventWindowPrefab;
		public List<EventConfig> Events = new List<EventConfig>();

#if (UNITY_EDITOR)
		[SerializeField, HideInInspector]
		internal EventConfig _selectedConfig;

		[MenuItem("Assets/Create/Create Event Collection")]
		public static void CreateEventCollection()
		{
			Util.ScriptableObjectUtil.CreateScriptableObject<EventCollection>();
		}
	}

	[CustomEditor(typeof(EventCollection))]
	public class EventCollectionEditor : Editor
	{
		private ReorderableList _list;



	    public override void OnInspectorGUI()
	    {
	    	DrawDefaultInspector();
	        if(GUILayout.Button("New Event"))
	        {
				EventCollection collection = (EventCollection)target;
	        	EventConfig config = new EventConfig();
	        	config.Name = "New Event";
				collection.Events.Add(config);
				collection._selectedConfig = config;
				EventEditor.Show(collection);
	        }
	    }
#endif
	}
}
