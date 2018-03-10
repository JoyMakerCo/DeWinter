#if (UNITY_EDITOR)
using System;
using UnityEngine;
using UnityEditor;

namespace Ambition
{
	public class IncidentLinkVO : IGraphComponent
	{
		private const float INTERSECT = 5f;
		private const float ARROW_SIZE = 5f;

		public string Text;
		public int Probability;

		public IncidentNodeVO Start;
		public IncidentNodeVO End;
		public CommodityVO[] Rewards;
		protected Vector2 _connectionStart;
		protected Vector2 _connectionEnd;

		public IncidentLinkVO(IncidentNodeVO start, IncidentNodeVO end)
		{
			Start = start;
			End = end;
			Redraw();
		}

		public void OnGUI(bool selected)
		{
			Vector3 mid, dir, norm;
			Vector3[] lines;
			dir = (_connectionEnd-_connectionStart).normalized;
			norm = new Vector2(dir.y, -dir.x)*ARROW_SIZE;
			mid = (_connectionEnd + _connectionStart)*.5f;
			dir *= ARROW_SIZE;

			lines = new Vector3[]{
				mid - dir - norm,
				mid + dir,
				mid - dir + norm
			};
			Handles.color = selected?Color.blue:Color.black;
			Handles.DrawLine(_connectionStart, _connectionEnd);
			Handles.DrawAAConvexPolygon(lines);
		}
	
		public bool Intersect(Vector2 MousePosition)
		{
			if (Start == null || End == null) return false;
			Vector2 p = MousePosition - _connectionStart;
			float P = p.sqrMagnitude;
			Vector2 l = _connectionEnd - _connectionStart;
			float L = l.sqrMagnitude;
			float dot = Vector2.Dot(p, l);
			return
				(dot >= 0) && (dot*dot/L <= L)
				&& (Math.Abs(P - (dot*dot/L)) <= INTERSECT*INTERSECT);
		}

		public void Redraw()
		{
			if (Start != null && End != null)
			{
				_connectionStart = Start.Rect.center;
				_connectionEnd = End.Rect.center;
			}
		}
	}
}
#endif
