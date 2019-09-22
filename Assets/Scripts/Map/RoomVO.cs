using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{
	public class RoomVO
	{
		[JsonProperty("name")]
		public string Name;

		//[JsonProperty("difficulty")]
	    //public int Difficulty; //Difficulty 1-5

		//[JsonProperty("moveThroughChance")]
		//public int MoveThroughChance = -1; // -1 indicates a value not yet assigned

        public bool Cleared = false;
        //public bool Revealed = false;
        //public bool Visited = false;

        [JsonProperty("background")]
		public Sprite Background;

        [JsonProperty("rewards")]
        public CommodityVO [] Rewards; //Granted after finishing a room

        public IncidentVO Incident;

        [SerializeField]
        private IncidentConfig _incident;

        [JsonProperty("actions")]
        public CommodityVO[] Actions; //Granted when the player enters a room

        public CharacterVO [] Guests;
        public int NumGuests => Guests?.Length ?? 0;

        public RoomVO() { }
        public RoomVO(RoomVO room)
        {
            Name = room.Name;
            Background = room.Background;
            Incident = room.Incident ?? _incident?.GetIncident();
            Guests = room.Guests == null ? null : new CharacterVO[room.NumGuests];
            for (int i=NumGuests-1; i>=0; i--)
                Guests[i] = new CharacterVO(room.Guests[i]);
            if (room.Actions != null)
            {
                Actions = new CommodityVO[room.Actions.Length];
                for (int i = Actions.Length - 1; i >= 0; i--)
                    Actions[i] = new CommodityVO(room.Actions[i]);
            }
            if (room.Rewards != null)
            {
                Rewards = new CommodityVO[room.Rewards.Length];
                for (int i = Rewards.Length - 1; i >= 0; i--)
                    Rewards[i] = new CommodityVO(room.Rewards[i]);
            }
        }

        //public bool HostHere => (Features != null) && (Array.IndexOf(Features, PartyConstants.HOST) >= 0);

        //public bool IsAdjacentTo(RoomVO room)
        //{
        //	return Array.IndexOf(Doors, room) >= 0;
        //}

        //public int[] Bounds { get; private set; }

        //      public int[] _vertices;
        //[JsonProperty("vertices")]
        //public int[] Vertices
        //{
        //	get { return _vertices; }
        //	set {
        //		_vertices = value;
        //		Bounds = new int[4];
        //		Bounds[0] = _vertices.Where((v,i)=>i%2==0).Min();
        //		Bounds[1] = _vertices.Where((v,i)=>i%2==1).Min();
        //		Bounds[2] = _vertices.Where((v,i)=>i%2==0).Max();
        //		Bounds[3] = _vertices.Where((v,i)=>i%2==1).Max();
        //	}
        //}

        // Set in the map file or map generation 
        //public RoomVO[] Doors;

        public override string ToString ()
		{
			return "[RoomVO: " + Name + "]";
		}
	}
    
#if (UNITY_EDITOR)
    [CustomEditor(typeof(RoomVO))]
    [CanEditMultipleObjects]
    public class RoomVODrawer : Editor
    {
        public IncidentConfig Incident;
         
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.ObjectField(serializedObject.FindProperty("_incident"), typeof(IncidentConfig), new GUIContent("Incident"));
            //bool fixedDate = GUILayout.Toggle(dateProperty.longValue >= 0, "Fixed Date?");
            //if (!fixedDate) dateProperty.longValue = -1;
            //else
            //{
            //    EditorGUILayout.BeginHorizontal();
            //    DateTime date = dateProperty.longValue > 0 ? DateTime.MinValue.AddTicks(dateProperty.longValue) : DateTime.MinValue;
            //    int day = Mathf.Clamp(EditorGUILayout.IntField(date.Day, GUILayout.MaxWidth(30f)), 1, DateTime.DaysInMonth(date.Year, date.Month));
            //    int month = EditorGUILayout.Popup(date.Month - 1, MONTHS) + 1;
            //    int year = Mathf.Clamp(EditorGUILayout.IntField(date.Year, GUILayout.MaxWidth(80f)), 1, 9999);
            //    dateProperty.longValue = new DateTime(year, month, day).Ticks;
            //    EditorGUILayout.EndHorizontal();
            //}

            //EditorGUILayout.LabelField("Invitation Text (Leave Blank to auto-generate)");
            //SerializedProperty text = serializedObject.FindProperty("Invitation");
            //text.stringValue = GUILayout.TextArea(text.stringValue);

            //EditorGUILayout.PropertyField(serializedObject.FindProperty("Host"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("IntroIncident"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("ExitIncident"), true);

            //EditorGUILayout.LabelField("Length of Incident list will determine the Party Size. Leave list Empty to Auto-generate.");
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("Incidents"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("Faction"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("RSVP"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("LocalizationKey"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("Background"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("MapID"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("Turns"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("Requirements"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("Rewards"), true);
            //serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
