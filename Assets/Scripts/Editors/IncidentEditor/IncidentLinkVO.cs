#if (UNITY_EDITOR)
using System;
using UnityEngine;
using UnityEditor;
using Util;

namespace Ambition
{
	public class IncidentLinkVO : IGraphLinkComponent
	{
		private const float INTERSECT = 5f;
		private const float ARROW_SIZE = 5f;
		public Action DeleteLink;
		public string Text;
		public int Probability;
		public IncidentNodeVO Start;
		public IncidentNodeVO End;
		public CommodityVO[] Rewards;
		public IncidentLinkVO(IncidentNodeVO start, IncidentNodeVO end)
		{
			Start = start;
			End = end;
		}

		public void OnMenu(GenericMenu menu)
		{
			menu.AddItem(new GUIContent("Delete Transition"), false, OnDeleteLink);
		}

		public void OnGUI(Vector2 tail, Vector2 head, bool selected)
		{
			Vector3 mid, dir, norm;
			Vector3[] lines;
			dir = (head-tail).normalized;
			norm = new Vector2(dir.y, -dir.x)*ARROW_SIZE;
			mid = (head + tail)*.5f;
			dir *= ARROW_SIZE;

			lines = new Vector3[]{
				mid - dir - norm,
				mid + dir,
				mid - dir + norm
			};
			Handles.color = selected?Color.blue:Color.black;
			Handles.DrawLine(tail, head);
			Handles.DrawAAConvexPolygon(lines);
		}
	
		public bool Intersect(Vector2 tail, Vector2 head, Vector2 MousePosition)
		{
			if (Start == null || End == null) return false;
			Vector2 p = MousePosition - tail;
			float P = p.sqrMagnitude;
			Vector2 l = head - tail;
			float L = l.sqrMagnitude;
			float dot = Vector2.Dot(p, l);
			return
				(dot >= 0) && (dot*dot/L <= L)
				&& (Math.Abs(P - (dot*dot/L)) <= INTERSECT*INTERSECT);
		}

		private void OnDeleteLink()
		{
			DeleteLink();
		}
	}
}
#endif
