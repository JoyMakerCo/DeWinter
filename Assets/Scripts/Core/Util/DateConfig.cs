using System;
using UnityEngine;

#if (UNITY_EDITOR)
using System.Globalization;
using UnityEditor;
#endif

namespace Util
{
    [Serializable]
    public class DateConfig
    {
        [SerializeField]
        private long _ticks;
        public DateTime GetDateTime() => new DateTime(_ticks);
    }

#if (UNITY_EDITOR)
    [CustomPropertyDrawer(typeof(DateConfig))]
    public class DateConfigDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            int indent = EditorGUI.indentLevel;
            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;

            DateTime date = DateTime.MinValue;
            long ticks = property.FindPropertyRelative("_ticks").longValue;
            date = new DateTime(ticks < 0 ? 0 : ticks);

            Rect dy = new Rect(position.x, position.y, 30f, position.height);
            Rect mo = new Rect(position.x + 30f, position.y, 100f, position.height);
            Rect yr = new Rect(position.x + 130f, position.y, 60f, position.height);

            int year = Mathf.Clamp(EditorGUI.IntField(yr, date.Year), 1, 9999);
            int month = 1 + EditorGUI.Popup(mo, date.Month-1, CultureInfo.CurrentCulture.DateTimeFormat.MonthNames);
            int day = Mathf.Clamp(EditorGUI.IntField(dy, date.Day), 1, DateTime.DaysInMonth(year, month));

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
            property.FindPropertyRelative("_ticks").longValue = (new DateTime(year, month, day).Ticks);
        }
    }
#endif
}
