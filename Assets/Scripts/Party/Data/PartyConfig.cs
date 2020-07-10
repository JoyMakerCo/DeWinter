using System;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{
    [Serializable]
    public class PartyConfig : ScriptableObject
    {
        public string Description;
        public IncidentConfig IntroIncident;
        public IncidentConfig ExitIncident;
        public string LocalizationKey;
        public string Invitation;
        public Sprite EstablishingShot;
        public string[] Guests;

        [SerializeField]
        private long _date = -1;
        public long Date => _date;

        public FactionType Faction;
        public PartySize Size;
        public RSVP RSVP;
        public string Host;
        public int Turns;
        public MapView Map;
        public IncidentConfig[] RequiredIncidents;
        public IncidentConfig[] SupplementalIncidents;

        [SerializeField]
        public RequirementVO[] Requirements;
        public CommodityVO[] Rewards;

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

            EditorGUILayout.LabelField("Invitation Text (Leave Blank to auto-generate)");
            SerializedProperty text = serializedObject.FindProperty("Invitation");
            text.stringValue = GUILayout.TextArea(text.stringValue);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Host"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("IntroIncident"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ExitIncident"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RequiredIncidents"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SupplementalIncidents"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Faction"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Size"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RSVP"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LocalizationKey"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Map"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EstablishingShot"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Requirements"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Rewards"), true);
            serializedObject.ApplyModifiedProperties();
        }
#endif
    }
}
