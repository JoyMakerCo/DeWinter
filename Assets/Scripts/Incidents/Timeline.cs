using UnityEngine;
using System;
using System.Collections.Generic;
using Util;

#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditorInternal;
#endif

namespace Ambition
{
    [Serializable]
    public class Timeline : ScriptableObject
    {
        [HideInInspector]
        public IncidentConfig[] Incidents;

        private void OnEnable()
        {
            if (Incidents == null)
                Incidents = new IncidentConfig[0];
        }

#if (UNITY_EDITOR)

        public void SetIncidentConfigs(IncidentConfig[] configs)
        {
            SerializedObject obj = new SerializedObject(this);
            obj.Update();
            SerializedProperty incidents = obj.FindProperty("Incidents");
            incidents.arraySize = configs.Length;
            for (int i = configs.Length - 1; i >= 0; i--)
            {
                incidents.GetArrayElementAtIndex(i).objectReferenceInstanceIDValue = configs[i].GetInstanceID();
            }
            obj.ApplyModifiedProperties();
        }
#endif
    }

#if (UNITY_EDITOR)
    [CustomEditor(typeof(Timeline))]
    public class TimelineEditor : Editor
    {
        private ReorderableList _scheduledIncidents;
        private ReorderableList _randomIncidents;

        private void OnEnable()
        {
            _scheduledIncidents = new ReorderableList(serializedObject,
                                            serializedObject.FindProperty("Incidents"),
                                            true, true, true, true)
            {
                drawElementCallback = DrawIncident,
                drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, new GUIContent("Scheduled Incidents"));}
                //_incidents.onReorderCallback = ReorderIncident;
            };
            /*           _randomIncidents = new ReorderableList(serializedObject,
                                                       serializedObject.FindProperty("Incidents"),
                                                       false, true, true, true)
                       {
                           drawElementCallback = DrawIncident,
                           drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, new GUIContent("Random Incidents")); }
                       };
           */
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            _scheduledIncidents.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        public void CreateIncident(ReorderableList list)
        {
            EditorUtility.SetDirty(this);
            IncidentConfig config = ScriptableObjectUtil.CreateScriptableObject<IncidentConfig>("Incidents/New Incident");
            bool selected = (list.index >= 0);
            SerializedProperty current = list.serializedProperty.GetArrayElementAtIndex(list.index);
            EditorUtility.SetDirty(config);
            if (selected) list.serializedProperty.InsertArrayElementAtIndex(list.index);
            else list.index = list.serializedProperty.arraySize++;
            list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue = config;
            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("Assets/Create/Create Timeline")]
        public static void CreateTimeline()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<IncidentCollection>("New Timeline");
        }

        private void DrawIncident(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty prop = _scheduledIncidents.serializedProperty.GetArrayElementAtIndex(index);
            //EditorGUI.IntField(new Rect(0, 2, 20, rect.height-2), prop.FindPropertyRelative("Day").intValue);
            //EditorGUI.Popup(new Rect(22, 2, 28, rect.height-2), prop.FindPropertyRelative("Month").intValue, IncidentConfig.MONTHS);
            //EditorGUI.IntField(new Rect(52, 2, 36, rect.height - 2), prop.FindPropertyRelative("Year").intValue);
            //EditorGUI.ObjectField(new Rect(90, 2, rect.width-88, rect.height - 2), prop, GUIContent.none);
            EditorGUI.ObjectField(rect, prop, GUIContent.none);
        }
    }
#endif
}
