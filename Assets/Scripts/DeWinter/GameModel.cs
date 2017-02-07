using System;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class GameModel : DocumentModel
	{
		public GameModel() : base("GameData") {}

		[JsonProperty("factionList")]
		public FactionVO[] Factions;

		[JsonProperty("roomAdjectiveList")]
		public string[] RoomAdjectives;

		[JsonProperty("roomNounList")]
		public string[] RoomNouns;

		[JsonProperty("dispositionList")]
		public Disposition[] Dispositions;

		[JsonProperty("femaleTitleList")]
		public string[] FemaleTitles;

		[JsonProperty("maleTitleList")]
		public string[] MaleTitles;

		[JsonProperty("femaleFirstNameList")]
		public string[] FemaleNames;

		[JsonProperty("maleFirstNameList")]
		public string[] MaleNames;

		[JsonProperty("lastNameList")]
		public string[] LastNames;	

	}
}