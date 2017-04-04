using System;
using Newtonsoft.Json;

namespace DeWinter
{
	public class ReputationLevel
	{
		[JsonProperty("reputation")]
		public int Reputation;

		[JsonProperty("confidence")]
		public int Confidence;

		[JsonProperty("vip")]
		public int PartyInviteImportance;

		[JsonProperty("text")]
		public string Description;

		[JsonProperty("name")]
		public string Name;
	}
}