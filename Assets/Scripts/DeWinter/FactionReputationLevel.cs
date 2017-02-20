using System;
using Newtonsoft.Json;

namespace DeWinter
{
	public class FactionReputationLevel
	{
		[JsonProperty("reputation")]
		public int Reputation;

		[JsonProperty("confidence")]
		public int Confidence;

		[JsonProperty("vip")]
		public int PartyInviteImportance;

		[JsonProperty("text")]
		public int Description;
	}
}