using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;

namespace Ambition
{
	public class CharacterModel : DocumentModel
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

		[JsonProperty("characters")]
		public NotableVO[] Notables;

		public CharacterModel() : base("CharacterData") {}
	}
}
