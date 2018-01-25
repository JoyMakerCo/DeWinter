using System;
using Newtonsoft.Json;

namespace Ambition
{
	public class NotableVO : GuestVO
	{
		[JsonProperty("Background")]
		public string Background;

		[JsonProperty("Faction")]
		public string Faction;

		[JsonProperty("Reputation")]
		public int Reputation;

		[JsonProperty("Wealth")]
		public int Wealth;

		[JsonProperty("Spouse")]
		public string Spouse;

		[JsonProperty("Celibate")]
		public bool Celibate=false;

		[JsonIgnore]
		public int Devotion;

		[JsonIgnore]
		public Acquaintance Impression=Acquaintance.Unacquainted;
	}
}