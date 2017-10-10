using System;
using Newtonsoft.Json;

namespace Ambition
{
	public struct FactionLevelVO
	{
		[JsonProperty("requirement")]
		public int Requirement;

		[JsonProperty("text")]
		public string Text;

		[JsonProperty("confidence")]
		public int Confidence;

		[JsonProperty("importance")]
		public int Importance;

		[JsonProperty("largestAllowableParty")]
		public int LargestAllowableParty;
	}
}
