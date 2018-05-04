using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Ambition
{
	public class RoomGraphic : Image
	{
		private UIVertex[][] _polys=null;

		override protected void Awake()
		{
			base.Awake();
			useLegacyMeshGeneration = false;
		}

		public RoomVO Room
		{
			set {
				if (value != null && value.Vertices != null && _polys == null)
		    	{
					float scale = AmbitionApp.GetModel<MapModel>().MapScale;
					UIVertex v = UIVertex.simpleVert;
					int[] bounds = value.Bounds;
					int xMin = bounds[0];
					int yMin = bounds[1];
					int x;
					int y;
					int index;

					_polys = new UIVertex[1][]; // TODO: Are convex polygons a thing??
					_polys[0] = new UIVertex[value.Vertices.Length>>1];
					// If not, the room needs to be tesselated and the first index > 1.
		        	for (int i=(value.Vertices.Length>>1)-1; i>=0; i--)
		        	{
						index = i<<1;
						x = value.Vertices[index];
						y = value.Vertices[index+1];

						v.color = this.color;
						v.position = new Vector3(scale*(x - xMin), scale*(y - yMin), 0);
						v.uv0.Set(x, y);
						_polys[0][i] = v;
		 			}
		 		}
				SetAllDirty();
			}
		}

		protected override void OnPopulateMesh(VertexHelper vh)
	    {
			if (_polys != null)
			{
				vh.Clear();
				Array.ForEach(_polys, vh.AddUIVertexQuad);
			}
	    }
	}
}
