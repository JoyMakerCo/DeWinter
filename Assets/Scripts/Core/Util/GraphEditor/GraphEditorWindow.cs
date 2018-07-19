#if (UNITY_EDITOR)

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Util;

namespace Util
{
	public class GraphEditorWindow : EditorWindow
	{
		protected const string DEFAULT_TITLE = "Graph Editor";
		protected DirectedGraphObject _graphObject;
		protected SerializedObject _serializedObject;
		private Vector2 _scroll;
		private Rect _scrollRect;
		private Vector2 _mousePos;		
		private bool _dragging=false;
		protected int _selection;
		protected bool _selectionIsNode;
		
		public static void Show<W>(DirectedGraphObject graphObject, string title=null) where W:GraphEditorWindow
		{
            if (graphObject == null) return;
			if (title == null) title=DEFAULT_TITLE;
			W window = EditorWindow.GetWindow<W>(title);
			window._graphObject = graphObject;
			window._serializedObject = new SerializedObject(graphObject);
		}

		public static void Show(DirectedGraphObject graphObject, string title=null)
		{
			Show<GraphEditorWindow>(graphObject, title);
		}

		protected bool Move(int nodeIndex, Vector2 offset)
		{
			SerializedProperty prop = GetElementAt(GetPositions(), nodeIndex);
			if (prop != null) prop.vector2Value += offset;
			return prop != null;
		}

		protected SerializedProperty GetGraph()
		{
			return _serializedObject != null
				? _serializedObject.FindProperty("Graph")
				: null;
		}

		protected SerializedProperty GetNodes()
		{
			SerializedProperty graph = GetGraph();
if (graph == null) Debug.Log("NO GRAPH!");
			return graph != null ? graph.FindPropertyRelative("Nodes") : null;
		}

		protected SerializedProperty GetLinks()
		{
			SerializedProperty graph = GetGraph();
			return graph != null ? graph.FindPropertyRelative("Links") : null;
		}

		protected SerializedProperty GetPositions()
		{
			return _serializedObject != null
				? _serializedObject.FindProperty("Positions")
				: null;
		}

		protected SerializedProperty GetLinkData()
		{
			SerializedProperty graph = GetGraph();
			return graph != null ? graph.FindPropertyRelative("LinkData") : null;
		}

		protected SerializedProperty GetElementAt(SerializedProperty list, int index)
		{
			return list != null && index >= 0 && index < list.arraySize
				? list.GetArrayElementAtIndex(index)
				: null;
		}

		protected bool DeleteNode(int nodeIndex)
		{
			if (!DeleteIndex(GetNodes(), nodeIndex)) return false;
			Vector2Int offset = new Vector2Int(0,0);
			SerializedProperty links = GetLinks();
			SerializedProperty link;
			if (links == null) return true;
			for (int i=links.arraySize-1; i>=0; i--)
			{
				link = links.GetArrayElementAtIndex(i);
				if (link.vector2IntValue.x == nodeIndex
					|| link.vector2IntValue.y == nodeIndex)
				{
					DeleteLink(i);
				}
				else
				{
					offset.x = (link.vector2IntValue.x > nodeIndex) ? 1 : 0;
					offset.y = (link.vector2IntValue.y > nodeIndex) ? 1 : 0;
					link.vector2IntValue -= offset;
				}
			}
			return true;
		} 

		protected bool DeleteLink(int linkIndex)
		{
			if (!DeleteIndex(GetLinks(), linkIndex)) return false;
			DeleteIndex(GetLinkData(), linkIndex);
			return true;
		}

		protected SerializedProperty AddNode(Vector2 position, int fromNode = -1)
		{
			SerializedProperty prop = AddNewElement(GetNodes());
			if (prop == null) return null;
			
			SerializedProperty pos = AddNewElement(GetPositions());
			pos.vector2Value = position;
			return prop;
		}

		protected SerializedProperty Link(int fromNode, int toNode)
		{
			SerializedProperty prop = GetNodes();
			if (prop != null
				&& fromNode >= 0
				&& fromNode < prop.arraySize
				&& toNode >= 0
				&& toNode < prop.arraySize)
			{
				AddNewElement(GetLinkData());
				prop = AddNewElement(GetLinks());
				if (prop != null) prop.vector2IntValue = new Vector2Int(fromNode, toNode);
			}
			return prop;
		}

		protected SerializedProperty AddNewElement(SerializedProperty list)
		{
Debug.Log(list == null ? "NO LIST" : "LIST");
			if (list == null) return null;
			list.arraySize++;
			list.InsertArrayElementAtIndex(list.arraySize-1);
			return list.GetArrayElementAtIndex(list.arraySize-1);
		}

		protected bool DeleteIndex(SerializedProperty list, int index)
		{
			if (list == null || index >= list.arraySize) return false;
			if (list.GetArrayElementAtIndex(index) == null)
				list.DeleteArrayElementAtIndex(index);
			list.DeleteArrayElementAtIndex(index);
			return true;
		}

	    void OnSelectionChange()
	    {
	    	OnDestroy();
	    }

		void OnDestroy()
		{
			_serializedObject = null;
			_graphObject = null;
			Repaint();
		}

		void OnGUI()
		{
			_scroll = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scroll, _scrollRect);
			if (_graphObject != null)
			{
				switch(Event.current.type)
				{
					case EventType.MouseDown:
						_mousePos = Event.current.mousePosition;
						_dragging = _graphObject.Intersect(_mousePos, out _selection, out _selectionIsNode) && _selectionIsNode && Event.current.clickCount < 2;
						if (Event.current.button == 0 && Event.current.clickCount > 1 && _selection < 0)
							AddNode(_mousePos);
					break;
					case EventType.MouseDrag:
						if (_dragging)
						{
							Move(_selection, Event.current.mousePosition - _mousePos);
							_mousePos = Event.current.mousePosition;
						}
						break;

					case EventType.MouseUp:
						_dragging = false;
						_mousePos = Event.current.mousePosition;
						break;
				}
				_serializedObject.ApplyModifiedProperties();
			}
			GUI.EndScrollView();
		}

		private void AdjustScrollRect(Rect rect)
		{
			if (rect.xMin < _scrollRect.xMin) _scrollRect.xMin = rect.xMin;
			else if (rect.xMax > _scrollRect.xMax) _scrollRect.xMax = rect.xMax;
			if (rect.yMin < _scrollRect.yMin) _scrollRect.yMin = rect.yMin;
			else if (rect.yMax > _scrollRect.yMax) _scrollRect.yMax = rect.yMax;
		}
	}
}

#endif
