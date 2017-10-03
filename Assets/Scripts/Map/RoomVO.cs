using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

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

		[JsonProperty("timer")]
		public float TurnTimer; // Custom TurnTimer?

	    public bool Cleared=false;
	    public bool Revealed=false;

		public RewardVO [] Rewards;
		public GuestVO [] Guests;
		public List<EnemyVO> Enemies;
		public string [] Features;

		public bool HostHere
		{
			get {
				return (Features != null) && (Array.IndexOf(Features, PartyConstants.HOST) >= 0);
			}
		}

		[JsonProperty("coords")] // Temp property until room drawing is better defined
		public int[] Coords = new int[2];

	    // Drawing instructions
		// TODO: Expand the visual description of the room
//		[JsonProperty("shape")]
//		public Vector2[] Shape;

	    public string Style;

	    public RoomVO[] Neighbors;

	    public bool IsNeighbor(RoomVO room)
	    {
	    	return Neighbors != null && Array.IndexOf(Neighbors, room) >= 0;
	    }
	}
}
