using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class MapView : MonoBehaviour
	{
		public Texture2D Tile;
		public Texture2D Wall;
		public GameObject MapHolder;

		private MapVO _map;

		void Start() 
		{
			DeWinterApp.Subscribe<MapVO>(HandleMapReady);
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