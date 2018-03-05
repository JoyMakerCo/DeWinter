using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Ambition
{
	public class RoomVO
	{
		[JsonProperty("name")]
		public string Name;

		[JsonProperty("difficulty")]
	    public int Difficulty; //Difficulty 1-5

		[JsonProperty("moveThroughChance")]
		public int MoveThroughChance = -1; // -1 indicates a value not yet assigned

		public MapVO Map;
	    public bool Cleared=false;

		public CommodityVO [] Rewards;
		public GuestVO [] Guests;
		public List<EnemyVO> Enemies;
		public string [] Features=new string[0];

		public bool HostHere
		{
			get {
				return (Features != null) && (Array.IndexOf(Features, PartyConstants.HOST) >= 0);
			}
		}

		public bool IsAdjacentTo(RoomVO room)
		{
			return Array.IndexOf(Doors, room) >= 0;
		}

		[JsonProperty("vertices")]
		public int[] Vertices;

		// Set in the map file or map generation 
		public RoomVO[] Doors;

		private int[] _bounds = null;
		internal int[] GetBounds()
		{
			if (_bounds == null)
			{
				_bounds = new int[4];
				_bounds[0] = Vertices.Where((v,i)=>i%2==0).Min();
				_bounds[1] = Vertices.Where((v,i)=>i%2==1).Min();
				_bounds[2] = Vertices.Where((v,i)=>i%2==0).Max();
				_bounds[3] = Vertices.Where((v,i)=>i%2==1).Max();
			}
			return _bounds;
		}

		public override string ToString ()
		{
			return "[RoomVO: " + Name + "]";
		}
	}
}
