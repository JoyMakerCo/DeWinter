using System;
using Core;

namespace DeWinter
{
	public class MapModel : DocumentModel
	{
		public MapModel () : base ("MapData") {}

		[JsonProperty("roomAdjectiveList")]
		public string[] RoomAdjectives;

		[JsonProperty("roomNounList")]
		public string[] RoomNouns;
	}
}