using System;
using Newtonsoft.Json;

namespace Ambition
{
	public class GuestDifficultyVO
	{
		[JsonProperty("opinion")]
		public int[] Opinion;
	}
}
