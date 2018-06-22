#if (UNITY_EDITOR)

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Util;

namespace Util
{
	public class GraphEditorWindow<T> : EditorWindow
	{
		public Action<T, Vector2, bool> DrawNode;
		public Func<T, Vector2, Vector2> Intersect;

		protected const string DEFAULT_TITLE = "Directed Graph Editor";
		private const float ARROW_SIZE = 5f;
		protected DirectedGraph<T> _graph;
		protected SerializedObject _graphObject;

		private Vector2 _scroll;
		private Rect _scrollRect;
		private Vector2 _mousePos;		
		private bool _dragging=false;
		private bool _dirty=false;
		private T _selection;
		private T _selectedLink;
		
		public static void Show(DirectedGraph<T> graph, string title=null)
		{
			if (title == null) title=DEFAULT_TITLE;
			EditorWindow.GetWindow<GraphEditorWindow<T>>(title).SetGraph(graph);
		}

		public void SetGraph(DirectedGraph<T> graph)
		{
			if (graph != _graph)
			{
				_graph = graph;
				if (_graph != null)
				{
					if (_graph.Nodes==null) _graph.Nodes = new T[0];
					if (_graph.Links==null) _graph.Links = new DirectedGraphLink[0];
					// List<Vector2> positions = _graph.Positions == null ? new List<Vector2>() : new List<Vector2>(_graph.Positions);
					// positions.AddRange(new Vector2[_graph.Nodes.Length-positions.Count]);
					// _graph.Positions = positions.ToArray();
				}
				// _graphObject = (_graph != null ? new SerializedObject(_graph) : null);
			}
		}

	    void OnSelectionChange()
	    {
	    	OnDestroy();
	    }

		void OnDestroy()
		{
			_graph = null;
			_graphObject = null;
			titleContent.text = DEFAULT_TITLE;
			Repaint();
		}

		void OnGUI()
		{
			if (_graphObject != null)
			{
				_graphObject.Update();
				if (DrawNode != null)
				{
					for (int i=_graph.Nodes.Length-1; i>=0; i--)
					{
						// DrawNode(_graph.Nodes[i], _graph.Positions[i], _graph.Nodes[i].Equals(_selection));
					}
				}
				RenderLinks();
			}
		}

		protected void RenderLinks()
		{
			if (_graph != null)
			{
				Vector3 mid, dir, norm;
				Vector3[] lines;
				Vector2 head = new Vector2(), tail = new Vector2();
				T[] links;
				foreach(T node in _graph.Nodes)
				{
					// tail = _graph.GetPosition(node);
					links = _graph.GetNeighbors(node);
					foreach (T link in links)
					{
						// head = _graph.GetPosition(link);
						dir = (head-tail).normalized;
						norm = new Vector2(dir.y, -dir.x)*ARROW_SIZE;
						mid = (head + tail)*.5f;
						dir *= ARROW_SIZE;

						lines = new Vector3[]{
							mid - dir - norm,
							mid + dir,
							mid - dir + norm
						};
						Handles.color =(_selection.Equals(node) && _selectedLink.Equals(link))?Color.blue:Color.black;
						Handles.DrawLine(head, tail);
						Handles.DrawAAConvexPolygon(lines);
					}
				}
			}
		}
	}
}

#endif