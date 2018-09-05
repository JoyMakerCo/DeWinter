using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class MapGraphic : Image
	{

		private const int DOOR_SIZE = 2;
		private const float THICKNESS = 0.05f;

		private UIVertex[][] _walls;
		private MapModel _model;
		private float _scale;

		override protected void Start()
		{
			base.Start();
			_model = AmbitionApp.GetModel<MapModel>();
			_scale = _model.MapScale;
			if (_model.Map != null) HandleMap(_model.Map);
			else AmbitionApp.Subscribe<MapVO>(HandleMap);
		}

		private void HandleMap(MapVO map)
		{
			if (map != null)
			{
				DrawMap(_model.Map);
				AmbitionApp.Unsubscribe<MapVO>(HandleMap);
			}
		}

		private void DrawMap(MapVO map)
		{
			List<UIVertex[]> quads = new List<UIVertex[]>();
			foreach (RoomVO room in map.Rooms)
			{
				if (room != null && room.Vertices != null)
				{
					int len=room.Vertices.Length;
					for (int i=0; i<len; i+=2)
					{
						DrawLine(room.Vertices[i], room.Vertices[i+1], room.Vertices[(i+2)%len], room.Vertices[(i+3)%len], quads);
					}
				}
			}
			_walls = quads.ToArray();
		}

		private void DrawLine(int x0, int y0, int x1, int y1, List<UIVertex[]> quads, bool door=false)
		{
			UIVertex[] result = new UIVertex[]{UIVertex.simpleVert, UIVertex.simpleVert, UIVertex.simpleVert, UIVertex.simpleVert};
			float[] ends = new[]{_scale*x0, _scale*y0, _scale*x1, _scale*y1};
			Vector2 norm0 = new Vector2(x1-x0, y1-y0).normalized*THICKNESS*_scale;
			Vector2 norm1 = new Vector2(y0-y1, x1-x0).normalized*THICKNESS*_scale;
			for(int i=result.Length-1; i>=0; i--) result[i].color = Color.black;
			result[0].position.Set(ends[0]-norm0.x-norm1.x, ends[1]-norm0.y-norm1.y,0f);
			result[1].position.Set(ends[0]-norm0.x-norm1.x, ends[3]+norm0.y+norm1.y,0f);
			result[2].position.Set(ends[2]+norm0.x+norm1.x, ends[3]+norm0.y+norm1.y,0f);
			result[3].position.Set(ends[2]+norm0.x+norm1.x, ends[1]-norm0.y-norm1.y,0f);
			quads.Add(result);
		}

		protected override void OnPopulateMesh(VertexHelper vh)
	    {
			if (_walls != null)
			{
				vh.Clear();
				Array.ForEach(_walls, vh.AddUIVertexQuad);
			}
	    }
	}
}

