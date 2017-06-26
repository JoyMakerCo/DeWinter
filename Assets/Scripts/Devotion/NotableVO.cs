using System;
using Newtonsoft.Json;

namespace Ambition
{
	public class NotableVO
	{
		[JsonProperty("Name")]
		public string Name;

		[JsonProperty("Gender")]
		public Gender Gender;

		[JsonProperty("Background")]
		public string Background;

		[JsonProperty("Title")]
		public string Title;

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

		[JsonProperty("Opinion")]
		public float MaxOpinion;

		[JsonIgnore]
		public float Opinion;

		[JsonProperty("Interest")]
		public float MaxInterest;

		[JsonIgnore]
		public float Interest;

		[JsonProperty("Disposition")]
		public string Disposition;

		[JsonIgnore]
		public int Devotion;

		[JsonIgnore]
		public Acquaintance Impression=Acquaintance.Unacquainted;
	}
}