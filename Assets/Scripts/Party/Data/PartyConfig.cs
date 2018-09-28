using System;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{
    [Serializable]
    public class PartyConfig : ScriptableObject
    {
        [SerializeField]
        private PartyVO _party;

        [SerializeField]
        private long _date = -1;
        public PartyVO Party
        {
            get
            {
                _party.Date = (_date > 0)
                    ? DateTime.MinValue.AddTicks(_date)
                    : default(DateTime);
                return _party;
            }
        }

#if (UNITY_EDITOR)
        [MenuItem("Assets/Create/Create Party")]
        public static void CreateParty()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<PartyConfig>("New Party");
        }
    }

    [CustomEditor(typeof(PartyConfig))]
    public class PartyConfigDrawer : Editor
    {
        private static readonly string[] MONTHS = new string[]{
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty party = serializedObject.FindProperty("_party");

            SerializedProperty dateProperty = serializedObject.FindProperty("_date");
            bool fixedDate = GUILayout.Toggle(dateProperty.longValue >= 0, "Fixed Date?");
            if (!fixedDate) dateProperty.longValue = -1;
            else
            {
                EditorGUILayout.BeginHorizontal();
                DateTime date = dateProperty.longValue > 0 ? DateTime.MinValue.AddTicks(dateProperty.longValue) : DateTime.MinValue;
                int day = Mathf.Clamp(EditorGUILayout.IntField(date.Day, GUILayout.MaxWidth(30f)), 1, DateTime.DaysInMonth(date.Year, date.Month));
                int month = EditorGUILayout.Popup(date.Month - 1, MONTHS) + 1;
                int year = Mathf.Clamp(EditorGUILayout.IntField(date.Year, GUILayout.MaxWidth(80f)), 1, 9999);
                dateProperty.longValue = new DateTime(year, month, day).Ticks;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.PropertyField(party.FindPropertyRelative("Faction"), true);
            EditorGUILayout.PropertyField(party.FindPropertyRelative("Importance"), true);
            party.FindPropertyRelative("RSVP").enumValueIndex =
                     EditorGUILayout.Toggle("Immediate", party.FindPropertyRelative("RSVP").enumValueIndex == (int)RSVP.Accepted)
                     ? (int)RSVP.Accepted : (int)RSVP.New;
            EditorGUILayout.PropertyField(party.FindPropertyRelative("MapID"), true);
            EditorGUILayout.PropertyField(party.FindPropertyRelative("Turns"), true);
            EditorGUILayout.PropertyField(party.FindPropertyRelative("Requirements"), true);
            EditorGUILayout.PropertyField(party.FindPropertyRelative("Rewards"), true);
            serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}