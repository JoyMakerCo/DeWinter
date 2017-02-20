using System;
using System.Collections;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class MapModel : DocumentModel
	{
		private RoomVO _room;
		private MapVO _map;

		[JsonProperty("roomAdjectiveList")]
		public string [] RoomAdjectives;

		[JsonProperty("roomNounList")]
		public string [] RoomNames;

		public MapVO Map
		{
			get { return _map; }
			set {
				_map = value;
				_room = (_map != null) ? _map.Entrance : null;
			}
		}

		public RoomVO Room
		{
			get { return _room; }
			set {
				_room = value;
				_room.Revealed = true;
			}
		}

		public MapModel() : base("MapData") {}

		public bool IsEntrance
		{
			get { return Room == Map.Entrance; }
		}

		public bool HostHere
		{
			get {
				return Array.IndexOf(Room.Features, PartyConstants.HOST) >= 0;
			}
		}

		public int MoveThroughChance
		{
			get {
				if (Room == null) return 0; // Early out

				int chance = 90 - (Room.Cleared ? 0 : Room.Difficulty * 10);
				InventoryModel inventory = DeWinterApp.GetModel<InventoryModel>();
				ItemVO accessory;

// TODO: Implement Item states
				if(inventory.Equipped.TryGetValue("accessory", out accessory)
					&& accessory.Name == "Cane")
		        {
	                chance += 10;
		        }

		        return Math.Min(chance, 100);
			}
		}
	}
}