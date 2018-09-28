using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Ambition
{
	public class WallList
	{
		public class WallVO
		{
			public int[] Vertices;
			public RoomVO[] Rooms;
			public float[] Scalars;

			public WallVO(int[] vertices, RoomVO room1=null, RoomVO room2=null)
			{
				Vertices = vertices;
				Rooms = new RoomVO[]{room1, room2};
			}
		}
		
		private Vector2 _line;
		private int[] _origin;

		public List<WallVO> Walls;

		public WallList (int[] line)
		{
			_line = Vectorize(line).normalized;
			_origin = new int[]{line[0], line[1]};
		}

		// TODO: Stop checking collinearity. Form a room graph and build the rooms around it.
		public bool AddRoom(RoomVO room)
		{
			Vector2 vec;
			int[] verts = room.Vertices;
			int[] wallVerts = new int[4];
			int len = verts.Length;
			float[] scalars;
            for (int i=len-2; i>=0; i-=2)
            {
				wallVerts[0] = verts[i];
				wallVerts[1] = verts[i+1];
				wallVerts[2] = verts[(i+2)%len];
				wallVerts[3] = verts[(i+3)%len];
				vec = Vectorize(wallVerts);
				if (IsCollinear(vec))
				{
					if (verts[i] < verts[i+2])
					{
						scalars = new float[]{(float)(verts[i]-_origin[0])/_line[0], (float)(verts[i+2]-_origin[0])/_line[0]};
					}
					else
					{
						scalars = new float[]{(float)(verts[i+2]-_origin[0])/_line[0], (float)(verts[i]-_origin[0])/_line[0]};
					}
					List<WallVO> walls = Walls.FindAll(w=>w.Scalars[1] >= scalars[0] && w.Scalars[0] <= scalars[1]);
					WallVO wall=new WallVO(wallVerts,room);
					wall.Scalars = scalars;
					walls.Sort();
					if (walls[0].Scalars[0] < scalars[0])
					{
						wall.Scalars[0] = walls[0].Scalars[1];
						walls[0].Scalars[1] = scalars[0];

						wall.Vertices[0] = walls[0].Vertices[2];
						wall.Vertices[1] = walls[0].Vertices[3];
						walls[0].Vertices[2] = wallVerts[0];
						walls[1].Vertices[3] = wallVerts[1];

						wall.Rooms[1] = walls[0].Rooms[0];

//						Walls.Insert(Walls.in, wall);
//						w.Scalars[1] = scalars[0];
//						w.Vertices[2] = (int)Math.Round(_origin[0] + _line[0]*w.Scalars[1]);
//						w.Vertices[3] = (int)Math.Round(_origin[1] + _line[1]*w.Scalars[1]);
//						index = Walls.IndexOf(w);
					}
//					if (walls[walls.Length-1].Scalars[1] > scalars[1])
//					{
//						// Copy logic from above
//					}
//					for (int i=walls.Count-1; i>=0; i--)
//					{
//						w.Rooms[1] = room;
//					}
//					 = Walls.Find(w=>w.Scalars[0]==scalars[0]);
//					if (wall == null)
//					{
//						wall
//						index = Walls.FindIndex(w=>w.Scalars[0] > scalars[0]);
//						if (index < 0) Walls.Add(wall);
//						else Walls.Insert(index, wall);
//					}
					return true;
				}
			}
			return false;
		}

		private bool IsCollinear(Vector2 vec)
		{
			float dot = Vector2.Dot(_line, vec.normalized);
            return dot*dot - 1f < .00001;
		}

		private Vector2 Vectorize(int[] line)
		{
			return new Vector2((float)line[2] - (float)line[0], (float)line[3] - (float)line[1]);
		}
	}
}

