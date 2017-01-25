using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DeWinter
{
	public struct Door
	{
		public RoomVO Room;
		public Vector2 Coords;

		public Door(RoomVO room, Vector2 coords)
		{
			Room = room;
			Coords = coords;
		}
	}

	public class RoomVO
	{
		public string Name;
	    public int Difficulty; //Difficulty 1-5
	    public bool Cleared=false;
	    public bool Revealed=false;
	    public Reward [] Rewards;
		public Guest [] Guests;
	    public string [] Features;

	    // Drawing instructions
	    public string Style;
	    public TileType[] Layout;
	    public Door [] Doors;

	    public RoomVO(string name=null)
	    {
	    	Name = name;
	    	Rewards = new List<Reward>();
	    	Guests = new List<Guest>();
	    	Features = new List<string>();
	    }
	}
}