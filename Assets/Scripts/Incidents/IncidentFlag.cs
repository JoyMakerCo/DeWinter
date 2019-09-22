using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Ambition
{
    [Serializable]
    public enum IncidentFlagType
    {
        LowPeril, HighPeril, LowCred, HighCred, Cost
    }

    [Serializable]
    public struct IncidentFlag
    {
        public IncidentFlagType Type;
        public string Phrase; // Use to override default phrases
        public int Value; // Use to display associated value
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(IncidentFlag))]
    public class IncidentFlagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty flag, GUIContent label)
        {
            int index = flag.FindPropertyRelative("Type").enumValueIndex;
            IncidentFlagType[] types = (IncidentFlagType[])(Enum.GetValues(typeof(IncidentFlagType)));
            IncidentFlagType type = (types[index]);
            EditorGUILayout.BeginHorizontal();
            type = (IncidentFlagType)(EditorGUILayout.EnumPopup(type));
            flag.FindPropertyRelative("Type").enumValueIndex = Array.IndexOf(types, type);
            flag.FindPropertyRelative("Phrase").stringValue = EditorGUILayout.TextField(flag.FindPropertyRelative("Phrase").stringValue);
            flag.FindPropertyRelative("Value").intValue = EditorGUILayout.IntField(flag.FindPropertyRelative("Value").intValue);
            EditorGUILayout.EndHorizontal();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0f;
    }
#endif
}
