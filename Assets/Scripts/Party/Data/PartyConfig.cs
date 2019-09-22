using System;
using System.Linq;
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
        public PartyVO GetParty() => new PartyVO()
        {
            Name = name,
            Date = (_date > 0
                ? DateTime.MinValue.AddTicks(_date)
                : default),
            LocalizationKey = this.LocalizationKey,
            Description = this.Description,
            Invitation = this.Invitation,
            Faction = this.Faction,
            RSVP = this.RSVP,
            Size = GetSize(),
//            Guests = Guests.Select(g=>AmbitionApp.GetModel<CharacterModel>().Characters.First(c=>c.Name == g)).ToArray(),
            IntroIncident = this.IntroIncident?.GetIncident(),
            ExitIncident = this.ExitIncident?.GetIncident(),
            Host = this.Host,
            RequiredIncidents = GetRequiredIncidents(),
            SupplementalIncidents = this.SupplementalIncidents?.Select(i => i.GetIncident()).ToArray(),
            Requirements = this.Requirements?.ToArray() ?? new CommodityVO[0],
            Rewards = this.Rewards?.ToList() ?? new System.Collections.Generic.List<CommodityVO>(),
            Map = Map?.CreateMapVO()
        };

        public FactionType Faction;
        public PartySize Size;
        public RSVP RSVP;
        public string Host;
        public int Turns;
        public MapView Map;
        public IncidentConfig[] RequiredIncidents;
        public IncidentConfig[] SupplementalIncidents;
        public CommodityVO[] Requirements;
        public CommodityVO[] Rewards;

        private PartySize GetSize()
        {
            int count = RequiredIncidents?.Length ?? 0;
            if ((int)Size > count) return Size;
            return (count <= (int)(PartySize.Trivial)) ? PartySize.Trivial
                : (count >= (int)(PartySize.Grand)) ? PartySize.Grand
                : PartySize.Decent;
        }

        private IncidentVO[] GetRequiredIncidents()
        {
            if (RequiredIncidents == null) return new IncidentVO[0];
            IncidentVO[] result = new IncidentVO[RequiredIncidents.Length];
            for (int i = RequiredIncidents.Length - 1; i >= 0; i--)
                result[i] = RequiredIncidents[i]?.GetIncident();
            return result;
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
