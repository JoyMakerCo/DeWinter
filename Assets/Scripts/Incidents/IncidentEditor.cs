#if (UNITY_EDITOR)
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;  
using UnityEditor;
using Util;

namespace Ambition
{
    public class IncidentEditor : EditorWindow
    {
        private const int NUM_VISIBLE_LINES = 4;
        private const float CONNECT_BTN_WIDTH = 10f;
        private const float LINK_INTERSECT_WIDTH = 5f;

        private SerializedObject _object;
        private IncidentConfig _config;
        private Vector2 _scroll;
        private Rect _scrollRect;
        private Vector2 _mousePos;
        private int _selection;
        private bool _isNode;

        private SerializedProperty _graph;
        private SerializedProperty _nodes;
        private SerializedProperty _links;
        private SerializedProperty _linkData;
        private SerializedProperty _positions;

        public static void Show(SerializedObject obj)
        {
            IncidentEditor editor = EditorWindow.GetWindow<IncidentEditor>();
            if (obj == null || !(obj.targetObject is IncidentConfig)) editor.OnDestroy();
            else editor.SetConfig(obj);
        }

        public static void Show(IncidentConfig config)
        {
            IncidentEditor editor = EditorWindow.GetWindow<IncidentEditor>();
            if (config == null) editor.OnDestroy();
            else editor.SetConfig(new SerializedObject(config));
        }

        private void SetConfig(SerializedObject obj)
        {
            _config = obj.targetObject as IncidentConfig;
            titleContent.text = _config.name;

            _graph = _object.FindProperty("Graph");
            _nodes = _graph.FindPropertyRelative("Nodes");
            _links = _graph.FindPropertyRelative("Links");
            _linkData = _graph.FindPropertyRelative("LinkData");
            _positions = _graph.FindPropertyRelative("Positions");

            _scrollRect = new Rect();
            Repaint();
        }

	    void OnSelectionChange()
	    {
	    	OnDestroy();
	    }

	    void OnDestroy()
	    {
            _graph = null;
            _nodes = null;
            _links = null;
            _linkData = null;
            _positions = null;
            _config = null;
            _object = null;
			titleContent.text = "Event Editor";
			Repaint();
		}

	    public void OnGUI()
	    {
	    	if (_config != null)
	    	{
                _object.Update();
				_scroll = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scroll, _scrollRect);
				switch(Event.current.type)
				{
					case EventType.KeyDown:
                        if (Event.current.commandName == "Delete" || Event.current.commandName == "SoftDelete")
                        {
                            DeleteSelected();
                        }
                        else if (Event.current.modifiers == EventModifiers.None && _selection >= 0)
						{
                            EditorWindow w = GetWindow<EditorWindow>("UnityEditor.InspectorWindow");
                            if (w != null) w.Focus();
						}
						break;
                    case EventType.MouseDown:
						_mousePos = Event.current.mousePosition;
                        _selection = Array.FindIndex(_config.Positions, r => r.Contains(_mousePos));
                        _isNode = _selection >= 0;
                        if (!_isNode) _selection = Array.FindIndex(_config.Graph.Links, l => IntersectLink(_mousePos, l));
                            
                        if (Event.current.clickCount > 1)
                        {
                            CreateNewNode();
                            _selection = _config.Positions.Length;
                            _isNode = true;
                        }
						break;

					case EventType.MouseDrag:
                        if (_selection >= 0)
                        {
                            if (_isNode)
                            {
                                Rect rect = _positions.GetArrayElementAtIndex(_selection).rectValue;
                                rect.center += Event.current.mousePosition - _mousePos;
                                _positions.GetArrayElementAtIndex(_config.Selected).rectValue = rect;
                                _mousePos = Event.current.mousePosition;
                                AdjustScrollRect(rect);
                            }
                        }
                        break;

					case EventType.MouseUp:
                        _mousePos = Event.current.mousePosition;
						break;
				}
                _object.FindProperty("Selected").intValue = _selection;
                _object.FindProperty("IsNode").boolValue = _isNode;
                Repaint();
                _object.ApplyModifiedProperties();
                _config = _object.targetObject as IncidentConfig;

                for (int i = _links.arraySize - 1; i >= 0; i--)
                {
                    DrawLink(i, i == _selection);
                }
                DrawStartOutline();
                for (int i = _positions.arraySize - 1; i >= 0; i--)
                {
                    GUI.color = (i == _selection ? Color.yellow : Color.white);
                    GUI.Box(_positions.GetArrayElementAtIndex(i).rectValue, _config.Graph.Nodes[i].Text);
                }
				GUI.EndScrollView();
			}
	    }

