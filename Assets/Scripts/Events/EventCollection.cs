﻿using System;
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
		public EventVO[] Events;

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
		private const int INT_WIDTH = 20;
		private const int SPACER = 2;
		private const string FOCUS_ID = "FOCUS_ID";

		private ReorderableList _list;
		private bool _dirty=false;
		private SerializedProperty _property;
		
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

		void OnFocus()
		{
		}

	    public override void OnInspectorGUI()
	    {
			serializedObject.Update();

			if (!DrawComponent()) _list.DoLayoutList();
        	serializedObject.ApplyModifiedProperties();
			if (_dirty) EventEditor.InspectorUpdated();
			_dirty = false;

		// 	EditorGUI.FocusTextInControl(FOCUS_ID);
		// 	TextEditor txt = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);  
        //    if (txt != null) txt.SelectAll();
	    }

	    private bool DrawComponent()
	    {
			if (serializedObject == null || _list == null || _list.index < 0) return false;
			int index = serializedObject.FindProperty(EventCollection.SELECTED_COMPONENT).intValue;
			if (index < 0) return false;

			SerializedProperty prop = serializedObject.FindProperty("Events");
			if (_list.index >= prop.arraySize) return false;

			prop = prop.GetArrayElementAtIndex(_list.index);
			if (serializedObject.FindProperty(EventCollection.IS_MOMENT).boolValue)
			{
				prop = GetProp(prop, index, "Moments", "Editing Moment");
				if (prop != null) DrawMoment(prop);
			}
			else
			{
				prop = GetProp(prop, index, "Links", "Editing Link");
				if (prop != null) DrawLink(prop);
			}
			return prop != null;
	    }

		private void DrawEventConfig(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty prop = _list.serializedProperty.GetArrayElementAtIndex(index);

			Rect rLabel = new Rect(rect.x, rect.y, rect.width-DROPDOWN_WIDTH, rect.height-SPACER);
			Rect dLabel = new Rect(rect.x + rect.width - DROPDOWN_WIDTH + SPACER, rect.y, DROPDOWN_WIDTH-SPACER, rect.height-SPACER);

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

		private void DrawMoment(SerializedProperty moment)
		{
			EditorGUI.BeginChangeCheck();
			GUI.SetNextControlName(FOCUS_ID);
			SerializedProperty text = moment.FindPropertyRelative("Text");
			SerializedProperty terewardsxt = moment.FindPropertyRelative("Rewards");
			text.stringValue = GUILayout.TextArea(text.stringValue);
			EditorGUILayout.PropertyField(moment.FindPropertyRelative("Rewards"), true);
			_dirty = EditorGUI.EndChangeCheck();
		}

		private void DrawLink(SerializedProperty link)
		{
			EditorGUI.BeginChangeCheck();
			GUI.SetNextControlName(FOCUS_ID);
			SerializedProperty text = link.FindPropertyRelative("Text");
			text.stringValue = GUILayout.TextArea(text.stringValue);
			_dirty = EditorGUI.EndChangeCheck();
		}

		private void DrawReward(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty prop = _list.serializedProperty.GetArrayElementAtIndex(index);
			float h = rect.height-SPACER;

			Rect dRect = new Rect(rect.x, rect.y, DROPDOWN_WIDTH, h);
			Rect sRect = new Rect(rect.x + DROPDOWN_WIDTH + SPACER, rect.y, rect.width-INT_WIDTH-SPACER, h);
			Rect qRect = new Rect(rect.x + rect.width-INT_WIDTH, rect.y, INT_WIDTH, h);

			EditorGUI.PropertyField(dRect, prop.FindPropertyRelative("Type"), GUIContent.none);
			EditorGUI.PropertyField(sRect, prop.FindPropertyRelative("ID"), GUIContent.none);
			EditorGUI.PropertyField(qRect, prop.FindPropertyRelative("Amount"), GUIContent.none);
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
