using System;
using Newtonsoft.Json;

namespace Ambition
{
	public struct FactionLevelVO
	{
		[JsonProperty("requirement")]
		public int Requirement;

        [JsonProperty("deck_bonus")]
        public int DeckBonus;

        [JsonProperty("text")]
		public string Text;

		[JsonProperty("importance")]
		public int Importance;

		[JsonProperty("largestAllowableParty")]
		public int LargestAllowableParty;
	}
}