        protected bool IntersectLink(Vector2 point, Vector2Int link)
        {
            Vector2 from = _config.Positions[link.x].center;
            Vector2 to = _config.Positions[link.y].center;
            Vector2 p = point - from;
            float P = p.sqrMagnitude;
            Vector2 l = (from - to);
            float L = l.sqrMagnitude;
            float dot = Vector2.Dot(p, l);
            return
                (dot >= 0) && (dot * dot / L <= L)
                && (Math.Abs(P - (dot * dot / L)) <= LINK_INTERSECT_WIDTH * LINK_INTERSECT_WIDTH);
        }

        protected virtual void DrawLink(int index, bool selected)
        {
            Vector2Int link = _links.GetArrayElementAtIndex(index).vector2IntValue;
            Vector2 from = _positions.GetArrayElementAtIndex(link.x).rectValue.center;
            Vector2 to = _positions.GetArrayElementAtIndex(link.y).rectValue.center;
            Vector3 dir = (to - from).normalized * LINK_INTERSECT_WIDTH;
            Vector3 norm = new Vector2(dir.y, -dir.x) * LINK_INTERSECT_WIDTH;
            Vector3 mid = (to + from) * .5f;

            // ...AND I'LL FORM THE HEAD
            Vector3[] head = new Vector3[]{
                mid - dir - norm,
                mid + dir,
                mid - dir + norm
            };
            Handles.color = selected ? Color.white : Color.black;
            Handles.DrawLine(from, to);
            Handles.DrawAAConvexPolygon(head);
        }

		private void SetAsStartNode()
		{
            if (_isNode && _selection > 0 && _selection < _config.Positions.Length)
			{
                _nodes.InsertArrayElementAtIndex(0);
                _nodes.GetArrayElementAtIndex(0).objectReferenceValue = _nodes.GetArrayElementAtIndex(_selection + 1).objectReferenceValue;
                DeleteIndex(_nodes, _selection + 1);
                _selection = 0;
			}
		}

		private void DeleteSelected()
		{
            if (_isNode)
            {
                Vector2Int link;
                DeleteIndex(_nodes, _selection);
                DeleteIndex(_positions, _selection);
                for (int i = _links.arraySize - 1; i >= 0; i--)
                {
                    link = _links.GetArrayElementAtIndex(i).vector2IntValue;
                    if (link.x == _selection || link.y == _selection)
                    {
                        DeleteIndex(_links, i);
                        DeleteIndex(_linkData, i);
                    }
                }
            }
            else
            {
                DeleteIndex(_links, _selection);
                DeleteIndex(_linkData, _selection);
            }
            _selection = -1;
		}

        private void DeleteIndex(SerializedProperty array, int index)
        {
            if (array != null && index >= 0 && index < array.arraySize)
            {
                array.DeleteArrayElementAtIndex(index);
                if (array.GetArrayElementAtIndex(index) == null)
                    array.DeleteArrayElementAtIndex(index);
            }
        }


		private void CreateNewNode(int fromNode=-1)
		{
            _selection = _nodes.arraySize++;
            _nodes.InsertArrayElementAtIndex(_selection);
            _nodes.GetArrayElementAtIndex(_selection).FindPropertyRelative("Text").stringValue = "New Moment";
            if (fromNode >= 0) CreateLink(fromNode, _selection);
		}

		private void CreateLink(int start, int end)
		{
            if (start != end && start >= 0 && start < _config.Positions.Length && end >= 0 && end < _config.Positions.Length)
			{
                _selection = _links.arraySize++;
                _links.InsertArrayElementAtIndex(_selection);
                _links.GetArrayElementAtIndex(_selection).vector2IntValue = new Vector2Int(start, end);

                _selection = _linkData.arraySize++;
                _linkData.InsertArrayElementAtIndex(_selection);
                _isNode = true;
			}
	    }

		private void AdjustScrollRect(Rect rect)
		{
			if (rect.xMin < _scrollRect.xMin) _scrollRect.xMin = rect.xMin;
			else if (rect.xMax > _scrollRect.xMax) _scrollRect.xMax = rect.xMax;
			if (rect.yMin < _scrollRect.yMin) _scrollRect.yMin = rect.yMin;
			else if (rect.yMax > _scrollRect.yMax) _scrollRect.yMax = rect.yMax;
		}

        private void AdjustScrollRect()
        {
            if (_mousePos.x < _scrollRect.xMin) _scrollRect.xMin = _mousePos.x;
            else if (_mousePos.x > _scrollRect.xMax) _scrollRect.xMax = _mousePos.x;
            if (_mousePos.y < _scrollRect.yMin) _scrollRect.yMin = _mousePos.y;
            else if (_mousePos.y > _scrollRect.yMax) _scrollRect.yMax = _mousePos.y;
        }

		private void DrawStartOutline()
		{
            if (_config.Positions.Length > 0)
			{
                Rect rect = _config.Positions[0];
				rect.xMin-=2;
				rect.yMin-=2;
				rect.xMax+=2;
				rect.yMax+=2;
				GUI.color = Color.green;
				GUI.Box(rect, "");
			}
		}
	}
}
#endif
