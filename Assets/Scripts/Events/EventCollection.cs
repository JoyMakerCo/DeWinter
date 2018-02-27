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
		public EventConfig[] Events;

#if (UNITY_EDITOR)
		public const string SELECTED_EVENT = "_selectedEvent";
		public const string SELECTED_COMPONENT = "_selectedComponent";
		public const string IS_MOMENT = "_isMoment";

		[SerializeField]
		internal int _selectedEvent = -1;

		[SerializeField]
		internal int _selectedComponent = -1;

		[SerializeField]
		internal bool _isMoment = false;

		[MenuItem("Assets/Create/Create Event Collection")]
		public static void CreateEventCollection()
		{
			Util.ScriptableObjectUtil.CreateScriptableObject<EventCollection>();
		}
	}

	[CustomEditor(typeof(EventCollection))]
	public class EventCollectionEditor : Editor
	{
		private const int DROPDOWN_WIDTH = 70;

		private ReorderableList _list;
		private bool _dirty=false;

		private void OnEnable()
		{
        	_list = new ReorderableList(serializedObject, 
				serializedObject.FindProperty("Events"), 
                true, true, true, true);
			_list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Events"); };
			_list.drawElementCallback = DrawEventConfig;
			_list.onSelectCallback = SelectEventConfig;
			serializedObject.FindProperty(EventCollection.SELECTED_COMPONENT).intValue = -1;
    	}

	    public override void OnInspectorGUI()
	    {
			serializedObject.Update();
			if (!DrawMoment()) _list.DoLayoutList();
        	serializedObject.ApplyModifiedProperties();
			if (_dirty) EventEditor.InspectorUpdated();
			_dirty = false;

	    }

		private void DrawEventConfig(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty prop = _list.serializedProperty.GetArrayElementAtIndex(index);

			Rect rLabel = new Rect(rect.x, rect.y, rect.width-DROPDOWN_WIDTH, rect.height-2);
			Rect dLabel = new Rect(rect.x + rect.width - DROPDOWN_WIDTH + 2, rect.y, DROPDOWN_WIDTH - 2, rect.height-2);

			EditorGUI.PropertyField(rLabel, prop.FindPropertyRelative("Name"), GUIContent.none);
			EditorGUI.PropertyField(dLabel, prop.FindPropertyRelative("Setting"), GUIContent.none);
		}

		private void SelectEventConfig(ReorderableList list)
		{
			serializedObject.FindProperty(EventCollection.SELECTED_EVENT).intValue = _list.index;
			serializedObject.FindProperty(EventCollection.SELECTED_COMPONENT).intValue = -1;
			serializedObject.ApplyModifiedProperties();
			EventEditor.Show(serializedObject);
		}

		private bool DrawMoment()
		{
			if (serializedObject == null || _list == null || _list.index < 0) return false;
			int index = serializedObject.FindProperty(EventCollection.SELECTED_COMPONENT).intValue;
			if (index < 0) return false;

			SerializedProperty prop = serializedObject.FindProperty("Events");
			if (_list.index >= prop.arraySize) return false;


			string text;
			prop = prop.GetArrayElementAtIndex(_list.index);
			if (serializedObject.FindProperty(EventCollection.IS_MOMENT).boolValue)
			{
				prop = GetProp(prop, index, "Moments", "Editing Moment");
				if (prop == null) return false;
				text = GUILayout.TextArea(prop.FindPropertyRelative("Text").stringValue);
				_dirty = prop.FindPropertyRelative("Text").stringValue.CompareTo(text) != 0;
				if (_dirty) prop.FindPropertyRelative("Text").stringValue = text;
			}
			else
			{
				prop = GetProp(prop, index, "Links", "Editing Link");
				if (prop == null) return false;
				text = GUILayout.TextArea(prop.FindPropertyRelative("Text").stringValue);
				_dirty = prop.FindPropertyRelative("Text").stringValue.CompareTo(text) != 0;
				if (_dirty) prop.FindPropertyRelative("Text").stringValue = text;
			}
			return true;
		}

		private SerializedProperty GetProp(SerializedProperty config, int index, string whichProp, string labelText)
		{
			SerializedProperty prop = config.FindPropertyRelative(whichProp);
			if (index < prop.arraySize)
			{
				GUILayout.Label(labelText);
				return prop.GetArrayElementAtIndex(index);
			}
			serializedObject.FindProperty(EventCollection.SELECTED_COMPONENT).intValue = -1;
			return null;
		}
#endif
	}
}
