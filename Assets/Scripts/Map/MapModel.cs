using System;
using System.Collections;
using Core;

namespace DeWinter
{
	public class MapModel : IModel
	{
		public MapVO Map;

		private RoomVO _room;
		public RoomVO Room
		{
			get { return _room; }
			set {
				_room = value;
				_room.Revealed = true;
			}
		}

		public MapModel (MapVO map)
		{
			Map = map;
			Room = map.Entrance;
		}

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

				// TODO: Make Accessories Configurable
				if(GameData.tonightsParty.playerAccessory != null)
		        {
		            if (GameData.tonightsParty.playerAccessory.Type() == "Cane")
		            {
		                chance += 10;
		            }
		        }

		        return Math.Min(chance, 100);
			}
		}
	}
}