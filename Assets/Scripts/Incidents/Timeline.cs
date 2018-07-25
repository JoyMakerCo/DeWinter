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
            _incidents.onSelectCallback = SelectIncident;
            //_incidents.onReorderCallback = ReorderIncident;
        }

        public override void OnInspectorGUI()
        {
            _incidents.DoLayoutList();
        }

        public void CreateIncident(ReorderableList list)
        {
            CreateIncident();
        }

        public IncidentConfig CreateIncident()
        {
            IncidentConfig config = ScriptableObjectUtil.CreateScriptableObject<IncidentConfig>("Incidents/New Incident");
            EditorUtility.SetDirty(this);
            if (_incidents.index >= 0)
            {
                EditorUtility.SetDirty(config);
                config.Date = ((IncidentConfig)_incidents.list[_incidents.index]).Date;
            }
            _incidents.list.Add(config);
            return config;
        }

        private void SelectIncident(ReorderableList list)
        {
            int index = list.index;
            if (index >= 0 && index < ((Timeline)target).Incidents.Length)
            {
                IncidentConfig config = ((Timeline)target).Incidents[index];
                IncidentEditor.Show(config);
            }
        }

        [MenuItem("Assets/Create/Create Timeline")]
        public static void CreateTimeline()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<IncidentCollection>("New Timeline");
        }

        private void DrawIncident(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty prop = _incidents.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.ObjectField(rect, prop, GUIContent.none);
        }
    }
#endif
}
