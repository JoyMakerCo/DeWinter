#if (UNITY_EDITOR)
using System;
using System.Collections.Generic;
using UnityEngine;  
using UnityEditor;
using Util;

namespace Ambition
{
	public class IncidentNodeVO : IGraphNodeComponent
	{
		private const float NODE_WIDTH = 200f;
		private const float NODE_HEIGHT = 50f;
		private static readonly Color NORMAL_COLOR = Color.white;
		private static readonly Color INIT_COLOR = Color.green;
		private static readonly Color NORMAL_BORDER = Color.black;
		private static readonly Color SELECTED_BORDER = Color.yellow;

		public Action DeleteNode;
		public string Text;

		private Rect _rect=new Rect(0,0,NODE_WIDTH, NODE_HEIGHT);
		public Rect Rect
		{
			get { return _rect; }
		}

		public Vector2 Position
		{
			get { return _rect.center; }
			set { _rect.center = value; }
		}

		public IncidentNodeVO(string text)
		{
			Text = text;
		}

		public void OnMenu(GenericMenu menu)
		{
			menu.AddItem(new GUIContent("Delete Moment"), false, OnDeleteNode);
		}

		public bool Intersect(Vector2 position, Vector2 MousePosition)
		{
			_rect.center = position;
			return _rect.Contains(MousePosition);
		}

		public void OnGUI(Vector2 position, bool selected)
		{
			GUI.color = selected?Color.yellow:Color.white;
			_rect.center = position;
			GUI.Box(_rect, Text);
		}

		private void OnDeleteNode()
		{
			DeleteNode();
		}
	}
}
#endif
