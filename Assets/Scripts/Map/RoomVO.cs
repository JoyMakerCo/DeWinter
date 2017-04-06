using UnityEngine;
using System;
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
		public RoomVO [] Neighbors; // Starting North, Clockwise
		public float TurnTimer; // Custom TurnTimer?
public bool IsTutorial=false; //TODO: Remove haxx
		public bool IsImpassible=false;

// TODO: The map data structure will take care of this
public bool IsEntrance=false;

		public bool HostHere
		{
			get {
				return (Features != null) && (Array.IndexOf(Features, PartyConstants.HOST) >= 0);
			}
		}
		public int MoveThroughChance
		{
			get {
				if (IsImpassible && !Cleared) return 0;
				int chance = 90 - (Cleared ? 0 : Difficulty * 10);
				InventoryModel inventory = DeWinterApp.GetModel<InventoryModel>();
				ItemVO accessory;

// TODO: Implement Item states
				if(inventory.Equipped.TryGetValue("accessory", out accessory)
					&& accessory.Name == "Cane")
		        {
	                chance += 10;
		        }

		        return (chance < 100) ? chance : 100;
			}
		}

	    // Drawing instructions
		// TODO: Expand the visual description of the room
		public Vector2[] Shape;
	    public string Style;

	    public bool IsNeighbor(RoomVO room)
	    {
	    	return (Neighbors != null) && (Array.IndexOf(Neighbors, room) >= 0);
	    }
	}
}