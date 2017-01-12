using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Util;

public abstract class GraphEditorWindow<T> : EditorWindow
{
	protected struct NodeViewData<U>
	{
		public U Data;
		public Rect Rect;
		public NodeViewData(U data, Rect rect)
		{
			Data = data;
			Rect = rect;
		}
	}

	protected DirectedGraph<T> _graph;
	protected List<NodeViewData<T>> _nodeViews;

	protected void InitGraphData(string fileName)
	{
	// TODO: Read Graph data from a json file
	}

	void OnGUI() {
//		Handles.BeginGUI();
//        Handles.DrawBezier(windowRect.center, windowRect2.center, new Vector2(windowRect.xMax + 50f,windowRect.center.y), new Vector2(windowRect2.xMin - 50f,windowRect2.center.y),Color.red,null,5f);
//        Handles.EndGUI();

        BeginWindows();
		for (int i=_nodeViews.Count-1; i>=0; i--)
        {
			GUI.Window (i, _nodeViews[i].Rect, RenderNodeWindow, "Node " + i.ToString());
        }

        EndWindows();
    }

    void Awake()
    {
    }

	protected abstract void RenderNode(T node);

	protected void RenderConnections(T node)
	{
//		T[] connections = _graph.GetConnectionsFrom(node);

	}

	void RenderNodeWindow (int nodeID) 
    {
        GUI.DragWindow();
    }
}