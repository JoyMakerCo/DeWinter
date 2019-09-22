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

        [JsonIgnore]
        public Dictionary<string, CharacterVO> Characters = new Dictionary<string, CharacterVO>();

        [JsonProperty("Characters")]
        private CharacterVO[] _characters
        {
            set => Array.ForEach(value, c => Characters.Add(c.Name, c));
        }


        public CharacterModel() : base("CharacterData") {}
	}
}
