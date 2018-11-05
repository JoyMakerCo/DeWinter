using System;
using Newtonsoft.Json;

namespace Ambition
{
	public class ReputationLevel
	{
		[JsonProperty("reputation")]
		public int Reputation;

		[JsonProperty("vip")]
		public int PartyInviteImportance;

		[JsonProperty("text")]
		public string Description;

		[JsonProperty("title")]
		public string Title;
	}
}