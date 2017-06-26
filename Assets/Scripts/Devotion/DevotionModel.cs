using System.Collections.Generic;
using Newtonsoft.Json;
using Core;

using UnityEngine;

namespace Ambition
{
	public class DevotionModel : DocumentModel
	{
		[JsonProperty("SeductionMod")]
		public int SeductionModifier;

		[JsonProperty("SeductionAltMod")]
		public int SeductionAltModifier;

		[JsonProperty("SeductionMarriageMod")]
		public int SeductionMarriedModifier;

		[JsonProperty("SeductionTimeMod")]
		public int SeductionTimeModifier;

		[JsonProperty("SeductionDevotion")]
		public int SeductionDevotion;

		[JsonProperty("Notables")]
		public Dictionary <string, NotableVO> Notables;

		public DevotionModel() : base("Devotion") {}
	}
}