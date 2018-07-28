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
        public IncidentConfig[] Incidents;
    }

#if (UNITY_EDITOR)
    [CustomEditor(typeof(Timeline))]
    public class TimelineEditor : Editor
    {
        private ReorderableList _incidents;

        private void OnEnable()
        {
            _incidents = new ReorderableList(serializedObject,
                                            serializedObject.FindProperty("Incidents"),
                                            true, false, true, true);
            _incidents.drawElementCallback = DrawIncident;
            _incidents.onAddCallback = CreateIncident;
            //_incidents.onReorderCallback = ReorderIncident;
        }

        public override void OnInspectorGUI()
        {
            _incidents.DoLayoutList();
        }

        public void CreateIncident(ReorderableList list)
        {
            EditorUtility.SetDirty(this);
            IncidentConfig config = ScriptableObjectUtil.CreateScriptableObject<IncidentConfig>("Incidents/New Incident");
            bool selected = (list.index >= 0);
            SerializedProperty current = list.serializedProperty.GetArrayElementAtIndex(list.index);
            EditorUtility.SetDirty(config);
            config.Day = selected ? current.FindPropertyRelative("Day").intValue : 1;
            config.Month = selected ? current.FindPropertyRelative("Month").intValue : 1;
            config.Year = selected ? current.FindPropertyRelative("Year").intValue : 1795;
            if (selected) list.serializedProperty.InsertArrayElementAtIndex(list.index);
            else list.index = list.serializedProperty.arraySize++;
            list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue = config;
        }

        [MenuItem("Assets/Create/Create Timeline")]
        public static void CreateTimeline()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<IncidentCollection>("New Timeline");
        }

        private void DrawIncident(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty prop = _incidents.serializedProperty.GetArrayElementAtIndex(index);

            //EditorGUI.IntField(new Rect(0, 2, 20, rect.height-2), prop.FindPropertyRelative("Day").intValue);
            //EditorGUI.Popup(new Rect(22, 2, 28, rect.height-2), prop.FindPropertyRelative("Month").intValue, IncidentConfig.MONTHS);
            //EditorGUI.IntField(new Rect(52, 2, 36, rect.height - 2), prop.FindPropertyRelative("Year").intValue);
            //EditorGUI.ObjectField(new Rect(90, 2, rect.width-88, rect.height - 2), prop, GUIContent.none);
            EditorGUI.ObjectField(rect, prop, GUIContent.none);
        }
    }
#endif
}
