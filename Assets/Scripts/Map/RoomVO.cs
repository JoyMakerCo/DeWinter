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

        public bool Cleared = false;
        public bool Revealed = false;
        public bool Visited = false;

        [JsonProperty("background")]
		public string Background;

        [JsonProperty("rewards")]
        public CommodityVO [] Rewards; //Granted after finishing a room

        [JsonProperty("actions")]
        public CommodityVO[] Actions; //Granted when the player enters a room

        public GuestVO [] Guests;
		[JsonProperty("numGuests")]
		public int NumGuests
		{
			set { Guests = new GuestVO[value]; }
            get { return Guests != null ? Guests.Length : 0; }
		}

		public List<EnemyVO> Enemies;
		public string [] Features=new string[0];
        public bool Indoors = true; //Indoors vs. outdoors rooms haven't been implemented yet, but we need this here right now

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

        public int[] Bounds { get; private set; }

        public int[] _vertices;
		[JsonProperty("vertices")]
		public int[] Vertices
		{
			get { return _vertices; }
			set {
				_vertices = value;
				Bounds = new int[4];
				Bounds[0] = _vertices.Where((v,i)=>i%2==0).Min();
				Bounds[1] = _vertices.Where((v,i)=>i%2==1).Min();
				Bounds[2] = _vertices.Where((v,i)=>i%2==0).Max();
				Bounds[3] = _vertices.Where((v,i)=>i%2==1).Max();
			}
		}

		// Set in the map file or map generation 
		public RoomVO[] Doors;

        public override string ToString ()
		{
			return "[RoomVO: " + Name + "]";
		}
	}
}
