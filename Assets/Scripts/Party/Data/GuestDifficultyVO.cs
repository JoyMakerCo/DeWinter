using System;
using Newtonsoft.Json;

namespace Ambition
{
	public class GuestDifficultyVO
	{
		[JsonProperty("opinion")]
		public int[] Opinion;

		[JsonProperty("interest")]
		public int[] Interest;

		[JsonProperty("max_interest")]
		public int[] MaxInterest;
	}
}
