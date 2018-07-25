using System;
using Util;

#if (UNITY_EDITOR)
using UnityEngine;
using UnityEditor;
#endif

namespace Ambition
{
    public class IncidentConfig : DirectedGraphObject<MomentVO, TransitionVO>
    {
        public bool Active;
        public DateTime Date;

#if (UNITY_EDITOR)
        [HideInInspector]
        public int Selected;

        [HideInInspector]
        public bool IsNode;
#endif

        public IncidentVO GetIncident()
        {
            IncidentVO incident = new IncidentVO(this.Graph);
            incident.Name = this.name;
            incident.Active = Active;
            incident.Date = Date;
            return incident;
        }
    }

#if (UNITY_EDITOR)
    public class IncidentConfigDrawer : Editor
    {
        private SerializedProperty _graph;
        private SerializedProperty _nodes;
        private SerializedProperty _links;
        private SerializedProperty _linkData;

        private void OnEnable()
        {
            _graph = serializedObject?.FindProperty("Graph");
            if (_graph != null)
            {
                serializedObject.FindProperty("Selected").intValue = -1;
                _nodes = _graph.FindPropertyRelative("Nodes");
                _links = _graph.FindPropertyRelative("Links");
                _linkData = _graph.FindPropertyRelative("LinkData");
            }
        }

        private void OnDisable()
        {
            _graph = null;
            _nodes = null;
            _links = null;
            _linkData = null;
        }

        public void OnGUI()
        {
            if (_graph != null)
            {
                int index = serializedObject.FindProperty("Selected").intValue;
                if (index < 0)
                    DrawDefaultInspector();
                else if (serializedObject.FindProperty("IsMoment").boolValue)
                    EditorGUILayout.PropertyField(_nodes.GetArrayElementAtIndex(index), true);
                else
                    EditorGUILayout.PropertyField(_linkData.GetArrayElementAtIndex(index), true);
                
            }
        }
    }
    #endif
}
