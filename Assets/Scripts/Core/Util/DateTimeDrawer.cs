#if (UNITY_EDITOR)
using System;
using UnityEngine;
using UnityEditor;

namespace Util
{
    public class DateTimeDrawer
    {
        static readonly string[] MONTHS = new string[]{
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        };

        public static long ShowDateTimeDrawer(SerializedProperty longDateProperty)
        {
            EditorGUILayout.BeginHorizontal();
            long result = longDateProperty.longValue;
            DateTime date = result > 0 ? DateTime.MinValue.AddTicks(result) : DateTime.MinValue;
            int day = Mathf.Clamp(EditorGUILayout.IntField(date.Day, GUILayout.MaxWidth(30f)), 1, DateTime.DaysInMonth(date.Year, date.Month));
            int month = EditorGUILayout.Popup(date.Month - 1, MONTHS) + 1;
            int year = Mathf.Clamp(EditorGUILayout.IntField(date.Year, GUILayout.MaxWidth(80f)), 1, 9999);
            result = new DateTime(year, month, day).Ticks;
            EditorGUILayout.EndHorizontal();
            return result;
        }
    }
}
#endif
