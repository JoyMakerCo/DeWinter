#if (UNITY_EDITOR)
using System;
using System.Collections.Generic;
using UnityEngine;  
using UnityEditor;

namespace Ambition
{
	public class IncidentNodeVO : IGraphComponent
	{
		private const float NODE_WIDTH = 200f;
		private const float NODE_HEIGHT = 50f;
		private static readonly Color NORMAL_COLOR = Color.white;
		private static readonly Color INIT_COLOR = Color.green;
		private static readonly Color NORMAL_BORDER = Color.black;
		private static readonly Color SELECTED_BORDER = Color.yellow;

		public string Text;

		public bool IsStart;

		private Rect _rect=new Rect(0,0,NODE_WIDTH, NODE_HEIGHT);
		public Rect Rect
		{
			get { return _rect; }
		}

		public Vector2 Position
		{
			set { _rect.center = value; }
			get { return _rect.center; }
		}

		public IncidentNodeVO(string text)
		{
			Text = text;
		}

		public bool Intersect(Vector2 MousePosition)
		{
			return _rect.Contains(MousePosition);
		}

		public void OnGUI(bool selected)
		{
			GUI.color = selected?Color.yellow:Color.white;
			GUI.Box(_rect, Text);
		}
	}
}
#endif
