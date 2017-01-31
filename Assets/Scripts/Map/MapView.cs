using System;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class MapView : MonoBehaviour
	{
		public Texture2D Tile;
		public Texture2D Wall;
		public GameObject MapHolder;

		private MapVO _map;

		void Start() 
		{
			DeWinterApp.Subscribe<MapVO>(MapMessage.MAP_READY, HandleMapReady);
			DeWinterApp.Subscribe<RoomVO>(MapMessage.SELECT_ROOM, HandleSelectRoom);
		}

		private void HandleMapReady(MapVO map)
		{
			_map = map;
			DrawRoom(_map.Entrance, null);
		}

		private void HandleSelectRoom(RoomVO room)
		{
			
		}

		private void DrawRoom(RoomVO room, RoomVO fromRoom)
		{
			
		}
	}
}