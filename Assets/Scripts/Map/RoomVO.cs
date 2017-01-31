using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DeWinter
{
	public class RoomVO
	{
		public string Name;
	    public int Difficulty; //Difficulty 1-5
	    public bool Cleared=false;
	    public bool Revealed=false;
		public Reward [] Rewards;
		public Guest [] Guests;
		public List<Enemy> Enemies;
		public string [] Features;
		public RoomVO [] Neighbors;

	    // Drawing instructions
	    public string Style;
	    public TileType[] Layout;

	    public RoomVO(string name=null)
	    {
	    	Name = name;
	    }
	}
}