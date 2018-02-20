using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ambition
{
	[Serializable]
	public class EventConfig
	{
		public string Name;
		public EventSetting Setting;

		public string[] Moments;
		public EventLinkVO[] Links;

		#if (UNITY_EDITOR)
		public Vector2[] Positions;
		#endif

	}

	[Serializable]
	public struct EventLinkVO
	{
		public int Index;
		public int Target;
		public string Text;

		public string[] Rewards;
		public int[] Quantities;
	}

	[CustomPropertyDrawer(typeof(EventConfig))]
	public class EventConfigDrawer : PropertyDrawer
	{
		private const int DROPDOWN_WIDTH = 70;
		private const int BTN_WIDTH = 50;
		private const int BTN_OFFSET = 15;

	    // Draw the property inside the given rect
	    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	    {
	    	Rect rLabel = new Rect(position.x, position.y, position.width-BTN_WIDTH-DROPDOWN_WIDTH, position.height);
			Rect dLabel = new Rect(position.width-BTN_WIDTH-DROPDOWN_WIDTH, position.y, DROPDOWN_WIDTH+BTN_OFFSET, position.height);
			Rect rBtn = new Rect(position.width-BTN_WIDTH+BTN_OFFSET, position.y, BTN_WIDTH, position.height);

			EditorGUI.BeginProperty(position, GUIContent.none, property);
			EditorGUI.PropertyField(rLabel, property.FindPropertyRelative("Name"), GUIContent.none);
			EditorGUI.PropertyField(dLabel, property.FindPropertyRelative("Setting"), GUIContent.none);
			if (GUI.Button(rBtn, "Edit"))
			{
				EventCollection collection = (EventCollection)(property.serializedObject.targetObject);
				int index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
				collection._selectedConfig = collection.Events[index];
				EventEditor.Show(property.serializedObject);
			}
			EditorGUI.EndProperty();
        }
	}
}
