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
	public class IncidentCollection : ScriptableObject
	{
		public IncidentVO[] Incidents;

#if (UNITY_EDITOR)
		public const string SELECTED_INCIDENT = "_selectedIncident";
		public const string SELECTED_COMPONENT = "_selectedComponent";
		public const string IS_MOMENT = "_isMoment";

		[SerializeField]
		internal int _selectedIncident = -1;

		[SerializeField]
		internal int _selectedComponent = -1;

		[SerializeField]
		internal bool _isMoment = false;

		[MenuItem("Assets/Create/Create Incident Collection")]
		public static void CreateIncidentCollection()
		{
			Util.ScriptableObjectUtil.CreateScriptableObject<IncidentCollection>();
		}
	}

	[CustomEditor(typeof(IncidentCollection))]
	public class IncidentCollectionEditor : Editor
	{
		private const int DROPDOWN_WIDTH = 70;
		private const int INT_WIDTH = 20;
		private const int SPACER = 2;
		private const string FOCUS_ID = "FOCUS_ID";

		private int _lastIndex;
		private string _lastType;
		private ReorderableList _list;
		private bool _dirty=false;
		private SerializedProperty _property;
		
		private void OnEnable()
		{
        	_list = new ReorderableList(serializedObject, 
				serializedObject.FindProperty("Incidents"), 
                true, true, true, true);
			_list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Incidents"); };
			_list.drawElementCallback = DrawIncident;
			_list.onSelectCallback = SelectIncident;
			_list.onAddCallback = CreateIncident;

			serializedObject.FindProperty(IncidentCollection.SELECTED_COMPONENT).intValue = -1;
		}

	    public override void OnInspectorGUI()
	    {
			serializedObject.Update();
			if (!DrawComponent()) _lastType = null;
			if (_lastType == null) _list.DoLayoutList();
        	serializedObject.ApplyModifiedProperties();
			if (_dirty) IncidentEditor.InspectorUpdated();
			_dirty = false;
	    }

	    private bool DrawComponent()
	    {
			if (serializedObject == null || _list == null || _list.index < 0) return false;
			int index = serializedObject.FindProperty(IncidentCollection.SELECTED_COMPONENT).intValue;
			if (index < 0) return false;
			SerializedProperty prop = serializedObject.FindProperty("Incidents");
			if (_list.index >= prop.arraySize) return false;
			bool selectText = _lastIndex != index;
			_lastIndex = index;
			prop = prop.GetArrayElementAtIndex(_list.index);
			if (serializedObject.FindProperty(IncidentCollection.IS_MOMENT).boolValue)
			{
				if (_lastType != "Moment") selectText = true;
				_lastType = "Moment";
				prop = GetProp(prop, index, "Moments", "Editing Moment");
				if (prop != null) DrawMoment(prop);
			}
			else
			{
				if (_lastType != "Transition") selectText = true;
				_lastType = "Transition";
				prop = GetProp(prop, index, "Transitions", "Editing Transitions");
				if (prop != null) DrawTransition(prop);
			}
			if (prop != null && selectText)
			{
				EditorGUI.FocusTextInControl(FOCUS_ID);
				TextEditor txt = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);  
				if (txt != null)
				{
					txt.SelectAll();
					Array.Find(Resources.FindObjectsOfTypeAll<EditorWindow>(), w=>w.title == "Inspector").Focus();
				}
			}
			return prop != null;
	    }

		private void DrawIncident(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty prop = _list.serializedProperty.GetArrayElementAtIndex(index);

			Rect rLabel = new Rect(rect.x, rect.y, rect.width-DROPDOWN_WIDTH, rect.height-SPACER);
			Rect dLabel = new Rect(rect.x + rect.width - DROPDOWN_WIDTH + SPACER, rect.y, DROPDOWN_WIDTH-SPACER, rect.height-SPACER);

			EditorGUI.PropertyField(rLabel, prop.FindPropertyRelative("Name"), GUIContent.none);
			EditorGUI.PropertyField(dLabel, prop.FindPropertyRelative("Setting"), GUIContent.none);
		}

		private void SelectIncident(ReorderableList list)
		{
			serializedObject.FindProperty(IncidentCollection.SELECTED_INCIDENT).intValue = _list.index;
			serializedObject.FindProperty(IncidentCollection.SELECTED_COMPONENT).intValue = -1;
			serializedObject.ApplyModifiedProperties();
			IncidentEditor.Show(serializedObject);
		}

		private void CreateIncident(ReorderableList list)
		{
			SerializedProperty incidents = serializedObject.FindProperty("Incidents");
			int index = incidents.arraySize;
			incidents.arraySize++;
			SerializedProperty incident = incidents.GetArrayElementAtIndex(index);
			incident.FindPropertyRelative("Name").stringValue = "New Incident";
			incident.FindPropertyRelative("Moments").arraySize = 0;
			incident.FindPropertyRelative("Transitions").arraySize = 0;
			incident.FindPropertyRelative("Positions").arraySize = 0;
		}

		private void DrawMoment(SerializedProperty moment)
		{
			EditorGUI.BeginChangeCheck();
			GUI.SetNextControlName(FOCUS_ID);
			SerializedProperty text = moment.FindPropertyRelative("Text");
			text.stringValue = GUILayout.TextArea(text.stringValue);
			EditorGUILayout.PropertyField(moment.FindPropertyRelative("Background"), true);
			EditorGUILayout.PropertyField(moment.FindPropertyRelative("Character1"), true);
			EditorGUILayout.PropertyField(moment.FindPropertyRelative("Character2"), true);
			EditorGUILayout.PropertyField(moment.FindPropertyRelative("Speaker"), true);
			EditorGUILayout.PropertyField(moment.FindPropertyRelative("Rewards"), true);
			_dirty = EditorGUI.EndChangeCheck();
		}

		private void DrawTransition(SerializedProperty transition)
		{
			EditorGUI.BeginChangeCheck();
			GUI.SetNextControlName(FOCUS_ID);
			SerializedProperty text = transition.FindPropertyRelative("Text");
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
			serializedObject.FindProperty(IncidentCollection.SELECTED_COMPONENT).intValue = -1;
			return null;
		}
#endif
	}
}
